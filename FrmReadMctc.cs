using DatabaseReader.Definitions;
using DatabaseReader.Definitions.Persistence;
using DatabaseReader.Definitions.UI;
using DatabaseReader.Enums;
using DatabaseReader.Events;
using DatabaseReader.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
        private ContextMenu _contextMenu;
        private DataFile _dataFile;

        public FrmReadMctc()
        {
            InitializeComponent();
            PrepareUI();
        }

        private void PrepareUI()
        {
            MakeContextMenu();
            tvCaseSelection.ContextMenu = _contextMenu;
            tcPlotInfo.TableRowSelected += tcPlotInfo_RowSelected;
            ccMctcPlot.ChartPointSelected += ccMctcPlot_ChartPointSelected;
        }

        private void ccMctcPlot_ChartPointSelected(object sender, ChartPointSelectedEventArgs e)
        {
            var table = tcPlotInfo.DataTable;
            Guid id = Guid.NewGuid();

            if (!double.IsNaN(e.X) && !double.IsNaN(e.Y))
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if ((double)table.Rows[i][1] == e.X && (double)table.Rows[i][2] == e.Y)//todo - generalize
                    {
                        id = (Guid)table.Rows[i][tcPlotInfo.IDColumn];
                        break;
                    }
                }
            }

            tcPlotInfo.SelectRow(id);
        }

        private void tcPlotInfo_RowSelected(object sender, TableRowSelectedEventArgs e)
        {
            if (e == null)
            {
                ccMctcPlot.CrosshairSelectPoint();
                return;
            }

            double x = (double)e.Row[1]; //todo - generalize
            double y = (double)e.Row[2];
            ccMctcPlot.CrosshairSelectPoint(x, y);
        }

        private void MakeContextMenu()
        {
            _contextMenu = new ContextMenu();
            MenuItem checkAll = new MenuItem("Check All", CheckAll_Click);
            MenuItem checkNone = new MenuItem("Check None", CheckNone_Click);
            _contextMenu.MenuItems.Add(checkAll);
            _contextMenu.MenuItems.Add(checkNone);
        }

        private void CheckNone_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in tvCaseSelection.Nodes)
                node.Checked = false;

            tvCaseSelection.SelectedNode = null;
        }

        private void CheckAll_Click(object sender, EventArgs e)
        {
            foreach (TreeNode node in tvCaseSelection.Nodes)
                node.Checked = true;

            tvCaseSelection.SelectedNode = null;
        }

        private void btnReadMctc_Click(object sender, EventArgs e)
        {
            tcPlotInfo.ClearTable();
            ccMctcPlot.ClearPlot();
            _batches = null;
            _modelVariables = null;
            _dataFile = null;

            directory = "H:\\My Drive\\PhD @ Unimelb\\PhD\\PhD - Project\\Project - Calculations\\set_up_fitting\\TC RUN";
            if (!Directory.Exists(directory))
            {
                MessageBox.Show("Directory does not exist.");
                return;
            }
            else
            {
                //RunThermocalc();
                _files = Directory.GetFiles(directory).ToList();
            }

            Cursor = Cursors.WaitCursor;
            ReadBatchLines();
            ReadBatchBulks();

            if (_batches != null)
                ReadModelVariables();

            if (_modelVariables != null)
                ReadMctcResults();

            Cursor = Cursors.Default;

            _batches?.RemoveAll(b => b.IsFailed);
            _modelVariables?.RemoveAll(mv => mv.Batch.IsFailed);

            _dataFile = new DataFile();
            _dataFile.Batches = _batches;
            _dataFile.ModelVariables = _modelVariables;

            PopulateUI();
        }

        private void RunThermocalc()
        {
            // Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = true;
            startInfo.FileName = directory + "\\tc355aWin.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //startInfo.Verb = "runas";

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    //string output = exeProcess.StandardOutput.ReadToEnd();
                }
            }
            catch
            {
                // Log error.
            }
        }

        private void PopulateUI()
        {
            if (ckbxIncludeBatches.Checked)
                PopulateTreeView_IncludeBatches();
            else
                PopulateTreeView_PhaseModelsOnly();
        }

        private void PopulateTreeView_IncludeBatches()
        {
            tvCaseSelection.Nodes.Clear();
            if (_batches == null || _batches.Count == 0) return;
            foreach (var batch in _batches)
            {
                var batchNode = new TreeNode(batch.Name);
                batchNode.Tag = batch;
                foreach (var phase in batch.Phases)
                {
                    var phaseNode = new TreeNode(phase.ToString() + " - " + EnumExtensions.GetDescriptionFromValue(phase));
                    phaseNode.Tag = phase;
                    var phaseModelVars = batch.ModelVariables.Where(mv => mv.Phase == phase).ToList();
                    foreach (var modelVar in phaseModelVars)
                    {
                        var modelVarNode = new TreeNode(modelVar.ToString());
                        modelVarNode.Tag = modelVar;
                        phaseNode.Nodes.Add(modelVarNode);
                    }

                    if (phaseNode.Nodes.Count > 0)
                        batchNode.Nodes.Add(phaseNode);
                }

                tvCaseSelection.Nodes.Add(batchNode);
            }

            tvCaseSelection.ExpandAll();
        }

        private void PopulateTreeView_PhaseModelsOnly()
        {
            tvCaseSelection.Nodes.Clear();
            if (_modelVariables == null || _modelVariables.Count == 0 || _batches.Count == 0) return;
            var allPhases = _modelVariables.Select(v => v.Phase).ToList();
            var phases = allPhases.Distinct();

            foreach (var phase in phases)
            {
                var phaseNode = new TreeNode(phase.ToString() + " - " + EnumExtensions.GetDescriptionFromValue(phase));
                phaseNode.Tag = phase;
                var batches = _batches.Where(b => b.ModelVariables.Any(mv => mv.Phase == phase)).ToList();
                var allModelVars = batches.SelectMany(b => b.ModelVariables.Where(v => v.Phase == phase)).ToList();
                var groupedVars = allModelVars.GroupBy(v => v.ToString()).ToList();
                foreach (var group in groupedVars)
                {
                    var modelVar = group.FirstOrDefault();
                    var modelVarNode = new TreeNode(modelVar.ToString());
                    modelVarNode.Tag = modelVar;
                    phaseNode.Nodes.Add(modelVarNode);
                }

                tvCaseSelection.Nodes.Add(phaseNode);
            }

            tvCaseSelection.ExpandAll();
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
                batch.Experiment = new Experiment(Line1[1]);
                batch.ExperimentalRun = new ExperimentalRun() { Name = Line1[2], Experiment = batch.Experiment };

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

                batch.ExperimentalRun.Pressure = DoubleExtensions.GetDoubleValueFromText(Line2[varianceIndex + 3]);
                batch.ExperimentalRun.Temperature = DoubleExtensions.GetDoubleValueFromText(Line2[varianceIndex + 4]);

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

        private void ReadModelVariables()
        {
            var lines = ReadFileLines("observations");
            if (lines == null || lines.Count == 0) return;

            lines = CleanAndSplitLine(lines[0], includeDot: false);
            _modelVariables = new List<ModelVariable>();
            for (int i = 0; i < lines.Count; i++)
            {
                var varData = lines[i].Split(new char[] { '(', ')', 'b', '-' }).ToList();
                varData.RemoveAll(l => string.IsNullOrWhiteSpace(l));

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

                string batchKey = $"(* {batch.Index} *)";
                string nextBatchKey = $"(* {batch.Index + 1} *)";
                var batchLinesIndx = Enumerable.Range(0, lines.Count).Where(j => lines[j].Contains(batchKey)).ToList();
                var nextBatchLinesIndx = Enumerable.Range(0, lines.Count).Where(j => lines[j].Contains(nextBatchKey)).ToList();
                if (batchLinesIndx.Count != 2) continue;
                if (nextBatchLinesIndx.Count == 0)
                    nextBatchLinesIndx = new List<int>() { batchLinesIndx[0] + 2, lines.Count };

                var splitters = new char[] { '{', '}', '*', '(', ')', '\t', '"', ',', ';' };

                // what data is in lines[batchLinesIndx[0]]? phase proportions?
                string modVarNameLine = string.Empty;
                for (int j = batchLinesIndx[0]; j < nextBatchLinesIndx[0] - 1; j++)
                    modVarNameLine += lines[j + 1];

                var modVarNameLines = modVarNameLine.Split(splitters).ToList();
                modVarNameLines.RemoveAll(l => string.IsNullOrWhiteSpace(l));

                string modVarValueLine = string.Empty;
                for (int j = batchLinesIndx[1]; j < nextBatchLinesIndx[1]; j++)
                    modVarValueLine += lines[j];

                var modVarValueLines = modVarValueLine.Split(splitters).ToList();
                modVarValueLines.RemoveAll(l => string.IsNullOrWhiteSpace(l));

                if (modVarValueLines.Count <= 3)
                {
                    batch.IsFailed = true;
                    foreach (var modelVar in batch.ModelVariables)
                        _modelVariables.Remove(modelVar);

                    continue;
                }

                var modelVarsInResults = new List<ModelVariable>();
                for (int j = 0; j < modVarNameLines.Count; j++)
                {
                    var varName = modVarNameLines[j];
                    j++;
                    var varPhase = PhaseTypeExtensions.GetPhaseTypeByName(modVarNameLines[j]);
                    var modelVariable = batch.ModelVariables.FirstOrDefault(v => v.Name == varName && v.Phase == varPhase);
                    if (modelVariable == null) continue;
                    modelVariable.CalcVal = DoubleExtensions.GetDoubleValueFromText(modVarValueLines[2 + (j + 1) / 2]);
                    modelVarsInResults.Add(modelVariable);
                }

                var otherModelVars = batch.ModelVariables.Except(modelVarsInResults).ToList();
                foreach (var modelVar in otherModelVars)
                {
                    _modelVariables.Remove(modelVar);
                    batch.ModelVariables.Remove(modelVar);
                }
            }
        }

        private List<string> ReadFileLines(string filePartialName)
        {
            var file = _files.FirstOrDefault(s => s.Contains(filePartialName));
            if (file == null || new FileInfo(file).Extension != ".txt")
            {
                MessageBox.Show($"Couldn't fine {filePartialName}.txt");
                return null;
            }

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

        private void tvCaseSelection_MouseClick(object sender, MouseEventArgs e)
        {
            TreeViewHitTestInfo info = tvCaseSelection.HitTest(tvCaseSelection.PointToClient(Cursor.Position));
            if (info == null) return;
        }

        private void tvCaseSelection_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Nodes.Count > 0)
            {
                var allChildNodes = e.Node.GetAllNodes();
                allChildNodes.ForEach(cn => cn.Checked = e.Node.Checked);
            }
        }

        private void btnPlotAndTabulate_Click(object sender, EventArgs e)
        {
            ccMctcPlot.ClearPlot();
            tcPlotInfo.ClearTable();

            if (_modelVariables == null) return;
            var nodes = tvCaseSelection.GetAllNodes();
            var checkedNodes = nodes.Where(n => n.Checked).ToList();
            if (!checkedNodes.Any()) return;

            var modelVarNodes = checkedNodes.Where(n => n.Tag is ModelVariable).ToList();
            if (!modelVarNodes.Any()) return;

            int currentSelectedIndex = cmbxModelVariable.SelectedIndex;
            cmbxModelVariable.Items.Clear();
            foreach (var item in modelVarNodes)
                cmbxModelVariable.Items.Add(item.Tag);

            if (currentSelectedIndex < cmbxModelVariable.Items.Count && currentSelectedIndex >= 0)
                cmbxModelVariable.SelectedIndex = currentSelectedIndex;
            else
                cmbxModelVariable.SelectedIndex = 0;
        }

        private void cmbxModelVariable_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlotAndTabulate();
        }

        private void PlotAndTabulate()
        {
            var selectedModelVar = cmbxModelVariable.SelectedItem as ModelVariable;
            if (selectedModelVar == null) return;

            // getting all instances of the selected model variable
            var modelVars = _modelVariables.Where(mv => mv.Name == selectedModelVar.Name && mv.Phase == selectedModelVar.Phase).ToList();
            if (modelVars.Count == 0) return;

            #region Plot
            // categorizing based on batch phase assemblage
            var modelVarGroups = modelVars.GroupBy(mv => mv.Batch.GetPhaseAssemblageString()).ToList();
            var chartModel = new ChartModel();
            chartModel.Title = "title test";
            chartModel.XAxisLabel = $"Observed {selectedModelVar}";
            chartModel.YAxisLabel = $"Calculated {selectedModelVar}";
            chartModel.HasEqualDimensions = true;

            int colorIndex = 0;
            int markerIndex = 0;
            foreach (var modelVarGroup in modelVarGroups)
            {
                var curve = new CurveModel();
                curve.Name = modelVarGroup.FirstOrDefault().Batch.GetPhaseAssemblageString();

                curve.Xvals = modelVarGroup.Select(mv => mv.ObsVal).ToList();
                curve.Yvals = modelVarGroup.Select(mv => mv.CalcVal).ToList();
                curve.IsLineVisible = false;

                var color = ChartUtilities.GetColor(colorIndex++);
                if (color == Color.Transparent)
                {
                    colorIndex = (colorIndex - 1) % 10;
                    color = ChartUtilities.GetColor(colorIndex++);
                    markerIndex++;
                }

                curve.Transparency = (int)nudTransparency.Value;
                curve.MarkerColor = color;
                chartModel.Curves.Add(curve);
                curve.MarkerType = ChartUtilities.GetMarkerType(markerIndex);
                curve.MarkerSize = 8;
            }

            // x = y line
            var xyCurve = new CurveModel();
            double minVal = Math.Min(chartModel.Curves.Min(c => c.Xvals.Min()), chartModel.Curves.Min(c => c.Yvals.Min()));
            double maxVal = Math.Max(chartModel.Curves.Max(c => c.Xvals.Max()), chartModel.Curves.Max(c => c.Yvals.Max()));
            double difference = maxVal - minVal;
            minVal -= difference * 0.05;
            maxVal += difference * 0.05;
            xyCurve.Xvals = new List<double>() { minVal, maxVal };
            xyCurve.Yvals = new List<double>() { minVal, maxVal };
            xyCurve.IsMarkerVisible = false;
            xyCurve.LineWidth = 1;
            xyCurve.CurveType = Enums.UI.CurveType.FixedLine;
            chartModel.Curves.Add(xyCurve);
            ccMctcPlot.Plot(chartModel);

            // scaling
            var axisX = ccMctcPlot.Chart.GraphPane.XAxis;
            axisX.Scale.Min = minVal;
            axisX.Scale.Max = maxVal;
            var axisY = ccMctcPlot.Chart.GraphPane.YAxis;
            axisY.Scale.Min = minVal;
            axisY.Scale.Max = maxVal;
            axisY.MajorGrid.IsZeroLine = false;
            ccMctcPlot.Chart.Refresh();

            // settings
            ccMctcPlot.UseLabelFiltering = true;

            var filterPureOnly = "Only Pure Phases";
            var filterSolutionOnly = "Only Solution Phases";
            var purePhasesOnlyString = new List<string>();
            var solutionPhasesString = new List<string>();
            var currentPhase = modelVars.FirstOrDefault().Phase;
            foreach (PhaseType phase in modelVars.SelectMany(v => v.Batch.Phases).Distinct().Except(new List<PhaseType> { currentPhase }))
            {
                var phaseState = PhaseTypeExtensions.GetPhaseStateByPhaseType(phase);
                if (phaseState == PhaseState.Pure)
                    purePhasesOnlyString.Add(phase.ToString());
                else if (phaseState == PhaseState.Solution)
                    solutionPhasesString.Add(phase.ToString());
            }

            var specialFilters = new Dictionary<string, List<string>>();
            if (purePhasesOnlyString.Count > 0 && solutionPhasesString.Count > 0)
            {
                purePhasesOnlyString.Add(currentPhase.ToString());
                solutionPhasesString.Add(currentPhase.ToString());
                specialFilters.Add(filterPureOnly, purePhasesOnlyString);
                specialFilters.Add(filterSolutionOnly, solutionPhasesString);
                ccMctcPlot.SpecialFilters = specialFilters;
            }
            #endregion

            #region Table
            var columns = new List<string>() { "#", "Obs", "Calc", "Assemblage", "Batch", "P [kbar]", "T [°C]", "Visible" };

            var rows = new List<List<object>>();
            int counter = 1;
            foreach (var mvar in modelVars)
            {
                var row = new List<object>
                {
                    counter++.ToString("000"),
                    mvar.ObsVal,
                    mvar.CalcVal,
                    mvar.Batch.GetPhaseAssemblageString(),
                    mvar.Batch,
                    mvar.Batch.ExperimentalRun.Pressure,
                    mvar.Batch.ExperimentalRun.Temperature,
                    true
                };

                rows.Add(row);
            }

            tcPlotInfo.ClearTable();
            tcPlotInfo.FillTable(columns, rows);
            tcPlotInfo.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tcPlotInfo.MultiSelect = false;
            #endregion
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            DialogResult result = saveDialog.ShowDialog();
            if (result == DialogResult.OK)
                SaveAsJSON(saveDialog.FileName);
        }

        private void SaveAsJSON(string fileName)
        {
            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            var json = JsonConvert.SerializeObject(_dataFile, jsonSettings);
            File.WriteAllText(fileName, json);
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            tcPlotInfo.ClearTable();
            ccMctcPlot.ClearPlot();
            _batches = null;
            _modelVariables = null;
            _dataFile = null;

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            DialogResult result = openDialog.ShowDialog();
            if (result == DialogResult.OK)
                OpenJSON(openDialog.FileName);

            Cursor = Cursors.Default;
        }

        private void OpenJSON(string fileName)
        {
            var json = File.ReadAllText(fileName);
            _dataFile = JsonConvert.DeserializeObject<DataFile>(json);
            _batches = _dataFile.Batches;
            _modelVariables = _dataFile.ModelVariables;

            PopulateUI();
        }
    }
}
