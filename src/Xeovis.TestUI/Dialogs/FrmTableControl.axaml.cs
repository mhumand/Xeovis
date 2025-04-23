using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using Xeovis.UI.Models;

namespace Xeovis.TestUI;

public partial class FrmTableControl : Window
{
    public FrmTableControl()
    {
        InitializeComponent();
        DataContext = this;
    }

    private void btn1_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var tableModel = GetSampleTableModel();
        
        table1.PopulateTable(tableModel);

    }

    private void btn2_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var tableModel = GetSampleTableModel();
        tableModel.IsReadOnly = true;

        table2.PopulateTable(tableModel);
    }

    private TableModel GetSampleTableModel()
    {
        var col1 = new ColumnModel("Index", new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 });
        var col2 = new ColumnModel("Date", new List<DateTime>() { DateTime.Now, DateTime.Now.AddHours(1), DateTime.Now.AddDays(1) });
        var col3 = new ColumnModel("Visible", [true, false, true, true, null]);
        var col4 = new ColumnModel("Value", new List<double>() { 2e6, 1.56e-2, 1.564, 418.1, double.NaN, double.PositiveInfinity, 888.444 });

        return new TableModel("Sample Table", [col1, col2, col3, col4]);
    }
}