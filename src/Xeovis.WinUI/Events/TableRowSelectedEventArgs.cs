using System;
using System.Collections.Generic;

namespace DatabaseReader.Events
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
