using DatabaseReader.Definitions;
using DatabaseReader.Enums;
using DatabaseReader.Helpers;
using DevExpress.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DatabaseReader
{
    public partial class FrmReadMctc : Form
    {
        private string directory;
        private List<string> _files;
        private List<Batch> _batches;
        private List<ModelVariable> _modelVariables;

        public FrmReadMctc()
        {
            InitializeComponent();
        }

        private void ReadMctc_Click(object sender, EventArgs e)
        {
            directory = "H:\\My Drive\\PhD @ Unimelb\\PhD\\PhD - Project\\Project - Calculations\\set_up_fitting\\TC RUN";
            if (!Directory.Exists(directory))
            {
                MessageBox.Show("Directory does not exist.");
                return;
            }
            else
                _files = Directory.GetFiles(directory).ToList();

            ReadBatchLines();
            ReadBatchBulks();
            ReadModelVariables();
            ReadMctcResults();
        }

        private void ReadBatchLines()
        {
            var lines = ReadFileLines("batchlines");
            if (lines == null || lines.Count == 0) return;

            _batches = new List<Batch>();
            for (int i = 0; i < lines.Count; i++)
            {
                // batch name
                var Line1 = ReadAndSplitLine(lines[i]);

                var batch = new Batch();
                int.TryParse(Line1[0], out int index);
                batch.Index = index;
                batch.Experiment = new Experiment(Line1[1]);
                batch.ExperimentalRun = new ExperimentalRun() { Name = Line1[2], Experiment = batch.Experiment };

                //batch data
                i++;
                var Line2 = ReadAndSplitLine(lines[i]);

                int varianceIndex = 0;
                for (int j = 0; j < Line2.Count; j++)
                {
                    int.TryParse(Line2[j], out int variance);
                    if (variance != 0)
                    {
                        varianceIndex = j;
                        break;
                    }
                }

                for (int j = 1; j < varianceIndex; j++)
                    batch.Phases.Add(PhaseTypeExtensions.GetPhaseTypeByName(Line2[j]));

                batch.ExperimentalRun.Pressure = DoubleExtensions.GetDoubleValueFromText(Line2[varianceIndex + 3]);
                batch.ExperimentalRun.Temperature = DoubleExtensions.GetDoubleValueFromText(Line2[varianceIndex + 4]);

                _batches.Add(batch);
            }
        }

        private void ReadBatchBulks()
        {
            var lines = ReadFileLines("batchbulks");
            if (lines == null || lines.Count == 0) return;

            var oxidesStr = ReadAndSplitLine(lines[0]);
            var oxides = new List<OxideType>();
            foreach (var oxideStr in oxidesStr)
                oxides.Add(EnumExtensions.GetValueFromDescription<OxideType>(oxideStr));

            for (int i = 1; i < lines.Count; i++)
            {
                var compStr = ReadAndSplitLine(lines[i], includeDot: false);
                var composition = new List<double>();
                for (int j = 1; j <= oxides.Count; j++)
                    composition.Add(DoubleExtensions.GetDoubleValueFromText(compStr[j]));

                _batches[i - 1].BulkOxides = oxides;
                _batches[i - 1].BulkComposition = composition;
            }
        }

        private void ReadModelVariables()
        {
            var lines = ReadFileLines("observations");
            if (lines == null || lines.Count == 0) return;

            lines = ReadAndSplitLine(lines[0], includeDot: false);
            _modelVariables = new List<ModelVariable>();
            for (int i = 0; i < lines.Count; i++)
            {
                var varData = lines[i].Split(new char[] { '(', ')','b','-' }).ToList();
                varData.Remove(l => string.IsNullOrWhiteSpace(l));

                var modelVariable = new ModelVariable();
                modelVariable.Name = varData[0];
                modelVariable.Phase = PhaseTypeExtensions.GetPhaseTypeByName(varData[1]);
                int.TryParse(varData[2], out int batchIndex);
                modelVariable.Batch = _batches.FirstOrDefault(b => b.Index == batchIndex);

                i++;
                modelVariable.ObsVal = DoubleExtensions.GetDoubleValueFromText(lines[i]);

                modelVariable.Batch.ModelVariables.Add(modelVariable);
                _modelVariables.Add(modelVariable);
            }
        }

        private void ReadMctcResults()
        {
            var lines = ReadFileLines("-dr");
            if (lines == null || lines.Count == 0) return;

            for (int i = 0; i < _batches.Count; i++)
            {
                var batch = _batches[i];
                string key = $"(* {batch.Index} *)";
                var batchLinesIndx = Enumerable.Range(0, lines.Count).Where(j => lines[j].Contains(key)).ToList();
                if (batchLinesIndx.Count != 2) continue;

                var splitters = new char[] { '{', '}', '*', '(', ')', '\t', '"', ',' , ';'};

                // what data is in lines[batchLinesIndx[0]]? phase proportions?
                var line1 = lines[batchLinesIndx[0] + 1];
                var modVarNameLines = line1.Split(splitters).ToList();
                modVarNameLines.Remove(l => string.IsNullOrWhiteSpace(l));

                var line2 = lines[batchLinesIndx[1]];
                var modVarValueLines = line2.Split(splitters).ToList();
                modVarValueLines.Remove(l => string.IsNullOrWhiteSpace(l));

                if (modVarValueLines.Count <= 3)
                {
                    batch.IsFailed = true;
                    continue;
                }

                int counter = 1;
                for (int j = 0; j < modVarNameLines.Count; j++)
                {
                    var varName = modVarNameLines[j];
                    j++;
                    var varPhase = PhaseTypeExtensions.GetPhaseTypeByName(modVarNameLines[j]);
                    var modelVariable = batch.ModelVariables.FirstOrDefault(v => v.Name ==  varName && v.Phase == varPhase);
                    if (modelVariable == null) continue;
                    modelVariable.CalcVal = DoubleExtensions.GetDoubleValueFromText(modVarValueLines[2 + counter]);
                    counter++;
                }
            }
        }

        private List<string> ReadFileLines(string filePartialName)
        {
            var file = _files.FirstOrDefault(s => s.Contains(filePartialName));
            var fileInfo = new FileInfo(file);
            if (file == null || fileInfo.Extension != ".txt")
            {
                MessageBox.Show($"Couldn't fine {filePartialName}.txt");
                return null;
            }

            var lines = File.ReadAllLines(file).ToList();
            lines.Remove(l => string.IsNullOrWhiteSpace(l));

            return lines;
        }
        private List<string> ReadAndSplitLine(string line, bool includeDot = true)
        {
            string cleaned = line.Trim().TrimStart('%').Trim();
            var chars = includeDot ? new char[] { ' ', '.' } : new char[] { ' ' };
            var splitted = cleaned.Split(chars).ToList();
            splitted.Remove(l => string.IsNullOrWhiteSpace(l));

            return splitted;
        }
    }
}
