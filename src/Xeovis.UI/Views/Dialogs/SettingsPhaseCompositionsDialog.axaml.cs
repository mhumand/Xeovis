using Avalonia.Controls;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI;

public partial class SettingsPhaseCompositionsDialog : Window
{
    private SettingsPhaseCompositionsViewModel _viewModel = new();
    public SettingsPhaseCompositionsViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new();
            DataContext = _viewModel;
            PrepareUI();
        }
    }

    public SettingsPhaseCompositionsDialog()
    {
        InitializeComponent();
    }

    private void PrepareUI()
    {
        var tableModel = _viewModel.GetProjectValuesTableModel();
        tableControl.PopulateTable(tableModel);
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }

    private async void btnSave_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _viewModel.SavePhaseOxides(tableControl.ViewModel.TableModel);
        this.Close(DialogResultType.OK);
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
}