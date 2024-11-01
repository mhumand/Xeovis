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
        public DataTable Table;

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

            Table = new DataTable();
            int index = 0;
            foreach (var column in columns)
            {
                bool isDouble = values[0][index] is double;
                Table.Columns.Add(column, isDouble ? typeof(double) : typeof(string));
                index++;
            }

            foreach (var objects in values)
            {
                var row = Table.NewRow();
                for (int i = 0; i < objects.Count; i++)
                    row[i] = objects[i];

                Table.Rows.Add(row);
            }

            dgvTable.DataSource = Table;
            dgvTable.ClearSelection();
            FastAutoSizeColumns();
        }

        public void ClearTable()
        {
            dgvTable.DataSource = null;
            dgvTable.Columns.Clear();
        }

        public void FastAutoSizeColumns()
        {
            DataGridView targetGrid = dgvTable;

            var gridTable = (DataTable)targetGrid.DataSource;
            using (var gfx = targetGrid.CreateGraphics())
            {
                for (int i = 0; i < gridTable.Columns.Count; i++)
                {
                    string[] colStringCollection = gridTable.AsEnumerable().Where(r => r.Field<object>(i) != null).Select(r => r.Field<object>(i).ToString()).ToArray();

                    colStringCollection = colStringCollection.OrderBy((x) => x.Length).ToArray();
                    if (colStringCollection.Length == 0) continue;
                    string longestColString = colStringCollection.Last();
                    var colWidth = gfx.MeasureString(longestColString, targetGrid.Font);
                    var colHeaderWidth = gfx.MeasureString(gridTable.Columns[i].ToString(), targetGrid.Font);

                    if (colWidth.Width > colHeaderWidth.Width)
                    {
                        targetGrid.Columns[i].Width = (int)colWidth.Width + 10;
                    }
                    else
                    {
                        targetGrid.Columns[i].Width = (int)colHeaderWidth.Width + 15;
                    }
                }
            }
        }
    }
}
