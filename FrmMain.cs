using DatabaseReader.Definitions;
using DatabaseReader.Enums;
using DatabaseReader.Helpers;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static DatabaseReader.Helpers.DoubleExtensions;

namespace DatabaseReader
{
    public partial class FrmMain : Form
    {
        private List<Experiment> _Experiments;
        private ContextMenu _contextMenu;

        public FrmMain()
        {
            InitializeComponent();
            _Experiments = new List<Experiment>();
            SetUpUI();
        }

        private void SetUpUI()
        {
            string path = "C:\\Users\\humandm\\Dropbox\\DATABASE";
            txtPath.Text = path;
            txtPath.Select(0, 0);
            ActiveControl = btnScanDirectory;
            txtSubDirectory.Enabled = ckbxSubDirectory.Checked;

            cmbxManualSelection.Items.Add("None");
            cmbxManualSelection.Items.Add("Mohammad_SelectRuns");
            cmbxManualSelection.SelectedIndex = 0;
            cmbxManualSelection.Enabled = false;
            tabPageSubselectRuns.Hide();
            MakeContextMenu();
        }

        private void btnScanDirectory_Click(object sender, EventArgs e)
        {
            _Experiments.Clear();
            string path = txtPath.Text;
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Choose a directory.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Directory.Exists(path))
            {
                var folders = Directory.GetDirectories(path).ToList();

                cklbxExperiments.Items.Clear();
                foreach (var folder in folders)
                {
                    var info = new DirectoryInfo(folder);
                    if (!info.Name.Any(char.IsDigit)) continue;
                    var files = info.GetFiles("*.xls", ckbxSubDirectory.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                    if (!files.Any()) continue;

                    Experiment experiment = new Experiment(info.Name);
                    experiment.DirectoryInfo = info;
                    _Experiments.Add(experiment);
                    cklbxExperiments.Items.Add(experiment);
                }
            }

            tabControl.Visible = true;
            cklbxExperiments.ContextMenu = _contextMenu;
        }

        private void btnReadExperiments_Click(object sender, EventArgs e)
        {
            cmbxExperiments.Items.Clear();
            cmbxPhases.Items.Clear();

            Cursor = Cursors.WaitCursor;
            foreach (var item in cklbxExperiments.CheckedItems)
            {
                var exp = item as Experiment;
                if (exp == null) continue;
                ReadExperiment(exp);
                ReadPhasesData(exp);

                if (exp.Runs.Any())
                    cmbxExperiments.Items.Add(exp);
            }
            Cursor = Cursors.Default;

            if (cmbxExperiments.Items.Count > 0)
                cmbxExperiments.SelectedIndex = 0;
            else
            {
                tcExperiment.ClearTable();
                tcPhases.ClearTable();
            }

            PrepareAdvancedDataManagerUI();
        }

