using System;

namespace DatabaseReader.Events
{
    public class TableCellDoubleClickedEventArgs : EventArgs
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public object CellValue { get; set; }

        public TableCellDoubleClickedEventArgs()
        {
            
        }
    }
}
