using Avalonia.Controls;
using System;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class CheckBoxControl : UserControl
{
    private CheckBoxControlViewModel _viewModel = new CheckBoxControlViewModel();

    public CheckBoxControlViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value;
            DataContext = value;
        }
    }

    public event EventHandler<CheckStateChangedEventArgs> CheckStateChanged;

    public CheckBoxControl()
    {
        InitializeComponent();
    }

    private void checkBox_CheckChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CheckStateChanged?.Invoke(sender, new CheckStateChangedEventArgs((bool)checkBox.IsChecked));
    }
}