        private void ReadExperiment(Experiment experiment)
        {
            DirectoryInfo info = experiment.DirectoryInfo;
            if (info == null) return;
            List<FileInfo> subFiles = info.GetFiles("*.xls", SearchOption.TopDirectoryOnly).ToList();
            if (ckbxSubDirectory.Checked)
            {
                string subPath = info.FullName + $"\\{txtSubDirectory.Text}";
                if (Directory.Exists(subPath))
                {
                    var filePaths = Directory.GetFiles(subPath, "*.xls").ToList();
                    subFiles.Clear();
                    foreach (var path in filePaths)
                        subFiles.Add(new FileInfo(path));
                }
            }

            if (subFiles == null || subFiles.Count == 0) return;
            subFiles.Remove(subFiles.FirstOrDefault(f => Path.GetFileNameWithoutExtension(f.FullName) == info.Name));
            experiment.Files = subFiles;
            var expFile = subFiles.FirstOrDefault(f => f.Name.ToLower().Contains("experiment"));
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

                    if (nameIndex < 0 || pressureIndex < 0 || temperatureIndex < 0 || durationIndex < 0 || pressureIndex < 0) return;

                    experiment.Runs.Clear();
                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        var cells = table.Rows[i].ItemArray;
                        ExperimentalRun run = new ExperimentalRun();

                        if (indexIndex >= 0)
                            run.ReadingIndex = (int)GetDoubleValueFromText(cells[indexIndex].ToString());

                        run.Name = cells[nameIndex].ToString();
                        if (string.IsNullOrEmpty(run.Name)) continue;

                        run.Pressure = GetDoubleValueFromText(cells[pressureIndex].ToString());
                        if (double.IsNaN(run.Pressure)) continue;
                        run.Pressure *= 10.0; //GPa to KBar

                        run.Temperature = GetDoubleValueFromText(cells[temperatureIndex].ToString());
                        if (double.IsNaN(run.Temperature)) continue;

                        run.Duration = GetDoubleValueFromText(cells[durationIndex].ToString());

                        var phases = cells[phasesIndex].ToString().Split(new char[] { '+', ',' });
                        foreach (var ph in phases)
                        {
                            var phase = new Phase(ph);
                            phase.ExperimentalRun = run;
                            run.Phases.Add(phase);
                        }

                        var oxides = Enum.GetValues(typeof(OxideType));
                        run.BulkOxides.Clear();
                        run.BulkComposition.Clear();
                        var indx = 0;
                        foreach (var oxide in oxides)
                        {
                            var index = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains(oxide.ToString()) && !c.ToString().Contains("err")));
                            if (index < 0) continue;
                            run.BulkOxides.Add((OxideType)oxide);
                            double value = GetDoubleValueFromText(cells[index].ToString());
                            run.BulkComposition.Add(double.IsNaN(value) ? 0 : value);
                        }

