using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xeovis.Base;
using Xeovis.Domain;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Globals;

namespace Xeovis.UI.ViewModels.DialogsViewModels;

public partial class mctcCalibrationViewModel : ViewModelBase
{
    public mctcCalibrationViewModel()
    {
        
    }

    public void Calculate(string fileName, string mctcDetails)
    {
        var sr = new StreamReader(fileName);
        var content = sr.ReadToEnd();
        sr.Close();
        
        var lines = content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        var allRange = Enumerable.Range(0, lines.Count);

        var gaussmcIndex = allRange.FirstOrDefault(i => lines[i].Trim().StartsWith("gaussmc"));
        

        var batchLinesIndexes = allRange.Where(i => lines[i].Trim().StartsWith("batch ")).ToList();
        var batchStartIndex = batchLinesIndexes[0] - 1;
        var batchEndIndex = batchLinesIndexes.Last();

        var beforeBatch = lines.Take(batchStartIndex).ToList();
        var batchLines = lines.GetRange(batchStartIndex, batchEndIndex + 1 - batchStartIndex).ToList();
        var afterBatch = lines.TakeLast(lines.Count - 1 - batchEndIndex).ToList();
        var chunks = batchLines.Chunk(1200).ToList();

        foreach (var chunk in chunks)
        {
            var tempContent = string.Join(Environment.NewLine, beforeBatch) 
                              + Environment.NewLine + string.Join('\n', chunk) 
                              + Environment.NewLine + string.Join('\n', afterBatch) ;
            
            File.WriteAllText(fileName, tempContent);

            RunThermocalc(fileName);
        }
        
        
        File.WriteAllText(fileName, content);
    }
    
