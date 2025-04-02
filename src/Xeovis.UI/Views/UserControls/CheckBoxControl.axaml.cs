using Avalonia.Controls;
using System;
using Xeovis.UI.Events;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class CheckBoxControl : UserControl
{
    private CheckBoxControlViewModel _viewModel = new CheckBoxControlViewModel();

    public event EventHandler<CheckStateChangedEventArgs> CheckStateChanged;

    public CheckBoxControl()
    {
        InitializeComponent();
    }

    public void SetDataContext(CheckBoxControlViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = viewModel;
    }

    private void checkBox_CheckChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CheckStateChanged?.Invoke(sender, new CheckStateChangedEventArgs((bool)checkBox.IsChecked));
    }
}