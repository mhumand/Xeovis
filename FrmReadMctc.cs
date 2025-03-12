using DatabaseReader.CustomControls;
using DatabaseReader.Definitions;
using DatabaseReader.Definitions.Modelling;
using DatabaseReader.Definitions.Persistence;
using DatabaseReader.Definitions.UI;
using DatabaseReader.Enums;
using DatabaseReader.Enums.Modelling;
using DatabaseReader.Enums.UI;
using DatabaseReader.Events;
using DatabaseReader.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace DatabaseReader
{
    public partial class FrmReadMctc : Form
    {
        private string _directory;
        private List<string> _files;
        private ContextMenu _contextMenu;
        private DataFile _dataFile;
        private ProjectSettings _projectSettings;

        private List<Batch> _batches;
        private List<ModelVariable> _allModelVariables;
        private Dictionary<ModelVariable, List<ModelVariable>> _selectedModelVariables;
        private MctcPlotSettings _mctcPlotSettings;
        private List<MctcVariable> _mctcVariables;
        private int _mctcRunIndex;

        public FrmReadMctc()
        {
            InitializeComponent();
            PrepareUI();
        }

        private void PrepareUI()
        {
            MakeContextMenu();
            _mctcPlotSettings = new MctcPlotSettings();
            _projectSettings = new ProjectSettings();
            tvCaseSelection.ContextMenu = _contextMenu;
            tcBatchDetails.TableRowSelected += tcPlotInfo_RowSelected;
            ccMctcPlot.ChartPointSelected += ccMctcPlot_ChartPointSelected;
            ccMctcPlot.ClearPlot();
            ckbxShowBatchDetails.Checked = false;
            splitContainerDetailedTab.SplitterDistance = (int)(0.55 * tabpageDetailed.Width);

            var plotGroupingTypes = Enum.GetValues(typeof(PlotPointsGroupingType));
            foreach (PlotPointsGroupingType item in plotGroupingTypes)
                cmbxPlotPointsGrouping.Items.Add(item);
            cmbxPlotPointsGrouping.SelectedIndex = 0;

            var panelType = tableLayoutPanel.GetType();
            PropertyInfo info = panelType.GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(tableLayoutPanel, true);
        }

        private void ccMctcPlot_ChartPointSelected(object sender, ChartPointSelectedEventArgs e)
        {
            var table = tcBatchDetails.DataTable;
            Guid id = Guid.NewGuid();

            if (!double.IsNaN(e.X) && !double.IsNaN(e.Y))
            {
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if ((double)table.Rows[i][1] == e.X && (double)table.Rows[i][2] == e.Y)//todo - generalize
                    {
                        id = (Guid)table.Rows[i][tcBatchDetails.IDColumn];
                        break;
                    }
                }
            }

            tcBatchDetails.SelectRow(id);
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

        private void tsmReadMCTC_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_directory))
                if (!ShowDirectoryBrowsingDialog()) return;

            ReadMCTCDirectoryAndPopulateUI();
        }

        private void ReadMCTCDirectoryAndPopulateUI()
        {
            if (!tsmFixedUIOnReading.Checked)
            {
                tcBatchDetails.ClearTable();
                ccMctcPlot.ClearPlot();
            }

            _batches = null;
            _allModelVariables = null;
            _mctcVariables = null;
            _dataFile = null;

            _files = Directory.GetFiles(_directory).ToList();

            Cursor = Cursors.WaitCursor;
            ReadBatchLines();
            ReadBatchBulks();

            if (_batches != null)
                ReadModelVariables();

            if (_allModelVariables != null)
                ReadMctcResults();

            Cursor = Cursors.Default;

            _allModelVariables?.RemoveAll(mv => mv.Batch.IsAllFailed());
            _batches?.RemoveAll(b => b.IsAllFailed());

            if (_batches.Count == 0 || _mctcVariables == null) return;
            PopulateUI();
        }

        private bool ShowDirectoryBrowsingDialog()
        {
            var dialog = new FolderBrowserDialog();
            dialog.Description = "Select the directory that contains THERMOCALC MTCT output txt files.";
            dialog.SelectedPath = "H:\\My Drive\\PhD @ Unimelb\\PhD\\PhD - Project\\Project - Calculations\\set_up_fitting\\TC RUN";
            if (dialog.ShowDialog() == DialogResult.Cancel)
                return false;

            if (!Directory.Exists(dialog.SelectedPath))
            {
                MessageBox.Show("Directory does not exist.");
                _directory = null;
                return false;
            }

            _directory = dialog.SelectedPath;
            return true;
        }

        private void tsmResetMCTCDirectory_Click(object sender, EventArgs e)
        {
            _directory = null;
            tsmReadMCTC.PerformClick();
        }

        private void PopulateUI()
        {
            var checkedNodesIndex = tvCaseSelection.GetCheckedNodesIndex();
            var selectedModelVar = cmbxModelVariable.SelectedIndex;

            PopulateTreeView_PhaseModelsOnly();

            nudMctcRunNumber.Maximum = _mctcVariables.FirstOrDefault().Values.Count;
            tbMctcRunNumber.Maximum = (int)nudMctcRunNumber.Maximum;
            nudMctcRunNumber.Value = 1;
            tbMctcRunNumber.Value = 1;

            // keeping UI Fixed
            var allNodes = tvCaseSelection.GetAllNodes();
            if (tsmFixedUIOnReading.Checked && checkedNodesIndex.Count <= allNodes.Count)
            {
                tvCaseSelection.AfterCheck -= tvCaseSelection_AfterCheck;
                foreach (int index in checkedNodesIndex)
                    allNodes[index].Checked = true;

                tvCaseSelection.AfterCheck += tvCaseSelection_AfterCheck;

                RefreshSinglePlotAndTable();
            }
        }

        private void PopulateTreeView_PhaseModelsOnly()
        {
            tvCaseSelection.Nodes.Clear();
            if (_allModelVariables == null || _allModelVariables.Count == 0 || _batches.Count == 0) return;
            var allPhases = _allModelVariables.Select(v => v.PhaseType).ToList();
            var phases = allPhases.Distinct();

            foreach (var phase in phases)
            {
                var phaseNode = new TreeNode(phase.ToString() + " - " + EnumExtensions.GetDescriptionFromValue(phase));
                phaseNode.Tag = phase;
                var batches = _batches.Where(b => b.ModelVariables.Any(mv => mv.PhaseType == phase)).ToList();
                var allModelVars = batches.SelectMany(b => b.ModelVariables.Where(v => v.PhaseType == phase)).ToList();
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
            _allModelVariables = new List<ModelVariable>();
            for (int i = 0; i < lines.Count; i++)
            {
                var varData = lines[i].Split(new char[] { '(', ')', '-' }).ToList();
                varData.RemoveAll(l => string.IsNullOrWhiteSpace(l));

                var modelVariable = new ModelVariable();
                modelVariable.PhaseType = PhaseTypeExtensions.GetPhaseTypeByName(varData[1]);
                modelVariable.VariableType = EnumExtensions.GetValueFromDescription<ModelVariableType>(varData[0]);
                int.TryParse(varData[2].TrimStart(new char[] {'b'}), out int batchIndex);
                modelVariable.Batch = _batches.FirstOrDefault(b => b.Index == batchIndex);

                i++;
                modelVariable.ObsVal = DoubleExtensions.GetDoubleValueFromText(lines[i]);

                modelVariable.Batch.ModelVariables.Add(modelVariable);
                _allModelVariables.Add(modelVariable);
            }
        }

        private void ReadMctcResults()
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

                    var modelVarsInResults = new List<ModelVariable>();
                    for (int j = 0; j < modVarNameLines.Count; j++)
                    {
                        var varName = modVarNameLines[j];
                        j++;
                        var varPhase = PhaseTypeExtensions.GetPhaseTypeByName(modVarNameLines[j]);
                        var modelVariable = batch.ModelVariables.FirstOrDefault(v => v.VariableType.ToString() == varName && v.PhaseType == varPhase);
                        if (modelVariable == null) continue;
                        modelVariable.CalcVals.Add(k, DoubleExtensions.GetDoubleValueFromText(modVarValueLines[2 + (j + 1) / 2]));
                        modelVarsInResults.Add(modelVariable);
                    }

                    var otherModelVars = batch.ModelVariables.Except(modelVarsInResults).ToList();
                    foreach (var modelVar in otherModelVars)
                    {
                        _allModelVariables.Remove(modelVar);
                        batch.ModelVariables.Remove(modelVar);
                    }
                }
            }
        }

        private List<MctcVariable> GetMctcVariables(List<string> lines)
        {
            var allRange = Enumerable.Range(0, lines.Count);
            var splitters = new char[] { '{', '}', '*', '(', ')', '\t', '"', ',', ';' };

            var mcinfoIndex = allRange.Where(i => lines[i].Contains("mcinfo")).ToList()[0];
            var mcrangeIndex = allRange.Where(i => lines[i].Contains("mcrange")).ToList()[0];
            var mctcVarLineCount = mcrangeIndex - mcinfoIndex;
            var mctcVariables = new List<MctcVariable>();
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
                var mctcVariable = new MctcVariable() { Index = i, Name = name, StartingValue = startingVal, Range = rangeVal };
                mctcVariables.Add(mctcVariable);
            }

            return mctcVariables;
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
            tcBatchDetails.ClearTable();

            if (_allModelVariables == null) return;
            var checkedNodes = tvCaseSelection.GetCheckedNodes();
            if (!checkedNodes.Any()) return;

            var modelVarNodes = checkedNodes.Where(n => n.Tag is ModelVariable).ToList();
            if (!modelVarNodes.Any()) return;

            cmbxModelVariable.Items.Clear();
            foreach (var item in modelVarNodes)
                cmbxModelVariable.Items.Add(item.Tag);

            // preparing for overview
            _selectedModelVariables = new Dictionary<ModelVariable, List<ModelVariable>>();
            foreach (ModelVariable modelVariable in cmbxModelVariable.Items)
            {
                if (modelVariable == null) continue;
                var modelVarList = _allModelVariables.Where(mv => mv.Name == modelVariable.Name).ToList();
                _selectedModelVariables.Add(modelVariable, modelVarList);
            }

            //
            int currentSelectedIndex = cmbxModelVariable.SelectedIndex;
            var currentSelectedItem = cmbxModelVariable.SelectedItem as ModelVariable;
            bool indexCondition = currentSelectedIndex < cmbxModelVariable.Items.Count && currentSelectedIndex >= 0;
            bool phaseCondition = currentSelectedItem != null && currentSelectedItem.PhaseType == (modelVarNodes[0].Tag as ModelVariable).PhaseType;
            if (indexCondition && phaseCondition)
                cmbxModelVariable.SelectedIndex = currentSelectedIndex;
            else
                cmbxModelVariable.SelectedIndex = 0;

            FillMctcDetailsTable();
            PrepareTableLayoutPanel((int)nudLayoutColumnCount.Value);
            PlotAll();
        }

        private void PrepareTableLayoutPanel(int columnCount)
        {
            if (_selectedModelVariables == null || _selectedModelVariables.Count == 0) return;
            var panel = tableLayoutPanel;
            panel.Controls.Clear();

            panel.ColumnCount = columnCount;
            panel.RowCount = (int)Math.Ceiling(_selectedModelVariables.Count * 1.0 / tableLayoutPanel.ColumnCount);
            panel.VerticalScroll.Visible = true;

            panel.ColumnStyles.Clear();
            panel.RowStyles.Clear();

            for (int i = 0; i < panel.ColumnCount; ++i)
                panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            for (int i = 0; i < panel.RowCount; ++i)
                panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            //for (int i = 0; i < panel.RowCount; ++i)
            //    panel.RowStyles.Add(new RowStyle(SizeType.Absolute, panel.Width / columnCount - columnCount * 10));
        }

        private void cmbxModelVariable_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshSinglePlotAndTable();
        }

        private void RefreshSinglePlotAndTable()
        {
            tcBatchDetails.TableRowSelected -= tcPlotInfo_RowSelected;
            ccMctcPlot.ChartPointSelected -= ccMctcPlot_ChartPointSelected;

            PlotAndTabulate_SingleDetailed();

            tcBatchDetails.TableRowSelected += tcPlotInfo_RowSelected;
            ccMctcPlot.ChartPointSelected += ccMctcPlot_ChartPointSelected;
        }

        private void PlotAll(bool useCurrentCharts = false)
        {
            if (_selectedModelVariables == null || _selectedModelVariables.Count == 0) return;
            if (!useCurrentCharts)
                tableLayoutPanel.Controls.Clear();

            int counter = 0;
            for (int i = 0; i < tableLayoutPanel.RowCount; i++)
            {
                for (int j = 0; j < tableLayoutPanel.ColumnCount; j++)
                {
                    if (counter >= _selectedModelVariables.Count) break;
                    var modelVars = _selectedModelVariables.ElementAt(counter).Value;

                    var chartControl = new ChartControl();
                    if (useCurrentCharts)
                        chartControl = tableLayoutPanel.GetControlFromPosition(j, i) as ChartControl;
                    
                    if (!useCurrentCharts || chartControl == null)
                    {
                        chartControl = new ChartControl();
                        chartControl.Dock = DockStyle.Fill;
                        tableLayoutPanel.Controls.Add(chartControl, j, i);
                    }

                    bool isSuccessful = PlotInChartControl(chartControl, modelVars, _mctcRunIndex, false, hideLegend: true, autoScale: true);
                    if (!isSuccessful)
                        tableLayoutPanel.Controls.Remove(chartControl);

                    counter++;

                    chartControl.ChartDoubleClicked += overViewChartControl_DoubleClicked;
                    chartControl.Tag = _selectedModelVariables.ElementAt(counter - 1).Key;
                }
            }
        }

        private void PlotAndTabulate_SingleDetailed()
        {
            if (!(cmbxModelVariable.SelectedItem is ModelVariable selectedModelVar)) return;

            // getting all instances of the selected model variable
            var modelVars = _selectedModelVariables.FirstOrDefault(x => x.Key.PhaseType == selectedModelVar.PhaseType && x.Key.VariableType == selectedModelVar.VariableType).Value;
            if (modelVars.Count == 0) return;

            var isSuccessful = PlotInChartControl(ccMctcPlot, modelVars, _mctcRunIndex, true, legendOnRight: true);
            if (!isSuccessful)
            {
                ccMctcPlot.ClearPlot();
                tcBatchDetails.ClearTable();
                return;
            }

            #region Table
            var columns = new List<string>() { "#", "Obs", "Calc", "Err%", "Assemblage", "Batch", "P [kbar]", "T [°C]" };
            var rows = new List<List<object>>();
            int counter = 1;
            var groups = GroupModelVars(modelVars);
            foreach (var mvar in modelVars.Where(x => groups.Any(g => g.Contains(x)) && x.CalcVals.ContainsKey(_mctcRunIndex)))
            {
                var row = new List<object>
                {
                    counter++.ToString("000"),
                    mvar.ObsVal,
                    mvar.CalcVals[_mctcRunIndex],
                    Math.Round(100.0 * Math.Abs(1.0 - mvar.CalcVals[_mctcRunIndex] / mvar.ObsVal), 1),
                    mvar.Batch.GetPhaseAssemblageString(),
                    mvar.Batch,
                    mvar.Batch.ExperimentalRun.Pressure,
                    mvar.Batch.ExperimentalRun.Temperature
                };

                rows.Add(row);
            }

            tcBatchDetails.ClearTable();
            tcBatchDetails.FillTable(columns, rows);
            tcBatchDetails.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tcBatchDetails.MultiSelect = false;
            #endregion
        }

        private List<IGrouping<string, ModelVariable>> GroupModelVars(List<ModelVariable> modelVars)
        {
            var modelVarGroups = new List<IGrouping<string, ModelVariable>>();

            if (cmbxPlotPointsGrouping.SelectedItem is PlotPointsGroupingType.Batch)
                modelVarGroups = modelVars.GroupBy(mv => mv.Batch.GetPhaseAssemblageString()).ToList();
            else if (cmbxPlotPointsGrouping.SelectedItem is PlotPointsGroupingType.Temperature)
                modelVarGroups = modelVars.GroupBy(mv => mv.Batch.ExperimentalRun.Temperature.ToString()).ToList();
            else if (cmbxPlotPointsGrouping.SelectedItem is PlotPointsGroupingType.Pressure)
                modelVarGroups = modelVars.GroupBy(mv => mv.Batch.ExperimentalRun.Pressure.ToString()).ToList();

            var selectedGroups = new List<IGrouping<string, ModelVariable>>();
            if (ckbxExcludeIrrelevantBatches.Checked && cmbxPlotPointsGrouping.SelectedItem is PlotPointsGroupingType.Batch)
            {
                foreach (var modelVarGroup in modelVarGroups)
                {
                    var modelVar = modelVarGroup.FirstOrDefault();
                    var phases = modelVar.Batch.Phases.Except(new List<PhaseType>() { modelVar.PhaseType });
                    var varType = modelVar.VariableType;

                    var phaseAllCations = new List<CationType>();
                    foreach (var phase in phases)
                        phaseAllCations.AddRange(DefaultPhaseCompositions.GetCationsFromPhaseComposition(phase));

                    var phaseCations = phaseAllCations.Distinct().ToList();
                    var modelVariableCations = DefaultModelDefinitions.GetCationsFromModelVariable(modelVar.PhaseType, modelVar.VariableType);

                    if (phaseCations.Count != 0 && modelVariableCations.Count != 0)
                        if (!phaseCations.Any(c => modelVariableCations.Contains(c)))
                            continue;

                    selectedGroups.Add(modelVarGroup);
                }
            }
            else
                selectedGroups = modelVarGroups;

            return selectedGroups;
        }

        private bool PlotInChartControl(ChartControl chartControl, List<ModelVariable> modelVars, int mctcRunIndex, bool EqualDimensions, bool legendOnRight = false, bool hideLegend = false, bool autoScale = false)
        {
            var selectedGroups = GroupModelVars(modelVars);
            if (selectedGroups.Count == 0)
                return false;

            var modelVar = modelVars.FirstOrDefault();
            var chartModel = new ChartModel();
            chartModel.Title = modelVar + " ~ " + string.Join(" & ", DefaultModelDefinitions.GetCationsFromModelVariable(modelVar.PhaseType, modelVar.VariableType).ToArray());
            chartModel.XAxisLabel = $"Observed {modelVar}";
            chartModel.YAxisLabel = $"Calculated {modelVar}";
            chartModel.HasEqualDimensions = EqualDimensions;
            chartModel.IsLegendVisible = hideLegend ? false : _mctcPlotSettings.IsLegendVisible;
            if (legendOnRight) chartModel.LegendPosition = ZedGraph.LegendPos.Right;
            chartModel.AutoScale = autoScale;

            int colorIndex = 0;
            int markerIndex = 0;
            foreach (var modelVarGroup in selectedGroups)
            {
                var curve = new CurveModel();
                //curve.Name = modelVarGroup.FirstOrDefault().Batch.GetPhaseAssemblageString();
                curve.Name = modelVarGroup.Key;

                curve.Xvals = modelVarGroup.Where(mv => mv.CalcVals.ContainsKey(mctcRunIndex)).Select(mv => mv.ObsVal).ToList();
                curve.Yvals = modelVarGroup.Where(mv => mv.CalcVals.ContainsKey(mctcRunIndex)).Select(mv => mv.CalcVals[mctcRunIndex]).ToList();
                curve.IsLineVisible = false;

                var color = ChartUtilities.GetColor(colorIndex++);
                if (color == Color.Transparent)
                {
                    colorIndex = (colorIndex - 1) % 10;
                    color = ChartUtilities.GetColor(colorIndex++);
                    markerIndex++;
                }

                curve.Transparency = _mctcPlotSettings.ColorTransparencyIn100;
                curve.MarkerColor = color;
                curve.MarkerType = ChartUtilities.GetMarkerType(markerIndex);
                curve.MarkerSize = 10;

                if (curve.Xvals.Count > 0 && curve.Yvals.Count > 0)
                    chartModel.Curves.Add(curve);
            }


            if (chartModel.Curves.Count == 0 || chartModel.Curves.All(c => c.Xvals.Count == 0) || chartModel.Curves.All(c => c.Yvals.Count == 0))
                return false;

            /// x = y line
            var xyCurve = new CurveModel();
            double minVal = Math.Min(chartModel.Curves.Where(c => c.Xvals.Count > 0).Min(c => c.Xvals.Min()), chartModel.Curves.Where(c => c.Yvals.Count > 0).Min(c => c.Yvals.Min()));
            double maxVal = Math.Max(chartModel.Curves.Where(c => c.Xvals.Count > 0).Max(c => c.Xvals.Max()), chartModel.Curves.Where(c => c.Yvals.Count > 0).Max(c => c.Yvals.Max()));
            double difference = maxVal - minVal;
            minVal -= difference * 0.05;
            maxVal += difference * 0.05;
            xyCurve.Xvals = new List<double>() { minVal, maxVal };
            xyCurve.Yvals = new List<double>() { minVal, maxVal };
            xyCurve.IsMarkerVisible = false;
            xyCurve.LineWidth = 2;
            xyCurve.CurveType = Enums.UI.CurveType.FixedLine;
            chartModel.Curves.Add(xyCurve);
            chartControl.Plot(chartModel);

            /// scaling
            var axisX = chartControl.Chart.GraphPane.XAxis;
            axisX.Scale.Min = minVal;
            axisX.Scale.Max = maxVal;
            var axisY = chartControl.Chart.GraphPane.YAxis;
            axisY.Scale.Min = minVal;
            axisY.Scale.Max = maxVal;
            axisY.MajorGrid.IsZeroLine = false;
            chartControl.Chart.Refresh();

            /// settings
            chartControl.UseLabelFiltering = true;

            var filterPureOnly = "Only Pure Phases";
            var filterSolutionOnly = "Only Solution Phases";
            var purePhasesOnlyString = new List<string>();
            var solutionPhasesString = new List<string>();
            var currentPhase = modelVars.FirstOrDefault().PhaseType;
            foreach (PhaseType phase in modelVars.SelectMany(v => v.Batch.Phases).Distinct().Except(new List<PhaseType> { currentPhase }))
            {
                var phaseState = phase.GetPhaseState();
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
                chartControl.SpecialFilters = specialFilters;
            }

            return true;
        }

        private void SaveAsJSON(string fileName)
        {
            _dataFile = new DataFile();
            _dataFile.Batches = _batches;
            _dataFile.AllModelVariables = _allModelVariables;
            //_dataFile.SelectedModelVariables = _selectedModelVariables;
            _dataFile.MctcVariables = _mctcVariables;
            //_dataFile.MctcPlotSettings = _mctcPlotSettings;
            //_dataFile.MctcRunIndex = _mctcRunIndex;
            _dataFile.ProjectSettings = _projectSettings;

            var jsonSettings = new JsonSerializerSettings();
            jsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            jsonSettings.NullValueHandling = NullValueHandling.Ignore;
            jsonSettings.Formatting = Formatting.Indented;
            jsonSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
            var json = JsonConvert.SerializeObject(_dataFile, jsonSettings);
            File.WriteAllText(fileName, json);
        }

        private void OpenJSON(string fileName)
        {
            var json = File.ReadAllText(fileName);
            _dataFile = JsonConvert.DeserializeObject<DataFile>(json);
            _batches = _dataFile.Batches;
            _allModelVariables = _dataFile.AllModelVariables;
            //_selectedModelVariables = _dataFile.SelectedModelVariables;
            _mctcVariables = _dataFile.MctcVariables;
            //_mctcPlotSettings = _dataFile.MctcPlotSettings;
            //_mctcRunIndex = _dataFile.MctcRunIndex;
            _projectSettings = _dataFile.ProjectSettings;

            PopulateUI();
        }

        private void ckbxExcludeIrrelevantBatches_CheckedChanged(object sender, EventArgs e)
        {
            RefreshSinglePlotAndTable();
            PlotAll(true);
        }

        private void tsmOpen_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            tcBatchDetails.ClearTable();
            ccMctcPlot.ClearPlot();
            tvCaseSelection.Nodes.Clear();
            _batches = null;
            _allModelVariables = null;
            _dataFile = null;

            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            DialogResult result = openDialog.ShowDialog();
            if (result == DialogResult.OK)
                OpenJSON(openDialog.FileName);

            Cursor = Cursors.Default;
        }

        private void tsmSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            DialogResult result = saveDialog.ShowDialog();
            if (result == DialogResult.OK)
                SaveAsJSON(saveDialog.FileName);
        }

        private void tsmVariableDefinitions_Click(object sender, EventArgs e)
        {
            using (var dialog = new FrmVariableDefinitions(_projectSettings))
            {
                dialog.ShowDialog();
            }
        }

        private void tsmPlotSettings_Click(object sender, EventArgs e)
        {
            using (var settingDialog = new FrmMctcPlotSettings(_mctcPlotSettings))
            {
                if (settingDialog.ShowDialog() == DialogResult.OK)
                {
                    _mctcPlotSettings = settingDialog.Settings;
                    RefreshSinglePlotAndTable();
                    PlotAll();
                }
            }
        }

        private void tsmFile_DropDownOpening(object sender, EventArgs e)
        {
            tsmResetMCTCDirectory.Visible = _directory != null;
        }

        private void nudMctcRunNumber_ValueChanged(object sender, EventArgs e)
        {
            if ((int)nudMctcRunNumber.Value < 1) return;
            tbMctcRunNumber.Value = (int)nudMctcRunNumber.Value;
            FillMctcDetailsTable();

            RefreshSinglePlotAndTable();
            PlotAll(true);
        }

        private void tbMctcRunNumber_Scroll(object sender, EventArgs e)
        {
            nudMctcRunNumber.Value = tbMctcRunNumber.Value;
            FillMctcDetailsTable();

            RefreshSinglePlotAndTable();
            PlotAll(true);
        }

        private void FillMctcDetailsTable()
        {
            _mctcRunIndex = (int)nudMctcRunNumber.Value - 1;

            var columns = new List<string>();
            var _row = new List<object>();

            foreach (var mctcVar in _mctcVariables)
            {
                columns.Add(mctcVar.Name);
                _row.Add(mctcVar.Values[_mctcRunIndex]);
            }

            tcMctcDetails.ClearTable();
            tcMctcDetails.FillTable(columns, new List<List<object>>() { _row });
        }

        private void nudLayoutColumnCount_ValueChanged(object sender, EventArgs e)
        {
            PrepareTableLayoutPanel((int)nudLayoutColumnCount.Value);
            PlotAll(true);
        }

        private void tsmCalibration_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_directory))
                if (!ShowDirectoryBrowsingDialog()) return;

            var successful = RunThermocalc();

            if (successful)
                ReadMCTCDirectoryAndPopulateUI();
            else
            {
                var res = MessageBox.Show("Run was not successful!\nDo you want to open the -log file?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (res == DialogResult.Yes)
                {
                    var logfilePath = _directory + "\\tc-log.txt";
                    if (File.Exists(logfilePath)) Process.Start(logfilePath);
                }
            }
                
        }

        private bool RunThermocalc()
        {
            /// Use ProcessStartInfo class
            ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.UseShellExecute = false;
            //startInfo.RedirectStandardOutput = true;
            startInfo.FileName = _directory + "\\tc355aWin.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.WorkingDirectory = _directory;
            //startInfo.Verb = "runas";

            Cursor = Cursors.WaitCursor;
            bool result = false;

            try
            {
                /// Start the process with the info we specified.
                /// Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    result = exeProcess.HasExited;
                    //if (result)
                    //{
                    //    var output = exeProcess.StandardOutput.ReadToEnd();
                    //}
                }

                foreach (var process in Process.GetProcessesByName("tc355aWin"))
                {
                    process.Kill();
                }
            }
            catch
            {
                // Log error.
                result = false;
            }

            Cursor = Cursors.Default;
            return result;
        }

        private void ckbxShowBatchDetails_CheckedChanged(object sender, EventArgs e)
        {
            //splitContainerDetailedTab.Panel2Collapsed = ckbxShowBatchDetails.Checked;
            lblBatchDetails.Visible = ckbxShowBatchDetails.Checked;
            tcBatchDetails.Visible = ckbxShowBatchDetails.Checked;
        }
        
        private void overViewChartControl_DoubleClicked(object sender, ChartDoubleClickedEventArgs e)
        {
            var chartControl = sender as ChartControl;
            if (chartControl == null) return;
            if (!(chartControl.Tag is ModelVariable modelVar)) return;
            foreach (ModelVariable item in cmbxModelVariable.Items)
            {
                if (item == null) return;
                if (item.PhaseType == modelVar.PhaseType && item.VariableType == modelVar.VariableType)
                {
                    cmbxModelVariable.SelectedItem = item;
                    break;
                }
            }

            tabControlResults.SelectedTab = tabpageDetailed;
        }
    }
}
