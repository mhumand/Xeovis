using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseReader
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnDataManager_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new FrmDataManager();
            frm.FormClosed += (s, args) => this.Close();
            frm.Show();

        }

        private void btnReadMctc_Click(object sender, EventArgs e)
        {
            Hide();
            var frm = new FrmReadMctc();
            frm.FormClosed += (s, args) => this.Close();
            frm.Show();
        }
    }
}