                        run.Index = i;
                        run.Experiment = experiment;
                        experiment.Runs.Add(run);
                    }
                }
            }
        }

        private void cmbxExperiments_SelectedIndexChanged(object sender, EventArgs e)
        {
            var exp = (Experiment)cmbxExperiments.SelectedItem;
            int indx = cklbxExperiments.FindString(exp.Name);
            cklbxExperiments.SelectedIndex = indx;

            var columns = new List<string>() { "#", "Name", "P [Kbar]", "T [°C]", "t [hr]", "Phases" };
            foreach (var oxide in exp.Runs[0].BulkOxides)
                columns.Add(oxide.ToString());

            var values = new List<List<object>>();
            foreach (var run in exp.Runs)
            {
                var vals = new List<object>
                {
                    run.Index.ToString("00"),
                    run.Name,
                    run.Pressure,
                    run.Temperature,
                    run.Duration
                };

                string phases = "";
                foreach (var phase in run.Phases) phases += phase.ToString() + "+";
                vals.Add(phases.TrimEnd('+'));

                foreach (var comp in run.BulkComposition)
                    vals.Add(comp);

                values.Add(vals);
            }

            tcExperiment.FillTable(columns, values);

            cmbxPhases.Items.Clear();
            foreach (var phase in exp.PhasesWithData)
                cmbxPhases.Items.Add(EnumExtensions.GetDescriptionFromValue(phase));

            if (cmbxPhases.Items.Count > 0)
                cmbxPhases.SelectedIndex = 0;
        }

        private void ReadPhasesData(Experiment experiment)
        {
            var exp = experiment;
            var info = exp.DirectoryInfo;
            var phaseFiles = exp.Files.Except(exp.Files.Where(f => Path.GetFileNameWithoutExtension(f.FullName).ToLower().Contains("experiment")));
            if (phaseFiles == null || phaseFiles.Count() == 0) return;
            exp.PhasesWithData.Clear();
            foreach (var file in phaseFiles)
            {
                string phaseName = Path.GetFileNameWithoutExtension(file.FullName);
                var phaseType = PhaseTypeExtensions.GetPhaseTypeByName(phaseName.ToLower());

                if (!exp.PhasesWithData.Contains(phaseType))
                    exp.PhasesWithData.Add(phaseType);

                using (var stream = File.Open(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var result = reader.AsDataSet();
                        var table = result.Tables[0];
                        var headerRow = table.Rows[0].ItemArray.ToList();
                        int experimentIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().ToLower().Contains("index")));
                        int commentIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().ToLower().Contains("comment")));

                        if (experimentIndex < 0) continue;

                        for (int i = 1; i < table.Rows.Count; i++)
                        {
                            var cells = table.Rows[i].ItemArray;
                            var experimentName = cells[experimentIndex].ToString();
                            if (string.IsNullOrEmpty(experimentName)) continue;
                            int.TryParse(experimentName, out int intName); // some runs are read by their index in experiment.xls file
                            var run = exp.Runs.FirstOrDefault(r => r.Name == experimentName || r.ReadingIndex == intName);
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
                            foreach (var oxide in oxides)
                            {
                                var oxideIndex = headerRow.IndexOf(headerRow.FirstOrDefault(c => c.ToString().Contains(oxide.ToString()) && !c.ToString().Contains("err")));
                                if (oxideIndex < 0) continue;
                                runPhase.Oxides.Add((OxideType)oxide);
                                double value = GetDoubleValueFromText(cells[oxideIndex].ToString());
                                runPhase.Composition.Add(double.IsNaN(value) ? 0 : value);
                            }
                        }
                    }
                }
            }
        }

        private void cmbxPhases_SelectedIndexChanged(object sender, EventArgs e)
        {
            tcPhases.ClearTable();
            var exp = cmbxExperiments.SelectedItem as Experiment;
            var phaseType = PhaseTypeExtensions.GetPhaseTypeByName(cmbxPhases.SelectedItem.ToString());

            var allInstances = new List<Phase>();
            foreach (var run in exp.Runs.Where(r => r.Phases.Any(ph => ph.Type == phaseType)))
                allInstances.Add(run.Phases.FirstOrDefault(ph => ph.Type == phaseType));

            if (!allInstances.Any()) return;
            int maxOxideCount = allInstances.Max(i => i.Oxides.Count());
            var oxides = allInstances.FirstOrDefault(i => i.Oxides.Count() == maxOxideCount).Oxides;

            var columns = new List<string>() { "Comment", "Run" };
            foreach (var oxide in oxides)
                columns.Add(oxide.ToString());

            var values = new List<List<object>>();
            foreach (var phaseInstance in allInstances.Where(i => i.Composition.Any()))
            {
                var row = new List<object>
                {
                    phaseInstance.Comment,
                    phaseInstance.ExperimentalRun
                };

                foreach (var comp in phaseInstance.Composition)
                    row.Add(comp.Round(8));

                values.Add(row);
            }

            tcPhases.FillTable(columns, values);
        }

        private void ckbxSubDirectory_CheckedChanged(object sender, EventArgs e)
        {
            txtSubDirectory.Enabled = ckbxSubDirectory.Checked;
        }

        private void cklbxExperiments_MouseDown(object sender, MouseEventArgs e)
        {
            if (cklbxExperiments.Items.Count == 0) return;

            cklbxExperiments.SelectedIndex = cklbxExperiments.IndexFromPoint(e.X, e.Y);

            if (e.Button == MouseButtons.Middle)
            {
                var exp = cklbxExperiments.SelectedItem as Experiment;
                if (exp == null) return;
                Process.Start(exp.DirectoryInfo.FullName);
            }
            
        }

        private void cmbxManualSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cklbxExperiments.Items.Count; i++)
                cklbxExperiments.SetItemCheckState(i, CheckState.Unchecked);

            if (cmbxManualSelection.SelectedItem.ToString().ToLower().Contains("mohammad"))
            {
                var list = new List<string>()
                {
                    "Auzanneauetal2006",
                    "Elazaretal2019",
                    "Foleyetal2009",
                    "Greenetal2014",
                    "HermannSpandler2008",
                    "JokobssonHolloway2007",
                    "Mauriceetal2020",
                    "PatinoDouce2005",
                    "PatinoDouceBeard1995",
                    "RappWatson1995",
                    "SenDunn1994",
                    "SkjerlieJohnston1993",
                    "SkjerlieJohnston1996",
                    "SkjerliePatinoDouce2002",
                    "Skoraetal2015",
                    "Spandleretal2010",
                    "SpringerSeck1997",
                    "VielzeufMontel1994",
                    "Wangetal2010",
                    "WolfWyllie1994",
                    "Ziaja2012"
                };

                foreach (var item in list)
                {
                    var indx = cklbxExperiments.FindString(item);
                    cklbxExperiments.SetItemChecked(indx, true);
                }
            }
        }

        private void ckbxManualSelection_CheckedChanged(object sender, EventArgs e)
        {
            cmbxManualSelection.Enabled = ckbxManualSelection.Checked;
        }

        private void btnSubselectRuns_Click(object sender, EventArgs e)
        {
            if (GetListOfPhaseTypes().Any(c => c == PhaseType.NULL))
            {
                MessageBox.Show("Unrecognized phase 'NULL'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            tabPageSubselectRuns.Show();
            tabControl.SelectedTab = tabPageSubselectRuns;
            var filterTypes = Enum.GetValues(typeof(RunFilterType));
            cmbxRunFilterType.Items.Clear();
            foreach (var filter in filterTypes)
                cmbxRunFilterType.Items.Add(filter);

            cmbxRunFilterType.SelectedIndex = 0;
            flpFilters.Controls.Clear();

        }

        private void MakeContextMenu()
        {
            _contextMenu = new ContextMenu();
            MenuItem selectAll = new MenuItem("Select All", SelectAll_Click);
            MenuItem selectNone = new MenuItem("Select None", SelectNone_Click);
            _contextMenu.MenuItems.Add(selectAll);
            _contextMenu.MenuItems.Add(selectNone);

        }

        private void SelectNone_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem == null) return;
            ContextMenu menu = menuItem.GetContextMenu();
            if (menu == null) return;
            var cklbx = menu.SourceControl as CheckedListBox;
            if (cklbx == null) return;

            if (cklbx == null) return;
            for (int i = 0; i < cklbx.Items.Count; i++)
                cklbx.SetItemCheckState(i, CheckState.Unchecked);
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            if (menuItem == null) return;
            ContextMenu menu = menuItem.GetContextMenu();
            if (menu == null) return;
            var cklbx = menu.SourceControl as CheckedListBox;
            if (cklbx == null) return;

            if (cklbx == null) return;
            for (int i = 0; i < cklbx.Items.Count; i++)
                cklbx.SetItemCheckState(i, CheckState.Checked);
        }

        private void btnAddRunFilterType_Click(object sender, EventArgs e)
        {
            var runFilterType = (RunFilterType)cmbxRunFilterType.SelectedItem;
            if (runFilterType == null) return;


            var phaseTypes = GetListOfPhaseTypes();
            switch (runFilterType)
            {
                case RunFilterType.None:
                    break;
                case RunFilterType.MustContainUnused:
                case RunFilterType.MustContainAll:
                case RunFilterType.MustContainAny:
                case RunFilterType.MustNotContainAll:
                    AddRunFilterUI_PhasesList(phaseTypes, runFilterType);
                    break;
                case RunFilterType.MinPhaseCount:
                case RunFilterType.MaxPhaseCount:
                    AddRunFilterUI_PhasesCount(phaseTypes, runFilterType);
                    break;
                case RunFilterType.PressureBound:
                case RunFilterType.TemperatureBound:
                    AddRunFilterUI_BoundedPT(runFilterType);
                    break;
                case RunFilterType.IncludedExperiments:
                case RunFilterType.ExcludedExperiments:
                    AddRunFilterUI_Experiments(runFilterType);
                    break;
                default:
                    break;
            }

            var cmbxItemIndex = cmbxRunFilterType.FindString(runFilterType.ToString());
            cmbxRunFilterType.Items.RemoveAt(cmbxItemIndex);
            if (cmbxRunFilterType.Items.Count > 0)
                cmbxRunFilterType.SelectedIndex = 0;
        }

        private List<PhaseType> GetListOfPhaseTypes()
        {
            var experiments = new List<Experiment>();
            foreach (var item in cmbxExperiments.Items)
            {
                var exp = (Experiment)item;
                if (exp == null) continue;
                experiments.Add(exp);
            }

            var phaseTypes = experiments.SelectMany(e => e.PhasesWithData);
            return phaseTypes.Distinct().ToList();
        }

        private void AddRunFilterUI_PhasesList(List<PhaseType> phaseTypes, RunFilterType filterType)
        {
            var groupBox = new GroupBox();
            groupBox.Text = filterType.ToString();

            if (phaseTypes == null) return;
            var cklbx = new CheckedListBox();
            foreach (var phaseType in phaseTypes)
                cklbx.Items.Add(phaseType);

            cklbx.Location = new System.Drawing.Point(groupBox.Location.X + 5, groupBox.Location.Y + 15);
            cklbx.ColumnWidth = 60;
            cklbx.MultiColumn = true;
            cklbx.CheckOnClick = true;
            cklbx.BorderStyle = BorderStyle.None;
            cklbx.Dock = DockStyle.Fill;
            cklbx.ContextMenu = _contextMenu;
            cklbx.Tag = filterType;

            groupBox.Controls.Add(cklbx);
            groupBox.Width = flpFilters.Width - 10;
            groupBox.Tag = filterType;
            flpFilters.Controls.Add(groupBox);
        }

        private void AddRunFilterUI_PhasesCount(List<PhaseType> phaseTypes, RunFilterType filterType)
        {
            var groupBox = new GroupBox();
            groupBox.Text = filterType.ToString();

            var nudCount = new NumericUpDown();
            nudCount.Location = new System.Drawing.Point(groupBox.Location.X + 5, groupBox.Location.Y + 15);
            nudCount.Dock = DockStyle.Fill;
            nudCount.Minimum = 3;
            nudCount.Maximum = phaseTypes.Count;
            nudCount.Value = 3;
            nudCount.Increment = 1;
            nudCount.Tag = filterType;

            groupBox.Controls.Add(nudCount);
            groupBox.Width = flpFilters.Width - 10;
            groupBox.Tag = filterType;
            groupBox.Height = nudCount.Height + 30;
            flpFilters.Controls.Add(groupBox);
        }

        private void AddRunFilterUI_BoundedPT(RunFilterType filterType)
        {
            var groupBox = new GroupBox();
            groupBox.Text = filterType.ToString();
            if (filterType == RunFilterType.PressureBound)
                groupBox.Text += " [Kbar]";
            else if (filterType == RunFilterType.TemperatureBound)
                groupBox.Text += " [°C]";

            var txtBox = new TextBox();
            txtBox.Location = new System.Drawing.Point(groupBox.Location.X + 5, groupBox.Location.Y + 15);
            txtBox.Dock = DockStyle.Fill;
            txtBox.Text = "";
            txtBox.Select(0, 0);
            txtBox.Tag = filterType;

            groupBox.Controls.Add(txtBox);
            groupBox.Width = flpFilters.Width - 10;
            groupBox.Tag = filterType;
            groupBox.Height = txtBox.Height + 30;
            flpFilters.Controls.Add(groupBox);
        }

        private void AddRunFilterUI_Experiments(RunFilterType filterType)
        {
            var groupBox = new GroupBox();
            groupBox.Text = filterType.ToString();

            var cklbx = new CheckedListBox();
            foreach (var exp in cmbxExperiments.Items)
                cklbx.Items.Add(exp as Experiment);

            cklbx.Location = new System.Drawing.Point(groupBox.Location.X + 5, groupBox.Location.Y + 15);
            cklbx.CheckOnClick = true;
            cklbx.BorderStyle = BorderStyle.None;
            cklbx.Dock = DockStyle.Fill;
            cklbx.ContextMenu = _contextMenu;
            cklbx.Tag = filterType;

            groupBox.Controls.Add(cklbx);
            groupBox.Width = flpFilters.Width - 10;
            groupBox.Tag = filterType;
            flpFilters.Controls.Add(groupBox);
        }

        private void btnGenerateRuns_Click(object sender, EventArgs e)
        {
            var SubsetRuns = new List<SubsetRun>();

            var Experiments = new List<Experiment>();
            foreach (var experiment in Experiments)
            {
                var runs = new List<ExperimentalRun>();

            }
        }

        private void PrepareAdvancedDataManagerUI()
        {
            var phaseList = GetListOfPhaseTypes();
            cmbxDMAdvancedPhaseList.Items.Clear();
            cklbxDMAdvancedOxides.Items.Clear();
            if (phaseList.Count == 0) return;
            foreach (var phase in phaseList)
                cmbxDMAdvancedPhaseList.Items.Add(EnumExtensions.GetDescriptionFromValue(phase));

            cmbxDMAdvancedPhaseList.SelectedIndexChanged -= CmbxDMAdvancedPhaseList_SelectedIndexChanged;
            cmbxDMAdvancedPhaseList.SelectedIndexChanged += CmbxDMAdvancedPhaseList_SelectedIndexChanged;
            cmbxDMAdvancedPhaseList.SelectedIndex = 0;

            cklbxDMAdvancedOxides.ContextMenu = _contextMenu;
            cklbxDMAdvancedOxides.ColumnWidth = 60;
            var oxides = Enum.GetValues(typeof(OxideType));
            foreach (var oxide in oxides)
                cklbxDMAdvancedOxides.Items.Add(oxide);
        }

        private void CmbxDMAdvancedPhaseList_SelectedIndexChanged(object sender, EventArgs e)
        {
            tcAdvancedDataManager.ClearTable();

            var Columns = new List<string>() { "#", "Experiment", "Run" };
            var oxides = Enum.GetValues(typeof(OxideType));
            foreach (var oxide in oxides)
                Columns.Add(oxide.ToString());

            var Rows = new List<List<object>>();
            int counter = 1;
            foreach (var item in cmbxExperiments.Items)
            {
                var exp = item as Experiment;
                var phaseType = PhaseTypeExtensions.GetPhaseTypeByName(cmbxDMAdvancedPhaseList.SelectedItem.ToString());

                var allInstances = new List<Phase>();
                foreach (var run in exp.Runs.Where(r => r.Phases.Any(ph => ph.Type == phaseType)))
                    allInstances.Add(run.Phases.FirstOrDefault(ph => ph.Type == phaseType));

                if (!allInstances.Any()) continue;

                foreach (var phaseInstance in allInstances.Where(x => x.Composition.Any()))
                {
                    var row = new List<object>
                    {
                        counter++.ToString("00"),
                        phaseInstance.ExperimentalRun.Experiment,
                        phaseInstance.ExperimentalRun
                    };

                    foreach (var oxide in oxides)
                    {
                        var index = phaseInstance.Oxides.IndexOf((OxideType)oxide);
                        if (index >= 0)
                            row.Add(phaseInstance.Composition[index].Round(4));
                        else
                            row.Add(0.0);
                    }

                    Rows.Add(row);
                }
            }

            tcAdvancedDataManager.FillTable(Columns, Rows);
        }
    }
}
