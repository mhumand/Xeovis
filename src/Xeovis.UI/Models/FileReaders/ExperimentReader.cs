using ExcelDataReader;
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
    public class ExperimentReader
    {
        private List<ListableItem> _folders;
        private string _subdirectory;
        private bool _hasSubdirectory => !string.IsNullOrWhiteSpace(_subdirectory);

        public ExperimentReader(List<ListableItem> folders, string subdirectory = null)
        {
            _folders = folders;
            _subdirectory = subdirectory;
        }

        public async Task<List<Experiment>> GetExperiments(IProgress<ProgressBarData> progress, CancellationToken token)
        {
            var experiments = new List<Experiment>();

            var progressData = new ProgressBarData(0, "Loading");
            int index = 1;
            foreach (var folder in _folders)
            {
                token.ThrowIfCancellationRequested();

                if (folder == null || folder.Tag is not DirectoryInfo info) continue;

                progressData.PrimaryValue = index * 1.0 / _folders.Count * 100.0;
                progressData.PrimaryLabel = $"Loading {folder.Label}";
                progress.Report(progressData);

                var experiment = new Experiment(folder.Label);
                experiment.DirectoryInfo = info;
                ReadExperimentFile(experiment);
                ReadPhasesFile(experiment);
                experiments.Add(experiment);

                index++;
            }

            return experiments;
        }

        private void ReadExperimentFile(Experiment experiment)
        {
            if (experiment == null || experiment.DirectoryInfo == null) return;

            List<FileInfo> subFiles = experiment.DirectoryInfo.GetFiles().Where(f => f.Extension.StartsWith(".xls")).ToList();

            if (_hasSubdirectory)
            {
                string subPath = experiment.DirectoryInfo.FullName + $"\\{_subdirectory}";
                if (Directory.Exists(subPath))
                {
                    var filePaths = Directory.GetFiles(subPath, "*.xls").ToList();
                    subFiles.Clear();
                    foreach (var path in filePaths)
                        subFiles.Add(new FileInfo(path));
                }
            }

            if (subFiles == null || subFiles.Count == 0) return;
            subFiles.Remove(subFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.FullName) == experiment.DirectoryInfo.Name));
            experiment.Files = subFiles;
            var expFile = subFiles.FirstOrDefault(f => f.Name.ToLower().Contains("experiment.xls"));
            if (expFile == null) return;

            using (var stream = File.Open(expFile.FullName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var table = result.Tables[0];
                    var headerRow = table.Rows[0].ItemArray.ToList();
                    int indexIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("Index")));
                    int nameIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("Experiment")));
                    int pressureIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("P (GPa)")));
                    int temperatureIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("T (C)")));
                    int durationIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("Duration")));
                    int phasesIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("Phases")));
                    int rocktypeIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains("Rocktype")));

                    if (nameIndex < 0 || pressureIndex < 0 || temperatureIndex < 0 || durationIndex < 0 || pressureIndex < 0) return;

                    experiment.Runs.Clear();
                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        var cells = table.Rows[i].ItemArray;
                        var run = new ExpRun();

                        if (indexIndex >= 0 && !string.IsNullOrWhiteSpace(cells[indexIndex].ToString()))
                            run.ReadingIndex = (int)DoubleExtensions.GetDoubleValueFromText(cells[indexIndex].ToString());

                        run.Name = cells[nameIndex].ToString();
                        if (string.IsNullOrEmpty(run.Name)) continue;

                        run.Pressure = DoubleExtensions.GetDoubleValueFromText(cells[pressureIndex].ToString());
                        if (double.IsNaN(run.Pressure)) continue;
                        run.Pressure *= 10.0; //GPa to KBar

                        run.Temperature = DoubleExtensions.GetDoubleValueFromText(cells[temperatureIndex].ToString());
                        if (double.IsNaN(run.Temperature)) continue;

                        run.Duration = DoubleExtensions.GetDoubleValueFromText(cells[durationIndex].ToString());

                        if (rocktypeIndex >= 0)
                            run.Rocktype = RockTypeExtensions.GetRockType(cells[rocktypeIndex].ToString());

                        var phases = cells[phasesIndex].ToString().Split(new char[] { '+', ',' });
                        foreach (var ph in phases)
                        {
                            var phase = new Phase(ph.Trim());
                            phase.ExpRunId = run.Id;
                            run.Phases.Add(phase);
                        }

                        var oxides = Enum.GetValues(typeof(OxideType));
                        run.BulkOxides.Clear();
                        run.BulkComposition.Clear();
                        var indx = 0;
                        foreach (OxideType oxide in oxides)
                        {
                            if (oxide == OxideType.O) continue;
                            var index = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains(oxide.ToString()) && !c.ToString().Contains("err")));
                            if (index < 0) continue;
                            run.BulkOxides.Add(oxide);
                            double value = DoubleExtensions.GetDoubleValueFromText(cells[index].ToString());
                            run.BulkComposition.Add(double.IsNaN(value) ? 0 : value);
                        }

                        run.Index = i;
                        run.ExperimentId = experiment.Id;
                        experiment.Runs.Add(run);
                    }
                }
            }
        }

        private void ReadPhasesFile(Experiment experiment)
        {
            var phaseFiles = experiment.Files.Except(experiment.Files.Where(f => Path.GetFileNameWithoutExtension(f.FullName).ToLower().Contains("experiment")));
            if (phaseFiles == null || phaseFiles.Count() == 0) return;
            experiment.PhasesWithData.Clear();
            foreach (var file in phaseFiles)
            {
                string phaseName = Path.GetFileNameWithoutExtension(file.FullName);
                var phaseType = PhaseTypeExtensions.GetPhaseTypeByName(phaseName.ToLower());

                if (!experiment.PhasesWithData.Contains(phaseType))
                    experiment.PhasesWithData.Add(phaseType);

                using (var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet();
                        var table = result.Tables[0];
                        var headerRow = table.Rows[0].ItemArray.ToList();
                        int runIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().ToLower().Contains("index")));
                        int commentIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().ToLower().Contains("comment")));

                        if (runIndex < 0) continue;

                        for (int i = 1; i < table.Rows.Count; i++)
                        {
                            var cells = table.Rows[i].ItemArray;
                            var runName = cells[runIndex].ToString();
                            if (string.IsNullOrEmpty(runName)) continue;
                            int.TryParse(runName, out int intName); // some runs are read by their index in experiment.xls file
                            var run = experiment.Runs.FirstOrDefault(r => r.Name == runName || r.ReadingIndex == intName);
                            if (run == null) continue;
                            var runPhase = run.Phases.FirstOrDefault(ph => ph.Type == phaseType);
                            if (runPhase == null) continue;

                            if (commentIndex >= 0)
                                runPhase.Comment = cells[commentIndex].ToString();

                            //todo: oxides index can be save only once
                            var oxides = Enum.GetValues(typeof(OxideType));
                            runPhase.Oxides.Clear();
                            runPhase.Composition.Clear();
                            var indx = 0;
                            foreach (OxideType oxide in oxides)
                            {
                                if (oxide == OxideType.O) continue;
                                var oxideIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains(oxide.ToString()) && !c.ToString().Contains("err")));
                                if (oxideIndex < 0) continue;
                                runPhase.Oxides.Add(oxide);
                                double value = DoubleExtensions.GetDoubleValueFromText(cells[oxideIndex].ToString());
                                runPhase.Composition.Add(double.IsNaN(value) ? 0 : value);
                            }
                        }
                    }
                }
            }
        }
    }
}
