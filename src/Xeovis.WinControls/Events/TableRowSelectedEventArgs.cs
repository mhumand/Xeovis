using System;
using System.Collections.Generic;

namespace Xeovis.WinControls
{
    public  class TableRowSelectedEventArgs : EventArgs
    {
        public List<string> Columns;
        public List<object> Row;

        public TableRowSelectedEventArgs(List<string> columns, List<object> row)
        {
            Columns = columns;
            Row = row;
        }
    }
}
