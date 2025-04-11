using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xeovis.UI.Models
{
    public partial class TableModel : ObservableObject
    {
        [ObservableProperty]
        public string name;

        [ObservableProperty]
        public ObservableCollection<ColumnModel> columns;

        public Guid Id { get; set; }

        public bool IsReadOnly { get; set; } = false;
        public bool CanResizeColumns { get; set; } = true;
        public bool CanSortColumns { get; set; } = true;
        public bool CanReorderColumns { get; set; } = true;

        public bool IsMultiSelect {  get; set; } = true;
        public bool IsRowSelect {  get; set; } = false;
        public bool CopyToClipboardWithHeaders { get; set; } = true;

        public TableModel()
        {
            Id = Guid.NewGuid();
            Name = "TABLE";
            Columns = new ObservableCollection<ColumnModel>();
        }

        public TableModel(string name, List<ColumnModel> columns) : this()
        {
            Name = name;
            Columns = new ObservableCollection<ColumnModel>(columns);
        }

        public int GetMaxRowCount()
        {
            return Columns.Max(c => c.Values.Count);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
