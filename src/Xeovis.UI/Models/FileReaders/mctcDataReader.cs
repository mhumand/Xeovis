using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xeovis.Base;
using Xeovis.Domain;

namespace Xeovis.UI.Models.FileReaders
{
    public class mctcDataReader
    {
        private readonly string _directory;
        private List<string> _files;
        private List<Batch> _batches;
        private List<xeosVariable> _xeosVariables;
        private List<mctcVariable> _mctcVariables;

        public mctcDataReader(string directory)
        {
            _directory = directory;
            _files = new List<string>();
            _batches = new List<Batch>();
            _xeosVariables = new List<xeosVariable>();
            _mctcVariables = new List<mctcVariable>();
        }

        public async Task<mctcBundle> ReadData(IProgress<ProgressBarData> progress, CancellationToken token)
        {
            if (!Directory.Exists(_directory)) return null;

            _files = Directory.GetFiles(_directory, "*.txt").ToList();
            if (_files.Count == 0) return null;

            var progressData = new ProgressBarData(0, "Loading");
            progressData.PrimaryLabel = $"Loading Batch Lines";
            progress.Report(progressData);
            ReadBatchLines();

            progressData.PrimaryValue = 25;
            progressData.PrimaryLabel = $"Loading Batch Bulks";
            progress.Report(progressData);
            ReadBatchBulks();

            progressData.PrimaryValue = 50;
            progressData.PrimaryLabel = $"Loading xeos Variables";
            progress.Report(progressData);
            ReadxeosVariables();

            progressData.PrimaryValue = 75;
            progressData.PrimaryLabel = $"Loading mctc Results";
            progress.Report(progressData);
            ReadmctcResults();

            progressData.PrimaryValue = 100;
            progress.Report(progressData);

            var mctcBundle = new mctcBundle(_batches, _mctcVariables, _xeosVariables);
            _xeosVariables.ForEach(x => x.mctcBundleId = mctcBundle.Id);
            _mctcVariables.ForEach(x => x.mctcBundleId = mctcBundle.Id);
            return mctcBundle;
        }

        private void ReadBatchLines()
        {
            var lines = ReadFileLines("batchlines");
            if (lines == null || lines.Count == 0) return;

            _batches = new List<Batch>();
            for (int i = 0; i < lines.Count; i++)
            {
                // batch name
                var Line1 = CleanAndSplitLine(lines[i]);

                var batch = new Batch();
                int.TryParse(Line1[0], out int index);
                batch.Index = index;
                batch.ExperimentName = Line1[1];
                batch.ExpRunName = Line1[2];

                //batch data
                i++;
                var Line2 = CleanAndSplitLine(lines[i], includeDot: false);

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

                batch.ExpRunPressure = DoubleExtensions.GetDoubleValueFromText(Line2[varianceIndex + 3]);
                batch.ExpRunTemperature = DoubleExtensions.GetDoubleValueFromText(Line2[varianceIndex + 4]);

                _batches.Add(batch);
            }
        }

        private void ReadBatchBulks()
        {
            var lines = ReadFileLines("batchbulks");
            if (lines == null || lines.Count == 0) return;

            var oxidesStr = CleanAndSplitLine(lines[0]);
            var oxides = new List<OxideType>();
            foreach (var oxideStr in oxidesStr)
                oxides.Add(EnumExtensions.GetValueFromDescription<OxideType>(oxideStr));

            for (int i = 1; i < lines.Count; i++)
            {
                var compStr = CleanAndSplitLine(lines[i], includeDot: false);
                var composition = new List<double>();
                for (int j = 1; j <= oxides.Count; j++)
                    composition.Add(DoubleExtensions.GetDoubleValueFromText(compStr[j]));

                _batches[i - 1].BulkOxides = oxides;
                _batches[i - 1].BulkComposition = composition;
            }
        }

