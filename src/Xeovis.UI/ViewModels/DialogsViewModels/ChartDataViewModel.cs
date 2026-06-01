using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel.__Internals;
using Xeovis.Presentation;

namespace Xeovis.UI.ViewModels.DialogsViewModels;

public partial class ChartDataViewModel : ViewModelBase
{
    private ChartModel _chartModel;
    
    [ObservableProperty]
    private TableModel tableModel;

    [ObservableProperty]
    private string title;
    
    public Guid Id { get; set; }
    
    public ChartDataViewModel()
    {
        Id = Guid.NewGuid();
        _chartModel = new ChartModel();
        tableModel = new TableModel();
        Title = string.Empty;
    }

    public ChartDataViewModel(ChartModel chartModel) : this()
    {
        _chartModel = chartModel;
        Title = chartModel.Title;
    }

    public void MakeTableModel()
    {
        var colName = new ColumnModel() { Header = "Name" };
        var colXvals =  new ColumnModel() { Header = _chartModel.XAxis.Label };
        var colYvals = new ColumnModel() { Header = _chartModel.YAxis.Label };

        foreach (var curve in _chartModel.Curves.Where(x => x.IsVisibleInLegend))
        {
            colXvals.Values.AddRange(curve.Xvals.Cast<object>());
            colYvals.Values.AddRange(curve.Yvals.Cast<object>());
            colName.Values.AddRange(Enumerable.Repeat<object>(curve.Name, curve.DataCount));
        }

        tableModel = new TableModel() { Name = "Chart Data" };
        tableModel.Columns.Add(colName);
        tableModel.Columns.Add(colXvals);
        tableModel.Columns.Add(colYvals);
        tableModel.IsReadOnly = true;
        tableModel.IsMultiSelect = true;
    }
}