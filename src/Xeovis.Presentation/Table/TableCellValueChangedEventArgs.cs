namespace Xeovis.Presentation
{
    public class TableCellValueChangedEventArgs : EventArgs
    {
        public int RowIndex { get; set; }
        public Guid ColumnId { get; set; }
        public object Value { get; set; }

        public TableCellValueChangedEventArgs(object value, int rowIndex, Guid columnId)
        {
            Value = value;
            RowIndex = rowIndex;
            ColumnId = columnId;
        }
    }
}
