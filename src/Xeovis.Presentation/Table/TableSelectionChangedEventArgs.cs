namespace Xeovis.Presentation
{
    public class TableSelectionChangedEventArgs : EventArgs
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public object? Value { get; set; }

        public TableSelectionChangedEventArgs(int rowIndex, int columnIndex, object? value = null)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            Value = value;
        }
    }
}
