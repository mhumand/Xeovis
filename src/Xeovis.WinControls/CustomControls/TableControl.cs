using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Xeovis.WinControls
{
    public partial class TableControl : UserControl
    {
        private ContextMenuStrip _columnContextMenu;
        private DataColumn _idColumn;

        public DataColumn IDColumn { get { return _idColumn; } }
        public DataTable DataTable;
        public EventHandler<TableCellDoubleClickedEventArgs> TableCellDoubleClicked;

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

            var dgvType = dgvTable.GetType();
            PropertyInfo info = dgvType.GetProperty("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(dgvTable, true);
        }

        public EventHandler<TableRowSelectedEventArgs> TableRowSelected;

        public void FillTable(List<string> columns, List<List<object>> values)
        {
            dgvTable.DataSource = null;
            dgvTable.Columns.Clear();

            if (columns == null || columns.Count == 0) return;
            if (values == null || values.Count == 0) return;

            DataTable = new DataTable();
            int index = 0;
            foreach (var column in columns)
            {
                bool isDouble = values[0][index] is double;
                bool isBool = values[0][index] is bool;

                if (isDouble)
                {
                    var col = DataTable.Columns.Add(column, typeof(double));
                    col.ReadOnly = true;
                }
                else if (isBool)
                {
                    var col = DataTable.Columns.Add(column, typeof(bool));
                    col.ReadOnly = false;
                }
                else
                {
                    var col = DataTable.Columns.Add(column, typeof(string));
                    col.ReadOnly = true;
                }

                index++;
            }

            foreach (var objects in values)
            {
                var row = DataTable.NewRow();
                for (int i = 0; i < objects.Count; i++)
                    row[i] = objects[i];

                DataTable.Rows.Add(row);
            }

            _idColumn = DataTable.Columns.Add("ID", typeof(Guid));
            var idColIndex = DataTable.Columns.IndexOf(_idColumn);
            foreach (DataRow row in DataTable.Rows)
                row[_idColumn] = Guid.NewGuid();

            dgvTable.DataSource = DataTable;
            dgvTable.Columns[idColIndex].Visible = false;
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
                        targetGrid.Columns[i].Width = (int)colWidth.Width + 15;
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

                var id = (Guid)dgvTable.SelectedRows[0].Cells[_idColumn.ColumnName].Value;
                int index = 0;
                for (int i = 0; i < DataTable.Rows.Count; i++)
                {
                    if (id == (Guid)DataTable.Rows[i][_idColumn])
                    {
                        index = i;
                        break;
                    }
                }

                var tableRow = DataTable.Rows[index];
                var row = new List<object>();
                var columns = new List<string>();
                foreach (DataColumn column in DataTable.Columns)
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

        public void SelectRow(Guid rowID)
        {
            if (rowID == null) return;

            ClearSelection();

            int rowIndex = -1;
            for (int i = 0; i < DataTable.Rows.Count; i++)
                if ((Guid)dgvTable.Rows[i].Cells[_idColumn.ColumnName].Value == rowID)
                    rowIndex = i;

            if (rowIndex >= 0)
            {
                dgvTable.Rows[rowIndex].Selected = true;
                dgvTable.FirstDisplayedScrollingRowIndex = rowIndex;
            }
        }

        private void dgvTable_MouseClick(object sender, MouseEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null) return;
            if (e.Button == MouseButtons.Right)
            {
                var columnIndex = dgvTable.HitTest(e.X, e.Y).ColumnIndex;
                var focusedColumn = dgvTable.Columns[columnIndex];

                if (focusedColumn.ValueType == typeof(bool))
                {
                    _columnContextMenu = new ContextMenuStrip();
                    var selectAllMenuItem = new ToolStripMenuItem("Select All");
                    selectAllMenuItem.Click += (s, arg) =>
                    {
                        foreach (DataRow row in DataTable.Rows)
                            row[columnIndex] = true;
                    };
                    _columnContextMenu.Items.Add(selectAllMenuItem);

                    var deselectAllMenuItem = new ToolStripMenuItem("Deselect All");
                    deselectAllMenuItem.Click += (s, arg) =>
                    {
                        foreach (DataRow row in DataTable.Rows)
                            row[columnIndex] = false;
                    };
                    _columnContextMenu.Items.Add(deselectAllMenuItem);

                    _columnContextMenu.Show(dgvTable, e.Location);
                }
            }
        }

        private void dgvTable_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var args = new TableCellDoubleClickedEventArgs();
            args.RowIndex = e.RowIndex;
            args.ColumnIndex = e.ColumnIndex;
            args.CellValue = DataTable.Rows[e.RowIndex][e.ColumnIndex];
            TableCellDoubleClicked?.Invoke(this, args);
        }
    }
}
