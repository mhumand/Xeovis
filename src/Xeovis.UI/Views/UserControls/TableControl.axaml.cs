using Avalonia.Controls;
using Avalonia.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xeovis.UI.Models;
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
            _viewModel = value;
            if (_viewModel != null && _viewModel.TableModel != null)
                FillTable(_viewModel.TableModel);
        }
    }

    private TableModel _tableModel => _viewModel.TableModel;
    private ObservableCollection<Dictionary<string, object>> _rows => _viewModel.Rows;

    public TableControl()
    {
        InitializeComponent();
        dataGrid.AutoGenerateColumns = false;

        dataGrid.CellEditEnding += DataGrid_CellEditEnding;
        dataGrid.CellEditEnded += DataGrid_CellEditEnded;
        dataGrid.KeyDown += DataGrid_KeyDown;
    }

    public void ClearTable()
    {
        dataGrid.Columns.Clear();
        dataGrid.ItemsSource = null;
        _viewModel = null;
    }

    public void FillTable(TableModel tableModel)
    {
        dataGrid.Columns.Clear();
        dataGrid.ItemsSource = null;
        if (tableModel == null) return;

        _viewModel = new TableControlViewModel(tableModel);

        // Generate columns dynamically based on the keys in the first row
        foreach (var key in _rows[0].Keys)
        {
            var column = tableModel.Columns.FirstOrDefault(c => c.Id.ToString() == key);
            if (column == null || !column.IsVisible) continue;

            string header = column.Header;
            bool isReadOnly = column.IsReadOnly;
            bool canSort = column.CanSort;
            bool canReorder = column.CanReorder;
            bool canResize = column.CanResize;

            var sampleValue = _rows[0][key];
            if (sampleValue is bool)
            {
                // Checkbox column
                dataGrid.Columns.Add(new DataGridCheckBoxColumn
                {
                    Header = header,
                    Binding = new Binding($"[{key}]"),
                    IsReadOnly = isReadOnly,
                    IsThreeState = false,
                    CanUserSort = canSort,
                    CanUserReorder = canReorder,
                    CanUserResize = canResize,
                    Tag = column,
                });
            }
            else
            {
                // Default to text column (for strings, numbers, etc.)
                dataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = header,
                    Binding = new Binding($"[{key}]"),
                    IsReadOnly = isReadOnly,
                    CanUserSort = canSort,
                    CanUserReorder = canReorder,
                    CanUserResize = canResize,
                    Tag = column,
                });
            }
        }

        dataGrid.FrozenColumnCount = tableModel.FrozenColumnCount;
        dataGrid.MinColumnWidth = 70;
        dataGrid.ItemsSource = _rows;
        dataGrid.IsReadOnly = tableModel.IsReadOnly;
        dataGrid.CanUserResizeColumns = tableModel.CanResizeColumns;
        dataGrid.CanUserSortColumns = tableModel.CanSortColumns;
        dataGrid.CanUserReorderColumns = tableModel.CanReorderColumns;
        dataGrid.SelectionMode = tableModel.IsMultiSelect ? DataGridSelectionMode.Extended : DataGridSelectionMode.Single;
        dataGrid.ClipboardCopyMode = tableModel.CopyToClipboardWithHeaders ? DataGridClipboardCopyMode.IncludeHeader : DataGridClipboardCopyMode.ExcludeHeader;
    }


    private void DataGrid_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Escape)
            dataGrid.SelectedIndex = -1;
    }

    private void DataGrid_CellEditEnded(object? sender, DataGridCellEditEndedEventArgs e)
    {

    }

    private void DataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {

    }
}