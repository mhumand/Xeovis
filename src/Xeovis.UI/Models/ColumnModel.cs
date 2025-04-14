using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Xeovis.UI.Models
{
    public partial class ColumnModel : ObservableObject
    {
        [ObservableProperty]
        private string header;

        [ObservableProperty]
        private ObservableCollection<object> values;

        [ObservableProperty]
        private bool isReadOnly;

        [ObservableProperty]
        private bool isVisible;

        [ObservableProperty]
        private bool canSort;

        [ObservableProperty]
        private bool canReorder;

        [ObservableProperty]
        private bool canResize;
        private string v;
        private List<DateTime> dateTimes;

        public Guid Id { get; set; }

        public int DataCount => Values != null ? Values.Count : 0;

        public bool HasData => DataCount > 0;

        public Type ValueType => HasData ? Values[0].GetType() : typeof(string);

        public ColumnModel()
        {
            Id = Guid.NewGuid();
            Values = new ObservableCollection<object>();
            Header = "COLUMN";
            IsReadOnly = false;
            IsVisible = true;
            CanSort = true;
            canReorder = true;
            canResize = true;
        }

        public ColumnModel(string header, List<object> values) : this()
        {
            Header = header;
            Values = new ObservableCollection<object>(values);
        }

        public ColumnModel(string header, List<string> values) : this()
        {
            Header = header;
            Values = new ObservableCollection<object>(values);
        }

        public ColumnModel(string header, List<double> values) : this()
        {
            Header = header;
            Values = new ObservableCollection<object>(values.Cast<object>());
        }

        public ColumnModel(string header, List<int> values) : this()
        {
            Header = header;
            Values = new ObservableCollection<object>(values.Cast<object>());
        }

        public ColumnModel(string header, List<bool> values) : this()
        {
            Header = header;
            Values = new ObservableCollection<object>(values.Cast<object>());
        }

        public ColumnModel(string header, List<DateTime> values) : this()
        {
            Header = header;
            Values = new ObservableCollection<object>(values.Cast<object>());
        }

        public override string ToString()
        {
            return Header;
        }
    }
}
