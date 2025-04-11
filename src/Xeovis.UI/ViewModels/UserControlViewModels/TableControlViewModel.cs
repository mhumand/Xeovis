using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels
{
    public partial class TableControlViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private ObservableCollection<Dictionary<string, object>> rows;

        [ObservableProperty]
        private TableModel tableModel;

        public Guid Id { get; set; }

        public TableControlViewModel()
        {
            Id = Guid.NewGuid();
            Name = "TABLE";
            Rows = new ObservableCollection<Dictionary<string, object>>();
            TableModel = new TableModel();
        }

        public TableControlViewModel(TableModel tableModel) : this()
        {
            TableModel = tableModel;
            Name = tableModel.Name;

            rows = new ObservableCollection<Dictionary<string, object>>();
            for (int i = 0; i < tableModel.GetMaxRowCount(); i++)
            {
                var row = new Dictionary<string, object>();
                foreach (var col in tableModel.Columns)
                {
                    if (i >= col.DataCount)
                    {
                        if (col.ValueType == typeof(bool))
                            col.Values.Add(false);
                        else
                            col.Values.Add(string.Empty);
                    }

                    row.Add(col.Id.ToString(), col.Values[i]);
                }

                rows.Add(row);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
