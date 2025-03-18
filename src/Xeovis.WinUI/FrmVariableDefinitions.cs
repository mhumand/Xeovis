using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Xeovis.Base;
using Xeovis.Domain;
using Xeovis.Persistence;

namespace Xeovis.WinUI
{
    public partial class FrmVariableDefinitions : Form
    {
        private string _fileName;
        private ProjectSettings _projectSettings;
        private List<XEOS> _xeosList;

        public FrmVariableDefinitions(ProjectSettings projectSettings)
        {
            InitializeComponent();
            _projectSettings = projectSettings;
            _fileName = projectSettings.AxFileDirectory;
            txtFilePath.Text = _fileName;
        }

        private void btnBrowseAndRead_Click(object sender, EventArgs e)
        {
            txtFilePath.Text = null;
            var dialog = new OpenFileDialog();
            dialog.Filter = "Text|*.txt|All|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                _fileName = dialog.FileName;
                txtFilePath.Text = _fileName;
                ReadAxFile();
                PopulateUI();
            }
        }

        private void ReadAxFile()
        {
            var sr = new StreamReader(_fileName);
            var content = sr.ReadToEnd();
            var lines = content.Split(new char[] { '\n' }).ToList();
            lines.RemoveAll(l => string.IsNullOrWhiteSpace(l));
            var allRange = Enumerable.Range(0, lines.Count);
            var verbatimIndexes = allRange.Where(i => lines[i].Trim() == "verbatim").ToList();
            verbatimIndexes.Add(allRange.Count() - 1);
            var splitters = new char[] { '(', ',', ')', ' ' };

            _xeosList = new List<XEOS>();

            for (int i = 0; i < verbatimIndexes.Count - 1; i++)
            {
                var xeosLines = lines.GetRange(verbatimIndexes[i + 1], verbatimIndexes[i + 2] - verbatimIndexes[i + 1]).ToList();

                var headerLine = lines[verbatimIndexes[i] - 1].Split(new char[] { ' ' }).ToList();
                headerLine.RemoveAll(l => string.IsNullOrWhiteSpace(l));

                int.TryParse(headerLine[1], out int endmemberCount);
                var endmemberLines = xeosLines.Where(l => l.Trim().StartsWith("p(")).ToList();
                var wLines = xeosLines.Where(l => l.Trim().StartsWith("W(")).ToList();

                var xeos = new XEOS();
                xeos.Name = headerLine[0];
                xeos.PhaseType = PhaseTypeExtensions.GetPhaseTypeByName(xeos.Name);

                /// endmembers
                foreach (var emLine in endmemberLines)
                {
                    var emParts = emLine.Split(splitters).ToList();
                    emParts.RemoveAll(l => string.IsNullOrWhiteSpace(l));
                    var endMember = new EndMember();
                    endMember.Name = emParts[1];
                    xeos.EndMembers.Add(endMember);
                }

                /// energy parameters
                foreach (var wLine in wLines)
                {
                    var wParts = wLine.Split(splitters).ToList();
                    wParts.RemoveAll(l => string.IsNullOrWhiteSpace(l));

                    var W = new EnergyParameter();
                    W.Endmember1 = xeos.EndMembers.FirstOrDefault(em => em.Name == wParts[1]);
                    W.Endmember2 = xeos.EndMembers.FirstOrDefault(em => em.Name == wParts[2]);
                    W.A = DoubleExtensions.GetDoubleValueFromText(wParts[3]);
                    W.B = DoubleExtensions.GetDoubleValueFromText(wParts[4]);
                    W.C = DoubleExtensions.GetDoubleValueFromText(wParts[5]);
                    xeos.Ws.Add(W);
                }

                /// alpha parameters
                var asfLineIndex = xeosLines.FindIndex(l => l.Trim() == "asf");
                if (asfLineIndex > 0)
                {
                    foreach (var endmember in xeos.EndMembers)
                    {
                        var alphaLine = xeosLines.GetRange(asfLineIndex, xeosLines.Count - asfLineIndex).Where(l => l.Trim().StartsWith(endmember.Name + " ")
                                                          || l.Trim().StartsWith("a(" + endmember.Name)).ToList()[0];
                        if (string.IsNullOrWhiteSpace(alphaLine)) continue;
                        var alphaParts = alphaLine.Split(splitters).ToList();
                        alphaParts.RemoveAll(l => string.IsNullOrWhiteSpace(l));

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

                /// model variables
                var variableLines = xeosLines.Where(l => l.Trim().Contains($"({xeos.Name})")).ToList();
                foreach (var varLine in variableLines)
                {
                    var variableLine = varLine.Trim();
                    if (varLine.StartsWith("p") || varLine.StartsWith("W(")) continue;
                    var varParts = varLine.Split(splitters).ToList();
                    varParts.RemoveAll(l => string.IsNullOrWhiteSpace(l));

                    var variable = new ModelVariable();
                    variable.VariableType = EnumExtensions.GetValueFromDescription<ModelVariableType>(varParts[0]);
                    variable.PhaseType = xeos.PhaseType;
                    variable.DefaultValue = DoubleExtensions.GetDoubleValueFromText(varParts[2]);
                    xeos.Variables.Add(variable);
                }

                _xeosList.Add(xeos);
                i++;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void PopulateUI()
        {
            var oxideList = new List<OxideType>() { OxideType.SiO2, OxideType.Al2O3, OxideType.FeO, OxideType.Fe2O3, OxideType.MgO, OxideType.CaO, OxideType.K2O, OxideType.Na2O, OxideType.Cr2O3, OxideType.TiO2, OxideType.H2O };

            cklbxOxides.Items.Clear();
            foreach (var item in oxideList)
                cklbxOxides.Items.Add(item);

            var cationList = new List<CationType> { CationType.Si, CationType.Al, CationType.Fe2, CationType.Fe3, CationType.Mg, CationType.Ca, CationType.K, CationType.Na, CationType.Cr, CationType.Ti };

            cklbxCations.Items.Clear();
            foreach (var item in cationList)
                cklbxCations.Items.Add(item);

            cmbxPhase.Items.Clear();
            foreach (var xeos in _xeosList)
                cmbxPhase.Items.Add(xeos);

            cmbxPhase.SelectedIndex = 0;
        }

        private void cmbxPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbxModelVariable.Items.Clear();
            tcEndmembers.ClearTable();
            tcEnergyParameters.ClearTable();
            for (int i = 0; i < cklbxOxides.Items.Count; i++)
                cklbxOxides.SetItemCheckState(i, CheckState.Unchecked);

            if (!(cmbxPhase.SelectedItem is XEOS xeos)) return;
            lblPhaseFullName.Text = EnumExtensions.GetDescriptionFromValue(xeos.PhaseType);
            var composition = DefaultPhaseCompositions.GetOxidesFromPhaseComposition(xeos.PhaseType);
            if (composition == null) return;
            for (int i = 0; i < cklbxOxides.Items.Count; i++)
                if (composition.Contains((OxideType)cklbxOxides.Items[i]))
                    cklbxOxides.SetItemCheckState(i, CheckState.Checked);

            foreach (var item in xeos.Variables.Where(v => !v.IsOrderVariable))
                cmbxModelVariable.Items.Add(item);

            cmbxModelVariable.SelectedIndex = 0;

            TabulateEndmembersAndAlphas(xeos);
            TabulateenergyParameters(xeos);
        }

        private void TabulateEndmembersAndAlphas(XEOS xeos)
        {
            if (xeos == null) return;
            tcEndmembers.ClearTable();

            var columns = new List<string>() { "endmember" };
            var hasAlphs = xeos.Alphas.Count > 0;
            if (hasAlphs)
                columns.AddRange(new List<string>() { "α_A", "α_B", "α_C" });

            var rows = new List<List<object>>();
            for (int i = 0; i < xeos.EndMembers.Count; i++)
            {
                var row = new List<object>() { xeos.EndMembers[i].Name };
                if (hasAlphs)
                {
                    var alpha = xeos.Alphas.FirstOrDefault(a => a.Endmember == xeos.EndMembers[i]);
                    if (alpha == null) continue;
                    row.AddRange(new List<object>() { alpha.A, alpha.B, alpha.C });
                }

                rows.Add(row);
            }

            tcEndmembers.FillTable(columns, rows);
        }

        private void TabulateenergyParameters(XEOS xeos)
        {
            if (xeos == null) return;
            tcEnergyParameters.ClearTable();

            var columns = new List<string>() { "Energy Parameter" };
            columns.AddRange(new List<string>() { "W_A", "W_B", "W_C" });
            var rows = new List<List<object>>();
            for (int i = 0; i < xeos.Ws.Count; i++)
            {
                var row = new List<object>() { xeos.Ws[i] };
                var Wij = xeos.Ws[i];
                if (Wij == null) continue;
                row.AddRange(new List<object>() { Wij.A, Wij.B, Wij.C });
                rows.Add(row);
            }

            tcEnergyParameters.FillTable(columns, rows);
        }

        private void cmbxModelVariable_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cklbxCations.Items.Count; i++)
                cklbxCations.SetItemCheckState(i, CheckState.Unchecked);

            if (!(cmbxModelVariable.SelectedItem is ModelVariable modelVariable)) return;
            var cations = DefaultModelDefinitions.GetCationsFromModelVariable(modelVariable.PhaseType, modelVariable.VariableType);
            if (cations == null) return;
            for (int i = 0; i < cklbxCations.Items.Count; i++)
                if (cations.Contains((CationType)cklbxCations.Items[i]))
                    cklbxCations.SetItemCheckState(i, CheckState.Checked);
        }
    }
}
