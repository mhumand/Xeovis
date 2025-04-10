using Avalonia.Controls;
using Xeovis.UI.ViewModels;

namespace Xeovis.TestUI;

public partial class FrmLabelBrowseDirectoryControl : Window
{
    public FrmLabelBrowseDirectoryControl()
    {
        InitializeComponent();
        DataContext = this;

        PrepareUI();
    }

    private void PrepareUI()
    {
        var vm1 = new LabelBrowseDirectoryControlViewModel("Browse Files - Excel Format", UI.Models.Enums.FileExtensionType.Excel);
        lbccFileBrowseExcel.ViewModel = vm1;
        lbccFileBrowseExcel.TextInputChanged += LbccFileBrowseExcel_TextInputChanged;

        var vm2 = new LabelBrowseDirectoryControlViewModel("Browse Folder", isFolderBrowser: true);
        lbccFolderBrowse.ViewModel = vm2;
        lbccFolderBrowse.TextInputChanged += LbccFolderBrowse_TextInputChanged;
    }

    private void LbccFolderBrowse_TextInputChanged(object? sender, UI.Models.Events.TextInputChangedEventArgs e)
    {
        txtFolder.Text = e.Text;
    }

    private void LbccFileBrowseExcel_TextInputChanged(object? sender, UI.Models.Events.TextInputChangedEventArgs e)
    {
        txtFileExcel.Text = e.Text;
    }
}