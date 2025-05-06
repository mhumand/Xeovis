using Xeovis.Persistence.Old;

namespace Xeovis.WinUI
{
    public partial class FrmMctcPlotSettings : Form
    {
        public mctcPlotSettings_Old Settings;

        public FrmMctcPlotSettings(mctcPlotSettings_Old settings)
        {
            Settings = settings;
            InitializeComponent();
            PrepareUI();
        }

        private void PrepareUI()
        {
            nudTransparency.Value = Settings.ColorTransparencyIn100;
            ckbxShowLegend.Checked = Settings.IsLegendVisible;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Settings.ColorTransparencyIn100 = (int)nudTransparency.Value;
            Settings.IsLegendVisible = ckbxShowLegend.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
