using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;
using Xeovis.UI.Views.Helpers;

namespace Xeovis.UI;

public partial class LabelBrowseDirectoryControl : UserControl
{
    private LabelBrowseDirectoryControlViewModel _viewModel = new LabelBrowseDirectoryControlViewModel();

    public LabelBrowseDirectoryControlViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new LabelBrowseDirectoryControlViewModel();
            DataContext = _viewModel;
        }
    }

    public event EventHandler<TextInputChangedEventArgs>? TextInputChanged;

    public LabelBrowseDirectoryControl()
    {
        InitializeComponent();
        Loaded += LabelBrowseDirectoryControl_Loaded;
        toolTip.IsVisible = false;
        btnRestore.IsVisible = false;
        tbInput.KeyDown += tbInput_KeyDown;
        tbInput.TextChanged += tbInput_TextChanged;
        txtBtnGrid.PointerEntered += txtBtnGrid_PointerEntered;
        txtBtnGrid.PointerExited += txtBtnGrid_PointerExited;
    }

    private void tbInput_TextChanged(object? sender, TextChangedEventArgs e)
    {
        TextInputChanged?.Invoke(this, new TextInputChangedEventArgs(tbInput.Text));
    }

    private void LabelBrowseDirectoryControl_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        toolTip.IsVisible = _viewModel.HasToolTip;
        toolTip.ToolTipText = _viewModel.ToolTip;
    }

    private async void btnBrowse_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_viewModel == null) return;
        var topLevel = TopLevel.GetTopLevel(this);

        if (_viewModel.IsFolderBrowser)
        {
            var options = new FolderPickerOpenOptions()
            {
                Title = "Choose Folder",
                AllowMultiple = false,
            };

            var folders = await topLevel.StorageProvider.OpenFolderPickerAsync(options);
            if (folders.Count > 0)
                _viewModel.Directory = Uri.UnescapeDataString(folders[0].Path.AbsolutePath);
        }
        else
        {
            var options = new FilePickerOpenOptions()
            {
                Title = "Choose File",
                AllowMultiple = false,
                FileTypeFilter = [FileExtentionHelper.GetFilePickerFileType(_viewModel.ExtensionType)]
            };

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
            if (files.Count > 0)
                _viewModel.Directory = Uri.UnescapeDataString(files[0].Path.AbsolutePath);
        }
    }
        
    private void btnRestore_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        tbInput.Text = _viewModel.OriginalDirectory;
    }

    private void txtBtnGrid_PointerExited(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        btnRestore.IsVisible = false;
    }

    private void txtBtnGrid_PointerEntered(object? sender, Avalonia.Input.PointerEventArgs e)
    {
        btnRestore.IsVisible = !_viewModel.IsReadOnly;
    }

    private void tbInput_KeyDown(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        if (e.Key == Avalonia.Input.Key.Escape || e.Key == Avalonia.Input.Key.Enter)
        {
            this.Focus();
        }
    }

    public string GetDirectory()
    {
        return _viewModel.Directory;
    }
}