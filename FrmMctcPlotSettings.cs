using System;
using System.Windows.Forms;

namespace DatabaseReader
{
    public partial class FrmMctcPlotSettings : Form
    {
        public MctcPlotSettings Settings;

        public FrmMctcPlotSettings(MctcPlotSettings settings)
        {
            Settings = settings;
            InitializeComponent();
            PrepareUI();
        }

        private void PrepareUI()
        {
            nudTransparency.Value = Settings.ColorTransparencyIn100;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Settings.ColorTransparencyIn100 = (int)nudTransparency.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }

    public class MctcPlotSettings
    {
        public int ColorTransparencyIn100 { get; set; }

        public MctcPlotSettings()
        {
            ColorTransparencyIn100 = 40;
        }
    }
}
