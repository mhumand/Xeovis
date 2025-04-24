using System;
using System.Collections.Generic;
using System.Linq;

namespace Xeovis.UI.Models
{
    public class TableModel
    {
        public string Name { get; set; }
        public List<ColumnModel> Columns { get; set; }
        public Guid Id { get; set; }
        public bool IsReadOnly { get; set; } = false;
        public bool CanResizeColumns { get; set; } = true;
        public bool CanSortColumns { get; set; } = true;
        public bool CanReorderColumns { get; set; } = true;
        public bool IsMultiSelect { get; set; } = true;
        public bool IsRowSelect { get; set; } = false;
        public bool CopyToClipboardWithHeaders { get; set; } = true;
        public int FrozenColumnCount { get; set; } = 0;

        public TableModel()
        {
            Id = Guid.NewGuid();
            Name = "TABLE";
            Columns = new List<ColumnModel>();
        }

        public TableModel(string name, List<ColumnModel> columns) : this()
        {
            Name = name;
            Columns = new List<ColumnModel>(columns);
        }

        public int GetMaxRowCount()
        {
            return Columns.Count == 0 ? 0 : Columns.Max(c => c.Values.Count);
        }

        public override string ToString()
        {
            return Name;
        }

        public ColumnModel GetColumnById(Guid id)
        {
            return Columns.FirstOrDefault(c => c.Id == id);
        }
    }
}
