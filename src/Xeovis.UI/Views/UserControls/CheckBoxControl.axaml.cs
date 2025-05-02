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
            _viewModel = value ?? new CheckBoxControlViewModel();
            DataContext = _viewModel;
        }
    }

    public bool IsChecked => _viewModel.IsChecked;

    public event EventHandler<CheckStateChangedEventArgs>? CheckStateChanged;

    public CheckBoxControl()
    {
        InitializeComponent();
        Loaded += CheckBoxControl_Loaded;
        toolTip.IsVisible = false;
    }

    private void CheckBoxControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            toolTip.ToolTipText = _viewModel.ToolTip;
            toolTip.IsVisible = _viewModel.HasToolTip;
        }
    }

    private void checkBox_CheckChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CheckStateChanged?.Invoke(this, new CheckStateChangedEventArgs((bool)checkBox.IsChecked));
    }
}