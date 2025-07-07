using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xeovis.Base;
using Xeovis.Domain;
using Xeovis.Domain.Definitions;

namespace Xeovis.UI.Models.FileReaders
{
    public class ExperimentReader
    {
        private List<ListableItem> _folders;
        private string _subdirectory;
        private bool _hasSubdirectory => !string.IsNullOrWhiteSpace(_subdirectory);
        private bool _useSubdirectory;

        public ExperimentReader(List<ListableItem> folders, string subdirectory = null, bool useSubdirectory = false)
        {
            _folders = folders;
            _subdirectory = subdirectory;
            _useSubdirectory = useSubdirectory;
        }

        public async Task<ExperimentReaderResults> GetExperiments(IProgress<ProgressBarData> progress, CancellationToken token)
        {
            var experiments = new List<Experiment>();

            var progressData = new ProgressBarData(0, "Loading");
            int index = 1;
            var unknownPhaseNames = new Dictionary<Guid, List<string>>();
            var unknownPhaseFiles = new Dictionary<Guid, List<string>>();
            foreach (var folder in _folders)
            {
                token.ThrowIfCancellationRequested();

                if (folder == null || folder.Tag is not DirectoryInfo info) continue;

                progressData.PrimaryValue = index * 1.0 / _folders.Count * 100.0;
                progressData.PrimaryLabel = $"Loading {folder.Label}";
                progress.Report(progressData);

                var experiment = new Experiment(folder.Label, info.FullName);
                ReadExperimentFile(experiment, out List<string> unknownPhaseNamesInExperiment);
                ReadPhasesFile(experiment, out List<string> unknownPhaseFilesInExperiment);
                experiments.Add(experiment);

                index++;

                if (unknownPhaseNamesInExperiment != null && unknownPhaseNamesInExperiment.Count > 0)
                    unknownPhaseNames.Add(experiment.Id, unknownPhaseNamesInExperiment);

                if (unknownPhaseFilesInExperiment != null && unknownPhaseFilesInExperiment.Count > 0)
                    unknownPhaseFiles.Add(experiment.Id, unknownPhaseFilesInExperiment);
            }

            return new ExperimentReaderResults(experiments, unknownPhaseNames, unknownPhaseFiles);
        }

        private void ReadExperimentFile(Experiment experiment, out List<string> unknownPhaseNames)
        {
            if (experiment == null || string.IsNullOrWhiteSpace(experiment.Path))
            {
                unknownPhaseNames = null;
                return;
            }

            var dirInfo = new DirectoryInfo(experiment.Path);
            List<FileInfo> subFiles = dirInfo.GetFiles().Where(f => f.Extension.StartsWith(".xls")).ToList();

            if (_useSubdirectory && _hasSubdirectory)
            {
                string subPath = dirInfo.FullName + $"\\{_subdirectory}";
                if (Directory.Exists(subPath))
                {
                    var filePaths = Directory.GetFiles(subPath, "*.xls").ToList();
                    subFiles.Clear();
                    foreach (var path in filePaths)
                        subFiles.Add(new FileInfo(path));
                }
            }

            if (subFiles == null || subFiles.Count == 0)
            {
                unknownPhaseNames = null;
                return;
            }

            subFiles.Remove(subFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.FullName) == dirInfo.Name));
            experiment.FilesPath = subFiles.Select(x => x.FullName).ToList();
            var expFile = subFiles.FirstOrDefault(f => f.Name.ToLower().Contains("experiment.xls"));
            if (expFile == null)
            {
                unknownPhaseNames = null;
                return;
            }