    public bool RunThermocalc(string _directory)
    {
        
        foreach (var process in Process.GetProcessesByName("tc360"))
        {
            process.Kill();
        }
        
        // string exePath = "/Users/mohammadhumand/Library/CloudStorage/" +
        //                  "GoogleDrive-mohammadhumand@gmail.com/" +
        //                  "My Drive/PhD @ Unimelb/PhD/PhD - Project/" +
        //                  "Project - Calculations/set_up_fitting/TC RUN/tc360";
        
        _directory = Path.GetDirectoryName(_directory) + "/tc360";
        
        /// Use ProcessStartInfo class
        ProcessStartInfo startInfo = new ProcessStartInfo();
        //startInfo.UseShellExecute = false;
        //startInfo.RedirectStandardOutput = true;
        // startInfo.FileName = _directory + "/tc360";
        startInfo.FileName = _directory;
        startInfo.WindowStyle = ProcessWindowStyle.Normal;
        startInfo.WorkingDirectory = Path.GetDirectoryName(_directory);
        //startInfo.Verb = "runas";

        bool result = false;

        try
        {
            // Start the process with the info we specified.
            // Call WaitForExit and then the using statement will close.
            using (Process exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
                result = exeProcess.HasExited;
                //if (result)
                //{
                //    var output = exeProcess.StandardOutput.ReadToEnd();
                //}
            }

            foreach (var process in Process.GetProcessesByName("tc360"))
            {
                process.Kill();
            }
        }
        catch
        {
            // Log error.
            result = false;
        }

        return result;
    }

    
    public List<XEOS> ReadAxFile(string fileName)
    {
        var sr = new StreamReader(fileName);
        var content = sr.ReadToEnd();
        var lines = content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        var allRange = Enumerable.Range(0, lines.Count);
        var verbatimIndexes = allRange.Where(i => lines[i].Trim() == "verbatim").ToList();
        verbatimIndexes.Add(allRange.Count() - 1);
        var splitters = new char[] { '(', ',', ')', ' ' };

        var xeosList = new List<XEOS>();

        for (int i = 0; i < verbatimIndexes.Count - 1; i++)
        {
            var xeosLines = lines.GetRange(verbatimIndexes[i + 1], verbatimIndexes[i + 2] - verbatimIndexes[i + 1]).ToList();

            var headerLine = new List<string>();
            var index = verbatimIndexes[i] - 1;
            while (true)
            {
                headerLine = lines[index].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (headerLine.Count > 0 && double.TryParse(headerLine.Last(), out _)) break;
                index--;
            }

            int.TryParse(headerLine[1], out int endmemberCount);
            var endmemberLines = xeosLines.Where(l => l.Trim().StartsWith("p(")).ToList();
            var wLines = xeosLines.Where(l => l.Trim().StartsWith("W(")).ToList();

            var xeos = new XEOS();
            xeos.PhaseNameId = PhaseNameList.GetNameSetIdByString(headerLine[3]);
            xeos.Name = headerLine[0];
            //xeos.PhaseNameSet = PhaseTypeExtensions.GetPhaseTypeByName(xeos.Name);

            // endmembers
            foreach (var emLine in endmemberLines)
            {
                var emParts = emLine.Split(splitters, StringSplitOptions.RemoveEmptyEntries).ToList();
                var endMember = new EndMember();
                endMember.Name = emParts[1];
                xeos.EndMembers.Add(endMember);
            }

            // energy parameters
            foreach (var wLine in wLines)
            {
                var wParts = wLine.Split(splitters, StringSplitOptions.RemoveEmptyEntries).ToList();

                var W = new EnergyParameter() { xeosId = xeos.Id };
                W.Endmember1 = xeos.EndMembers.FirstOrDefault(em => em.Name == wParts[1]);
                W.Endmember2 = xeos.EndMembers.FirstOrDefault(em => em.Name == wParts[2]);
                W.A = DoubleExtensions.GetDoubleValueFromText(wParts[3]);
                W.B = DoubleExtensions.GetDoubleValueFromText(wParts[4]);
                W.C = DoubleExtensions.GetDoubleValueFromText(wParts[5]);
                xeos.Ws.Add(W);
            }

            // alpha parameters
            var asfLineIndex = xeosLines.FindIndex(l => l.Trim() == "asf");
            if (asfLineIndex > 0)
            {
                foreach (var endmember in xeos.EndMembers)
                {
                    var alphaLine = xeosLines.GetRange(asfLineIndex, xeosLines.Count - asfLineIndex).Where(l => l.Trim().StartsWith(endmember.Name + " ")
                                                      || l.Trim().StartsWith("a(" + endmember.Name)).ToList()[0];
                    if (string.IsNullOrWhiteSpace(alphaLine)) continue;
                    var alphaParts = alphaLine.Split(splitters, StringSplitOptions.RemoveEmptyEntries).ToList();

                    if (alphaLine.Contains("("))
                    {
                        if (alphaParts.Count > 5) continue;
                        alphaParts.RemoveAt(0);
                    }
                    else
                        if (alphaParts.Count > 4) continue;

                    var alpha = new AlphaParameter();
                    alpha.Endmember = xeos.EndMembers.FirstOrDefault(em => em.Name == alphaParts[0]);
                    alpha.A = DoubleExtensions.GetDoubleValueFromText(alphaParts[1]);
                    alpha.B = DoubleExtensions.GetDoubleValueFromText(alphaParts[2]);
                    alpha.C = DoubleExtensions.GetDoubleValueFromText(alphaParts[3]);
                    xeos.Alphas.Add(alpha);
                }
            }

            // model variables
            var variableLines = xeosLines.Where(l => l.Trim().Contains($"({xeos.Name})")).ToList();
            foreach (var varLine in variableLines)
            {
                var variableLine = varLine.Trim();
                if (varLine.StartsWith("p") || varLine.StartsWith("W(")) continue;
                var varParts = varLine.Split(splitters, StringSplitOptions.RemoveEmptyEntries).ToList();

                var variable = new xeosVariable();
                variable.PhaseNameId = xeos.PhaseNameId;
                variable.NameId = xeosVariableNameList.GetVariableNameId(varParts[0], variable.PhaseNameId);
                variable.DefaultValue = DoubleExtensions.GetDoubleValueFromText(varParts[2]);
                xeos.Variables.Add(variable);
            }

            xeosList.Add(xeos);
            i++;
        }

        return xeosList;
    }

}