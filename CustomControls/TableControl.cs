using DatabaseReader.Events;
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

        public DataGridViewSelectionMode SelectionMode
        {
            get => dgvTable.SelectionMode;
            set => dgvTable.SelectionMode = value;
        }

        public bool MultiSelect
        {
            get => dgvTable.MultiSelect;
            set => dgvTable.MultiSelect = value;
        }

        public TableControl()
        {
            InitializeComponent();
        }

        public EventHandler<TableRowSelectedEventArgs> TableRowSelected;

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
            ClearSelection();
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
                        targetGrid.Columns[i].Width = (int)colWidth.Width + 10;
                    else
                        targetGrid.Columns[i].Width = (int)colHeaderWidth.Width + 15;
                }
            }
        }

        private void dgvTable_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTable.SelectionMode == DataGridViewSelectionMode.FullRowSelect
                && dgvTable.MultiSelect == false && dgvTable.SelectedRows.Count > 0)
            {
                if (TableRowSelected == null) return;

                var cell = dgvTable.SelectedRows[0].Cells[0].Value;
                int index = 0;
                for (int i = 0; i < Table.Rows.Count; i++)
                {
                    if (cell == Table.Rows[i][0].ToString())
                    {
                        index = i;
                        break;
                    }
                }
                var tableRow = Table.Rows[index];
                var row = new List<object>();
                var columns = new List<string>();
                foreach (DataColumn column in Table.Columns)
                {
                    columns.Add(column.ColumnName);
                    row.Add(tableRow[column]);
                }

                var eventArgs = new TableRowSelectedEventArgs(columns, row);
                TableRowSelected.Invoke(this, eventArgs);
            }
        }

        private void ClearSelection()
        {
            dgvTable.ClearSelection();
            TableRowSelected?.Invoke(this, null);
        }

        public void SelectRow(int rowIndex)
        {
            ClearSelection();

            if (rowIndex >= 0)
            {
                dgvTable.Sort(dgvTable.Columns[0], ListSortDirection.Ascending);
                dgvTable.Rows[rowIndex].Selected = true;
                dgvTable.FirstDisplayedScrollingRowIndex = rowIndex;
            }
        }
    }
}
