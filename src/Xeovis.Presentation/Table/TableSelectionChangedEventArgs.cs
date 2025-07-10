namespace Xeovis.Presentation
{
    public class TableSelectionChangedEventArgs : EventArgs
    {
        public int TrueRowIndex { get; set; }
        public int VisualRowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public object? Value { get; set; }

        public TableSelectionChangedEventArgs(int rowIndex, int visualRowIndex, int columnIndex, object? value = null)
        {
            TrueRowIndex = rowIndex;
            VisualRowIndex = visualRowIndex;
            ColumnIndex = columnIndex;
            Value = value;
        }
    }
}
