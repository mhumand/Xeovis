using System.IO;
using System.Linq;
using System.Text;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Serilog;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels;
using Xeovis.UI.ViewModels.DialogsViewModels;
using Xeovis.UI.Views.Helpers;

namespace Xeovis.UI.Views.Dialogs;

public partial class ChartDataDialog : Window
{
    private ChartDataViewModel _viewModel = new ChartDataViewModel();

    public ChartDataViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value ?? new ChartDataViewModel();
            PrepareUI();
            DataContext = _viewModel;
        }
    }
    
    public ChartDataDialog()
    {
        InitializeComponent();
        Log.Information("Dialog opened.");
        // if (this.Content is Control content)
        //     content.DataContext = this.DataContext;
    }

    private void PrepareUI()
    {
        if (_viewModel == null) return;
        _viewModel.MakeTableModel();
        tcData.PopulateTable(_viewModel.TableModel);
        tcData.ContextRequested += tcDataOnContextRequested;
        
        var vmIncludeHeader = new CheckBoxControlViewModel("Copy/Export with column headers",  isChecked: true);
        ckbxIncludeHeader.ViewModel = vmIncludeHeader;
        ckbxIncludeHeader.CheckStateChanged += ckbxIncludeHeaderOnCheckStateChanged;
    }

    private void ckbxIncludeHeaderOnCheckStateChanged(object? sender, CheckStateChangedEventArgs e)
    {
        _viewModel.TableModel.CopyToClipboardWithHeaders = e.IsChecked;
        tcData.UpdateCopyClipboardMode(_viewModel.TableModel.CopyToClipboardWithHeaders);
    }

    private void tcDataOnContextRequested(object? sender, ContextRequestedEventArgs e)
    {
        // constructing menu
        var menu = new ContextMenu();
        
        var copyTableItem = new MenuItem() { Header = $"Copy Table" };
        copyTableItem.Click += (_, __) => CopyTableToClipboard();
        menu.Items.Add(copyTableItem);
        
        var exportAsTxtItem = new MenuItem() { Header = $"Export as Text" };
        exportAsTxtItem.Click += (s, e) => SaveDataAsTxt();
        menu.Items.Add(exportAsTxtItem);
        menu.Open(tcData);
    }

    private async void SaveDataAsTxt()
    {
        var options = new FilePickerSaveOptions();
        options.Title = "Save Text File";
        options.FileTypeChoices = [FileExtentionHelper.GetFilePickerFileType(Models.Enums.FileExtensionType.Txt)];
        var file = await TopLevel.GetTopLevel(this).StorageProvider.SaveFilePickerAsync(options);

        if (file != null)
        {
            using var stream = await file.OpenWriteAsync();
            using var streamWriter = new StreamWriter(stream);
            await streamWriter.WriteLineAsync(GetDataString());
        }
    }
    
    public string GetDataString()
    {
        var table = _viewModel.TableModel;
        int maxNameLength = table.Columns[0].Values.Cast<string>().Max(x => x.Length) + 4;
        var xHeader = table.Columns[1].Header;
        var yHeader = table.Columns[2].Header;
        int xLabelLength = xHeader.Length;
        var sb = new StringBuilder();

        if (_viewModel.TableModel.CopyToClipboardWithHeaders)
        {
            sb.AppendLine($"{_viewModel.Title}");
            sb.AppendLine($"{"Name".PadRight(maxNameLength)}{xHeader.PadRight(xLabelLength + 4)}{yHeader}");
        }

        for (int i = 0; i < table.GetMaxRowCount(); i++)
        {
            var name = table.Columns[0].Values[i].ToString();
            var xVal = table.Columns[1].Values[i].ToString();
            var yVal = table.Columns[2].Values[i].ToString();
            sb.AppendLine(($"{name.PadRight(maxNameLength)}{xVal.PadRight(xLabelLength + 4)}{yVal}"));
        }
        
        return sb.ToString();
    }

    private void btnClose_OnClick(object? sender, RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }

    private void CopyTableToClipboard()
    {
        TopLevel.GetTopLevel(this)?.Clipboard?.SetTextAsync(GetDataString());
    }
    
    private void btnExport_OnClick(object? sender, RoutedEventArgs e)
    {
        SaveDataAsTxt();
    }

    private void btnCopy_OnClick(object? sender, RoutedEventArgs e)
    {
        CopyTableToClipboard(); 
    }
}