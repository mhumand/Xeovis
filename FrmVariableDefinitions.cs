using DatabaseReader.Definitions;
using DatabaseReader.Definitions.xEOS;
using DatabaseReader.Enums;
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

        public List<XEOS> xeosList;

        public FrmVariableDefinitions()
        {
            InitializeComponent();
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
            }
        }

        private void ReadAxFile()
        {
            var sr = new StreamReader(_fileName);
            var content = sr.ReadToEnd();
            var lines = content.Split(new char[] { '\n' }).ToList();
            lines.RemoveAll(l => string.IsNullOrWhiteSpace(l));
            var allRange = Enumerable.Range(0, lines.Count);
            var verbatimIndexes = allRange.Where(i => lines[i] == "verbatim").ToList();
            verbatimIndexes.Add(allRange.Count() - 1);
            var splitters = new char[] { '(', ',', ')', ' ' };

            xeosList = new List<XEOS>();

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
                xeos.Phase = PhaseTypeExtensions.GetPhaseTypeByName(xeos.Name);

                foreach (var emLine in endmemberLines)
                {
                    var emParts = emLine.Split(splitters).ToList();
                    emParts.RemoveAll(l => string.IsNullOrWhiteSpace(l));
                    var endMember = new EndMember();
                    endMember.Name = emParts[1];
                    xeos.EndMembers.Add(endMember);
                }

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

                xeosList.Add(xeos);
                i++;
            }
        }
    }
}
