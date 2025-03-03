using DatabaseReader.Definitions;
using DatabaseReader.Definitions.Modelling;
using DatabaseReader.Definitions.Persistence;
using DatabaseReader.Definitions.xEOS;
using DatabaseReader.Enums;
using DatabaseReader.Enums.Modelling;
using DatabaseReader.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DatabaseReader
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
                    variable.Phase = xeos.PhaseType;
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
            var oxideList = new List<OxideType>() { OxideType.SiO2, OxideType.Al2O3, OxideType.FeO, OxideType.Fe2O3, OxideType.MgO, OxideType.K2O, OxideType.Na2O, OxideType.Cr2O3, OxideType.TiO2, OxideType.H2O };

            foreach (var item in oxideList)
                cklbOxides.Items.Add(item);

            foreach (var xeos in _xeosList)
                cmbxPhase.Items.Add(xeos);

            cmbxPhase.SelectedIndex = 0;
        }

        private void cmbxPhase_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < cklbOxides.Items.Count; i++)
                cklbOxides.SetItemCheckState(i, CheckState.Unchecked);

            if (!(cmbxPhase.SelectedItem is XEOS xeos)) return;
            lblPhaseFullName.Text = EnumExtensions.GetDescriptionFromValue(xeos.PhaseType);
            var composition = DefaultPhaseCompositions.GetOxidesFromPhaseComposition(xeos.PhaseType);
            if (composition == null) return;
            for (int i = 0; i < cklbOxides.Items.Count; i++)
                if (composition.Contains((OxideType)cklbOxides.Items[i]))
                    cklbOxides.SetItemCheckState(i, CheckState.Checked);
        }
    }
}
