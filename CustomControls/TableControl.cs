using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseReader.CustomControls
{
    public partial class TableControl : UserControl
    {
        public TableControl()
        {
            InitializeComponent();
        }

        public void FillTable(List<string> columns, List<List<object>> values)
        {
            dgvTable.DataSource = null;
            dgvTable.Columns.Clear();

            if (columns == null || columns.Count == 0) return;
            if (values == null || values.Count == 0) return;

            var dt = new DataTable();
            int index = 0;
            foreach (var column in columns)
            {
                bool isDouble = values[0][index] is double;
                dt.Columns.Add(column, isDouble ? typeof(double) : typeof(string));
                index++;
            }

            foreach (var objects in values)
            {
                var row = dt.NewRow();
                for (int i = 0; i < objects.Count; i++)
                    row[i] = objects[i];

                dt.Rows.Add(row);
            }

            dgvTable.DataSource = dt;
            dgvTable.ClearSelection();
        }

        public void ClearTable()
        {
            dgvTable.DataSource = null;
            dgvTable.Columns.Clear();
        }
    }
}
