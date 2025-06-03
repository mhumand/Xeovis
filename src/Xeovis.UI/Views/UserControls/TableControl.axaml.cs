using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using Xeovis.Base;
using Xeovis.Base.Comparers;
using Xeovis.Presentation;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class TableControl : UserControl
{
    private TableControlViewModel _viewModel = new TableControlViewModel();
    public TableControlViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new TableControlViewModel();
            DataContext = _viewModel;
            PopulateTable(_viewModel.TableModel);
        }
    }

    private TableModel _tableModel => _viewModel.TableModel;
    private List<Dictionary<string, object>> _rows => _viewModel.Rows;

    public event EventHandler<TableSelectionChangedEventArgs>? SelectionChanged;
    public event EventHandler<TableCellValueChangedEventArgs>? CellValueChanged;

    public TableControl()
    {
        InitializeComponent();
        dataGrid.AutoGenerateColumns = false;

        dataGrid.CellEditEnding += DataGrid_CellEditEnding;
        dataGrid.CellEditEnded += DataGrid_CellEditEnded;
        dataGrid.KeyDown += DataGrid_KeyDown;
        dataGrid.SelectionChanged += DataGrid_SelectionChanged;
    }

    public void ClearTable()
    {
        dataGrid.Columns.Clear();
        dataGrid.ItemsSource = null;
        _viewModel = null;
    }

    public bool PopulateTable(TableModel tableModel)
    {
        dataGrid.Columns.Clear();
        dataGrid.ItemsSource = null;
        if (tableModel == null || tableModel.GetMaxRowCount() == 0) return false;

        _viewModel = new TableControlViewModel(tableModel);

        // Generate columns dynamically based on the keys in the first row
        foreach (var key in _rows[0].Keys)
        {
            var column = _tableModel.Columns.FirstOrDefault(c => c.Id.ToString() == key);
            if (column == null || !column.IsVisible) continue;

            string header = column.Header;
            bool isReadOnly = column.IsReadOnly;
            bool canSort = column.CanSort;
            bool canReorder = column.CanReorder;
            bool canResize = column.CanResize;

            DataGridColumn gridColumn;
            var sampleValue = _rows[0][key];
            if (sampleValue is bool)
            {
                // Checkbox column
                gridColumn = new DataGridCheckBoxColumn
                {
                    Header = header,
                    Binding = new Binding($"[{key}]"),
                    IsReadOnly = isReadOnly,
                    IsThreeState = false,
                    CanUserSort = canSort,
                    CanUserReorder = canReorder,
                    CanUserResize = canResize,
                    Tag = column,
                };
            }
            else if (column.HasOptions)
            {
                // Combobox column
                gridColumn = new DataGridTemplateColumn
                {
                    Header = header,
                    IsReadOnly = isReadOnly,
                    CanUserSort = canSort,
                    CanUserReorder = canReorder,
                    CanUserResize = canResize,
                    Tag = column,

                    CellTemplate = new FuncDataTemplate<object>((item, _) =>
                    {
                        var optionsComboBox = new ComboBox
                        {
                            ItemsSource = column.Options,
                            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                            [!ComboBox.SelectedItemProperty] = new Binding($"[{key}]")
                        };
                        optionsComboBox.Tag = column.Id;
                        optionsComboBox.SelectionChanged += optionsComboBox_SelectionChanged;
                        optionsComboBox.IsEnabled = !isReadOnly && !_tableModel.IsReadOnly;
                        optionsComboBox.BorderBrush = Brushes.Transparent;
                        optionsComboBox.Background = Brushes.Transparent;
                        optionsComboBox.AddHandler(ComboBox.PointerPressedEvent, optionsComboBox_PointerPressed, RoutingStrategies.Tunnel);
                        return optionsComboBox;
                    }),

                    //CellTemplate = new FuncDataTemplate<object>((item, _) =>
                    //{
                    //    return new TextBlock
                    //    {
                    //        Padding = new Avalonia.Thickness(10, 0),
                    //        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    //        [!TextBlock.TextProperty] = new Binding($"[{key}]")
                    //    };
                    //}),

                    //CellEditingTemplate = new FuncDataTemplate<object>((item, _) =>
                    //{
                    //    var optionsComboBox = new ComboBox
                    //    {
                    //        ItemsSource = column.Options,
                    //        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                    //        [!ComboBox.SelectedItemProperty] = new Binding($"[{key}]")
                    //    };
                    //    optionsComboBox.Tag = column.Id;
                    //    optionsComboBox.SelectionChanged += optionsComboBox_SelectionChanged;
                    //    return optionsComboBox;
                    //})
                };
            }
            else
            {
                // Default to text column (for strings, numbers, etc.)
                gridColumn = new DataGridTextColumn
                {
                    Header = header,
                    Binding = new Binding($"[{key}]"),
                    IsReadOnly = isReadOnly,
                    CanUserSort = canSort,
                    CanUserReorder = canReorder,
                    CanUserResize = canResize,
                    Tag = column,
                };

                if (column.ValueType == typeof(double) || column.ValueType == typeof(int))
                    gridColumn.CustomSortComparer = new NumericDictionaryComparer(column.Id.ToString());
            }

            gridColumn.HeaderPointerPressed += GridColumn_HeaderPointerPressed;
            dataGrid.Columns.Add(gridColumn);
        }

        SetDataGridProperties();
        return true;
    }

    private void optionsComboBox_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not ComboBox cmbox) return;
        var point = e.GetCurrentPoint(cmbox);
        if (!point.Properties.IsLeftButtonPressed)
            e.Handled = true;
    }

    private void optionsComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var cmbx = sender as ComboBox;
        if (cmbx.IsDropDownOpen && e.AddedItems.Count > 0)
        {
            var column = _tableModel.GetColumnById((Guid)cmbx.Tag);
            var trueIndex = dataGrid.ItemsSource.Cast<object>().ToList().IndexOf(dataGrid.SelectedItem);
            var value = e.AddedItems[0];
            column.Values[trueIndex] = value;
            CellValueChanged?.Invoke(this, new TableCellValueChangedEventArgs(value, trueIndex, column.Id));
        }
    }

    private void SetDataGridProperties()
    {
        dataGrid.FrozenColumnCount = _tableModel.FrozenColumnCount;
        //dataGrid.MinColumnWidth = 70;
        dataGrid.ItemsSource = _rows;
        dataGrid.IsReadOnly = _tableModel.IsReadOnly;
        dataGrid.CanUserResizeColumns = _tableModel.CanResizeColumns;
        dataGrid.CanUserSortColumns = _tableModel.CanSortColumns;
        dataGrid.CanUserReorderColumns = _tableModel.CanReorderColumns;
        dataGrid.SelectionMode =
            _tableModel.IsMultiSelect ? DataGridSelectionMode.Extended : DataGridSelectionMode.Single;
        dataGrid.ClipboardCopyMode = _tableModel.CopyToClipboardWithHeaders
            ? DataGridClipboardCopyMode.IncludeHeader
            : DataGridClipboardCopyMode.ExcludeHeader;
    }

    private void DataGrid_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Escape)
        {
            ClearSelection();
        }
    }

    private void ClearSelection()
    {
        dataGrid.SelectedIndex = -1;
        SelectionChanged?.Invoke(this, new TableSelectionChangedEventArgs(-1, -1));
    }

    private void DataGrid_CellEditEnded(object? sender, DataGridCellEditEndedEventArgs e)
    {
        //if (e.Column.Tag is not ColumnModel columnModel) return;
        //var rowIndex = dataGrid.ItemsSource.Cast<object>().ToList().IndexOf(dataGrid.SelectedItem);

        //var row = e.Row.DataContext as Dictionary<string, object>;
        //var column = dataGrid.Columns.IndexOf(e.Column);
        //var value = row.ToList()[column].Value.ToString();

        //columnModel.Values[rowIndex] = value;
    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        if (e.Column.Tag is not ColumnModel columnModel) return;
        var rowIndex = dataGrid.ItemsSource.Cast<object>().ToList().IndexOf(dataGrid.SelectedItem);

        var row = e.Row.DataContext as Dictionary<string, object>;
        var column = dataGrid.Columns.IndexOf(e.Column);
        var value = row.ToList()[column].Value.ToString();

        foreach (var rule in columnModel.ValidationRules)
        {
            if (!rule.Validate(value))
            {
                e.Cancel = true;
                var cell = e.EditingElement as TextBox;

                var popup = new Popup
                {
                    PlacementTarget = cell,
                    Placement = PlacementMode.Bottom,
                    IsLightDismissEnabled = true,
                    Child = new Border
                    {
                        Background = Brushes.AntiqueWhite,
                        BorderBrush = Brushes.Red,
                        BorderThickness = new Thickness(0, 0, 0, 2),
                        CornerRadius = new CornerRadius(2),
                        Padding = new Thickness(8),
                        Child = new TextBlock { Text = rule.ErrorMessage }
                    }
                };

                popup.IsOpen = true;
            }
        }

        if (!e.Cancel)
        {
            if (columnModel.ValueType == typeof(double))
                columnModel.Values[rowIndex] = DoubleExtensions.GetDoubleValueFromText(value);
            else
                columnModel.Values[rowIndex] = value;
        }
    }

    private void DataGrid_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var rowIndex = dataGrid.SelectedIndex;
        SelectionChanged?.Invoke(this,
            new TableSelectionChangedEventArgs(10, 0, e.AddedItems.Count > 0 ? e.AddedItems[0] : null));
    }

    private void GridColumn_HeaderPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not DataGridColumn gridColumn) return;
        var point = e.GetCurrentPoint(dataGrid);
        dataGrid.SelectedIndex = -1;
        if (!point.Properties.IsRightButtonPressed) return;

        var menu = new ContextMenu();
        var sortItem = new MenuItem() { Header = "Sort" };
        sortItem.Click += (s, e) => gridColumn.Sort();
        menu.Items.Add(sortItem);

        var clearSortItem = new MenuItem() { Header = "Clear Sorting" };
        clearSortItem.Click += (s, e) => gridColumn.ClearSort();
        menu.Items.Add(clearSortItem);

        dataGrid.ContextMenu = menu;
        menu.Closing += ColumnHeaderMenu_Closing;
        menu.Open(dataGrid);
    }

    private void ColumnHeaderMenu_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        dataGrid.ContextMenu = null;
    }

    public void SelectRow(int rowIndex)
    {
        Dispatcher.UIThread.Post(() =>
        {
            dataGrid.SelectionChanged -= DataGrid_SelectionChanged;
            dataGrid.SelectedIndex = rowIndex;
            if (rowIndex >= 0)
                dataGrid.ScrollIntoView(dataGrid.SelectedItem, dataGrid.Columns[0]);
            dataGrid.SelectionChanged += DataGrid_SelectionChanged;
        });
    }

    public void SelectRowContainingCellString(string cellString, int columnIndex)
    {
        var rows = _rows.Select(x => x.Values).ToList();
        var rowIndex = rows.FindIndex(x => x.ToList()[columnIndex].ToString().Contains(cellString));
        SelectRow(rowIndex);
    }
}