        private void ReadxeosVariables()
        {
            var lines = ReadFileLines("observations");
            if (lines == null || lines.Count == 0) return;

            lines = CleanAndSplitLine(lines[0], includeDot: false);
            _xeosVariables = new List<xeosVariable>();
            for (int i = 0; i < lines.Count; i++)
            {
                var varData = lines[i].Split(new char[] { '(', ')', '-' }).ToList();
                varData.RemoveAll(l => string.IsNullOrWhiteSpace(l));

                var xeosVariable = new xeosVariable();
                xeosVariable.PhaseType = PhaseTypeExtensions.GetPhaseTypeByName(varData[1]);
                xeosVariable.VariableType = EnumExtensions.GetValueFromDescription<ModelVariableType>(varData[0]);

                int.TryParse(varData[2].TrimStart(new char[] { 'b' }), out int batchIndex);
                var batch = _batches.FirstOrDefault(b => b.Index == batchIndex);
                xeosVariable.BatchId = batch.Id;

                i++;
                xeosVariable.ObsVal = DoubleExtensions.GetDoubleValueFromText(lines[i]);

                batch.xeosVariables.Add(xeosVariable);
                _xeosVariables.Add(xeosVariable);
            }
        }

        private void ReadmctcResults()
        {
            var allLines = ReadFileLines("-dr");
            if (allLines == null || allLines.Count == 0) return;

            var splitters = new char[] { '{', '}', '*', '(', ')', '\t', '"', ',', ';' };
            var allRange = Enumerable.Range(0, allLines.Count);

            _mctcVariables = GetMctcVariables(allLines);
            if (_mctcVariables == null || _mctcVariables.Count == 0) return;

            var resultsStartIndex = allRange.Where(i => allLines[i].Contains("cw")).ToList();
            var mctcVarValueEndIndex = allRange.Where(i => allLines[i].Contains("nw")).ToList();
            if (resultsStartIndex.Count == 0 || mctcVarValueEndIndex.Count == 0) return;
            for (int i = 0; i < resultsStartIndex.Count; i++)
            {
                var valsLine = string.Join("", allLines.GetRange(resultsStartIndex[i], mctcVarValueEndIndex[i] - resultsStartIndex[i]).ToArray());
                var valsLines = valsLine.Split(splitters).ToList();
                valsLines.RemoveAll(l => string.IsNullOrWhiteSpace(l));

                for (int j = 1; j < valsLines.Count; j++)
                    _mctcVariables.FirstOrDefault(v => v.Index == j - 1).Values.Add(DoubleExtensions.GetDoubleValueFromText(valsLines[j].Trim()));
            }

            for (int i = 0; i < _batches.Count; i++)
            {
                var batch = _batches[i];

                string batchKey = $"(* {batch.Index} *)";
                string nextBatchKey = $"(* {batch.Index + 1} *)";
                var batchLineIndexes = allRange.Where(j => allLines[j].Contains(batchKey)).ToList();
                var nextBatchLineIndexes = allRange.Where(j => allLines[j].Contains(nextBatchKey)).ToList();

                // what data is in lines[batchLinesIndx[0]]? phase proportions?
                string modVarNameLine = string.Empty;
                int cutoff = nextBatchLineIndexes.Count == 0 ? resultsStartIndex[0] - 2 : nextBatchLineIndexes[0] - 1;
                for (int j = batchLineIndexes[0]; j < cutoff; j++)
                    modVarNameLine += allLines[j + 1];

                var modVarNameLines = modVarNameLine.Split(splitters).ToList();
                modVarNameLines.RemoveAll(l => string.IsNullOrWhiteSpace(l));

                for (int k = 0; k < resultsStartIndex.Count; k++)
                {
                    string modVarValueLine = string.Empty;

                    if (nextBatchLineIndexes.Count == 0)
                        cutoff = resultsStartIndex.Count == k + 1 ? allLines.Count - 1 : resultsStartIndex[k + 1];
                    else
                        cutoff = nextBatchLineIndexes[1 + k];

                    for (int j = batchLineIndexes[1 + k]; j < cutoff; j++)
                        modVarValueLine += allLines[j];

                    var modVarValueLines = modVarValueLine.Split(splitters).ToList();
                    modVarValueLines.RemoveAll(l => string.IsNullOrWhiteSpace(l));

                    // in the -dr file, last line of each result section has an extra element of "0"
                    if (nextBatchLineIndexes.Count == 0)
                        modVarValueLines.RemoveAt(modVarValueLines.Count - 1);

                    if (modVarValueLines.Count <= 3)
                    {
                        batch.IsFailed.Add(k, true);
                        continue;
                    }
                    else
                        batch.IsFailed.Add(k, false);

                    var modelVarsInResults = new List<xeosVariable>();
                    for (int j = 0; j < modVarNameLines.Count; j++)
                    {
                        var varName = modVarNameLines[j];
                        j++;
                        var varPhase = PhaseTypeExtensions.GetPhaseTypeByName(modVarNameLines[j]);
                        var modelVariable = batch.xeosVariables.FirstOrDefault(v => v.VariableType.ToString() == varName && v.PhaseType == varPhase);
                        if (modelVariable == null) continue;
                        modelVariable.CalcVals.Add(k, DoubleExtensions.GetDoubleValueFromText(modVarValueLines[2 + (j + 1) / 2]));
                        modelVarsInResults.Add(modelVariable);
                    }

                    var otherModelVars = batch.xeosVariables.Except(modelVarsInResults).ToList();
                    foreach (var modelVar in otherModelVars)
                    {
                        _xeosVariables.Remove(modelVar);
                        batch.xeosVariables.Remove(modelVar);
                    }
                }
            }
        }

