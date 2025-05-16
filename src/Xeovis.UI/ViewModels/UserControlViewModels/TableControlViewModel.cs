using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using Xeovis.Presentation;

namespace Xeovis.UI.ViewModels
{
    public partial class TableControlViewModel : ViewModelBase
    {
        [ObservableProperty] private string name;
        public List<Dictionary<string, object>> Rows { get; set; }
        public TableModel TableModel { get; set; }
        public Guid Id { get; set; }

        public TableControlViewModel()
        {
            Id = Guid.NewGuid();
            Name = "TABLE";
            Rows = new List<Dictionary<string, object>>();
            TableModel = new TableModel();
        }

        public TableControlViewModel(TableModel tableModel) : this()
        {
            TableModel = tableModel;
            Name = tableModel.Name;

            Rows = new List<Dictionary<string, object>>();
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

                    if (col.ValueType == typeof(double))
                        row.Add(col.Id.ToString(), double.IsNaN((double)col.Values[i]) ? string.Empty : col.Values[i]);
                    else
                        row.Add(col.Id.ToString(), col.Values[i]);
                }

                Rows.Add(row);
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}