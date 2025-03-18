using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Xeovis.WinUI
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnDataManager_Click(object sender, EventArgs e)
        {
            btnDataManager.Enabled = false;
            var frm = new FrmDataManager();
            frm.FormClosed += (s, args) => btnDataManager.Enabled = true;
            frm.Show();

        }

        private void btnReadMctc_Click(object sender, EventArgs e)
        {
            btnReadMctc.Enabled = false;
            var frm = new FrmReadMctc();
            frm.FormClosed += (s, args) => btnReadMctc.Enabled = true;
            frm.Show();
        }
    }
}
