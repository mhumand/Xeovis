using Avalonia.Controls;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI;

public partial class SettingsPhaseNamesDialog : Window
{
    private SettingsPhaseNamesViewModel _viewModel = new();
    public SettingsPhaseNamesViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new();
            DataContext = _viewModel;
            PrepareUI();
        }
    }

    public SettingsPhaseNamesDialog()
    {
        InitializeComponent();
    }

    private void PrepareUI()
    {
        var tableModel = _viewModel.GetProjectValuesTableModel();
        tableControl.PopulateTable(tableModel);
        tableControl.CellValueChanged += TableControl_CellValueChanged;
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }

    private async void btnSave_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _viewModel.SavePhaseNames(tableControl.ViewModel.TableModel);
        this.Close(DialogResultType.OK);
    }

    private void btnRemoveSelected_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        tableControl.DeleteSelectedRow();
        _viewModel.UpdateValidationRules(tableControl.ViewModel.TableModel);
    }

    private void btnAddNewRow_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        tableControl.AddRowToBottom();
    }

    private void btnXeovisDefaults_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var tableModel = _viewModel.GetXeovisDefaultsTableModel();
        tableControl.PopulateTable(tableModel);
    }

    private void btnProjectDefaults_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var tableModel = _viewModel.GetProjectValuesTableModel();
        tableControl.PopulateTable(tableModel);
    }

    private void TableControl_CellValueChanged(object? sender, Presentation.TableCellValueChangedEventArgs e)
    {
        _viewModel.UpdateValidationRules(tableControl.ViewModel.TableModel);
    }
}