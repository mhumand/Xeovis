using Avalonia.Controls;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI;

public partial class SettingsXeosVariablesDialog : Window
{
    private SettingsXeosVariablesViewModel _viewModel = new();
    public SettingsXeosVariablesViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new();
            DataContext = _viewModel;
            PrepareUI();
        }
    }

    public SettingsXeosVariablesDialog()
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
        _viewModel.SaveXeosVariables(tableControl.ViewModel.TableModel);
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