            unknownPhaseNames = new List<string>();
            using (var stream = File.Open(expFile.FullName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var table = result.Tables[0];
                    var headerRow = table.Rows[0].ItemArray.ToList();
                    int indexIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("Index")));
                    int nameIndex =
                        headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("Experiment")));
                    int pressureIndex =
                        headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("P (GPa)")));
                    int temperatureIndex =
                        headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("T (C)")));
                    int durationIndex =
                        headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("Duration")));
                    int phasesIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("Phases")));
                    int rocktypeIndex =
                        headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("Rocktype")));

                    if (nameIndex < 0 || pressureIndex < 0 || temperatureIndex < 0 || durationIndex < 0 ||
                        pressureIndex < 0) return;

                    experiment.Runs.Clear();
                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        var cells = table.Rows[i].ItemArray;
                        var run = new ExpRun();

                        if (indexIndex >= 0 && !string.IsNullOrWhiteSpace(cells[indexIndex].ToString()))
                            run.ReadingIndex =
                                (int)DoubleExtensions.GetDoubleValueFromText(cells[indexIndex].ToString());

                        run.Name = cells[nameIndex].ToString();
                        if (string.IsNullOrEmpty(run.Name)) continue;

                        run.Pressure = DoubleExtensions.GetDoubleValueFromText(cells[pressureIndex].ToString());
                        if (double.IsNaN(run.Pressure)) continue;
                        run.Pressure = (run.Pressure * 10.0).Round(12); //GPa to KBar

                        run.Temperature = DoubleExtensions.GetDoubleValueFromText(cells[temperatureIndex].ToString());
                        if (double.IsNaN(run.Temperature)) continue;

                        run.Duration = DoubleExtensions.GetDoubleValueFromText(cells[durationIndex].ToString());

                        if (rocktypeIndex >= 0)
                            run.Rocktype = RockTypeExtensions.GetRockType(cells[rocktypeIndex].ToString());

                        var phases = cells[phasesIndex].ToString().Split(new char[] { '+', ',' });
                        foreach (var ph in phases)
                        {
                            var nameSet = PhaseNameList.GetNameSetByString(ph.Trim());
                            if (nameSet == null)
                            {
                                unknownPhaseNames.Add(ph.Trim());
                                continue;
                            }

                            var phase = new Phase(nameSet.Id);
                            phase.ExpRunId = run.Id;
                            run.Phases.Add(phase);
                        }

                        var oxides = Enum.GetValues(typeof(OxideType));
                        run.BulkComposition.Clear();
                        var indx = 0;
                        foreach (OxideType oxide in oxides)
                        {
                            if (oxide == OxideType.O) continue;
                            var index = headerRow.IndexOf(headerRow.FirstOrDefault(c =>
                                c.ToString().Contains(oxide.ToString()) && !c.ToString().Contains("err")));
                            if (index < 0) continue;
                            var value = DoubleExtensions.GetDoubleValueFromText(cells[index].ToString());
                            if (!double.IsNaN(value) && value > 0)
                                run.BulkComposition.Add(oxide, value);
                        }

                        run.Index = i;
                        run.ExperimentId = experiment.Id;
                        experiment.Runs.Add(run);
                    }
                }
            }
        }

        private bool FindIndexOfHeaderByName(List<DataRow> rows, string headerName, out int index)
        {
            var headerRow = rows[0].ItemArray.ToList();
            int runIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c =>
                c.ToString().Contains(headerName, StringComparison.CurrentCultureIgnoreCase)));
            if (runIndex < 0)
            {
                index = runIndex;
                return false;
            }

            bool isEmptyColumn = rows.Select(r => r.ItemArray.ToList()[runIndex]).Skip(1)
                .All(x => string.IsNullOrWhiteSpace(x.ToString()));
            index = runIndex;

            return runIndex >= 0 && !isEmptyColumn;
        }

        private int FindRunHeaderIndex(DataTable table, Experiment experiment)
        {
            var rowList = table.Rows.OfType<DataRow>().ToList();
            var headerRow = rowList[0].ItemArray.ToList();

            int runIndex = -1;
            var test1 = FindIndexOfHeaderByName(rowList, "index", out runIndex);

            bool test2 = true;
            if (!test1)
                test2 = FindIndexOfHeaderByName(rowList, "experiment", out runIndex);

            if (!test2)
            {
                var indexes = new List<int>();
                foreach (ExpRun run in experiment.Runs)
                {
                    var innerIndex = rowList
                        .Select(r => r.ItemArray.ToList().FindIndex(x =>
                            x.ToString().Equals(run.Name, StringComparison.CurrentCultureIgnoreCase)))
                        .FirstOrDefault(x => x > 0, -1);
                    if (innerIndex >= 0)
                        indexes.Add(innerIndex);
                }

                // choosing the most occured index
                runIndex = indexes.Count > 0
                    ? (int)indexes.GroupBy(n => n).OrderByDescending(g => g.Count()).First().Key
                    : -1;
            }

            return runIndex;
        }

        private void ReadPhasesFile(Experiment experiment, out List<string> unknownPhaseFiles)
        {
            var phaseFiles = experiment.FilesPath.Except(experiment.FilesPath.Where(f =>
                Path.GetFileNameWithoutExtension(f).ToLower().Contains("experiment")));
            if (phaseFiles == null || phaseFiles.Count() == 0)
            {
                unknownPhaseFiles = null;
                return;
            }

            experiment.PhasesWithData.Clear();
            unknownPhaseFiles = new List<string>();
            foreach (var file in phaseFiles)
            {
                string phaseName = Path.GetFileNameWithoutExtension(file);
                var phaseNameSet = PhaseNameList.GetNameSetByString(phaseName);
                if (phaseNameSet == null)
                {
                    unknownPhaseFiles.Add(phaseName);
                    continue;
                }

                if (!experiment.PhaseIdsWithData.Contains(phaseNameSet.Id))
                    experiment.PhaseIdsWithData.Add(phaseNameSet.Id);

                using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet();
                        var table = result.Tables[0];
                        var headerRow = table.Rows[0].ItemArray.ToList();

                        int runIndex = FindRunHeaderIndex(table, experiment);
                        int commentIndex =
                            headerRow.IndexOf(headerRow.FirstOrDefault(c =>
                                c.ToString().ToLower().Contains("comment")));

                        if (runIndex < 0) continue;

                        for (int i = 1; i < table.Rows.Count; i++)
                        {
                            var cells = table.Rows[i].ItemArray;
                            var runName = cells[runIndex].ToString();
                            if (string.IsNullOrEmpty(runName)) continue;
                            int.TryParse(runName,
                                out int intName); // some runs are read by their index in experiment.xls file
                            var run = experiment.Runs.FirstOrDefault(r => r.Name == runName || r.ReadingIndex == intName);
                            if (run == null) continue;
                            var runPhase = run.Phases.FirstOrDefault(ph => ph.NameSet == phaseNameSet);
                            if (runPhase == null) continue;

                            if (commentIndex >= 0)
                                runPhase.Comment = cells[commentIndex].ToString();

                            //todo: oxides index can be save only once
                            var oxides = Enum.GetValues(typeof(OxideType));
                            runPhase.Composition.Clear();
                            var indx = 0;
                            foreach (OxideType oxide in oxides)
                            {
                                if (oxide == OxideType.O) continue;
                                var oxideIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c =>
                                    c.ToString().Contains(oxide.ToString()) && !c.ToString().Contains("err")));
                                if (oxideIndex < 0) continue;
                                var value = DoubleExtensions.GetDoubleValueFromText(cells[oxideIndex].ToString());
                                if (!double.IsNaN(value) && value > 0)
                                    runPhase.Composition.Add(oxide, value);
                            }

                            if (runPhase.CompositionValues.All(x => x == 0))
                                run.Phases.Remove(runPhase);
                        }
                    }
                }
            }
        }

        public void RereadExperiment(Experiment experiment, out List<string> unknownPhaseNames, out List<string> unknownPhaseFiles)
        {
            experiment.ClearData();
            ReadExperimentFile(experiment, out unknownPhaseNames);
            ReadPhasesFile(experiment, out unknownPhaseFiles);
        }
    }
}