        private List<string> ReadFileLines(string filePartialName)
        {
            var file = _files.FirstOrDefault(s => s.Contains(filePartialName));
            if (file == null) return null;

            var lines = new List<string>();
            using (FileStream fs = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fs))
                {
                    while (!sr.EndOfStream)
                        lines.Add(sr.ReadLine());
                }
            }

            lines.RemoveAll(l => string.IsNullOrWhiteSpace(l));
            return lines;
        }

        private List<string> CleanAndSplitLine(string line, bool includeDot = true)
        {
            string cleaned = line.Trim().TrimStart('%').Trim();
            var chars = includeDot ? new char[] { ' ', '.' } : new char[] { ' ' };
            var splitted = cleaned.Split(chars).ToList();
            splitted.RemoveAll(l => string.IsNullOrWhiteSpace(l));

            return splitted;
        }

        private List<mctcVariable> GetMctcVariables(List<string> lines)
        {
            var allRange = Enumerable.Range(0, lines.Count);
            var splitters = new char[] { '{', '}', '*', '(', ')', '\t', '"', ',', ';' };

            var mcinfoIndex = allRange.Where(i => lines[i].Contains("mcinfo")).ToList()[0];
            var mcrangeIndex = allRange.Where(i => lines[i].Contains("mcrange")).ToList()[0];
            var mctcVarLineCount = mcrangeIndex - mcinfoIndex;
            var mctcVariables = new List<mctcVariable>();
            for (int i = 0; i < mctcVarLineCount; i++)
            {
                var line = lines[mcinfoIndex + i];
                var parts = line.Split(new char[] { '{', '}', '\\', '"' }).ToList();
                parts.RemoveAll(l => string.IsNullOrWhiteSpace(l) || l == "," || l == ";");
                if (parts.Count == 3) parts.RemoveAt(0);
                var name = parts[0];
                var valuesString = parts[1].Split(splitters).ToList();
                valuesString.RemoveAll(l => string.IsNullOrWhiteSpace(l));
                var startingVal = DoubleExtensions.GetDoubleValueFromText(valuesString[0].Trim());
                var rangeVal = DoubleExtensions.GetDoubleValueFromText(valuesString[1].Trim());
                var mctcVariable = new mctcVariable() { Index = i, Name = name, StartingValue = startingVal, Range = rangeVal };
                mctcVariables.Add(mctcVariable);
            }

            return mctcVariables;
        }

    }
}
