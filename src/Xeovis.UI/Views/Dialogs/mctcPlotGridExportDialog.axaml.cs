using Avalonia.Controls;
using Avalonia.Data;
using Xeovis.Presentation;
using Xeovis.UI.Models.Enums;

namespace Xeovis.UI;

public partial class mctcPlotGridExportDialog : Window
{
    private mctcPlotDto _mctcPlotDto = new mctcPlotDto();

    public string ImageFilesName;
    public string DataFilesName;
    public FileExtensionType ImageFileType;
    public FileExtensionType DataFileType;
    public bool IndiviualExport;
    public bool ExportImages;
    public bool ExportData;

    public mctcPlotGridExportDialog()
    {
        InitializeComponent();
        DataContext = this;

        ImageFilesName = "Plots";
        DataFilesName = "Tables";
    }

    public mctcPlotGridExportDialog(mctcPlotDto plotDto) : this()
    {
        _mctcPlotDto = plotDto;
        PrepareUI();
    }

    private void PrepareUI()
    {
        // Image-related
        ckbxExportPlots.ViewModel = new ViewModels.CheckBoxControlViewModel("Export Plots as Image")
        {
            ToolTip = "The overall layout of plots is preserved as shown in the main window, except for empty spaces."
        };

        txtImageFileName.ViewModel = new ViewModels.LabelInputControlViewModel("Image File Name", _mctcPlotDto.Name + " - Images");
        cmbxImageFileFormat.ViewModel = new ViewModels.LabelComboBoxControlViewModel()
        {
            Label = "Image File Format",
            Items = new System.Collections.ObjectModel.ObservableCollection<Models.ListableItem>()
            {
                new Models.ListableItem(FileExtensionType.JPEG),
                new Models.ListableItem(FileExtensionType.PNG)
            }
        };

        var bindingImages = new Binding
        {
            Source = ckbxExportPlots.ViewModel,
            Path = nameof(ckbxExportPlots.ViewModel.IsChecked),
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };
        txtImageFileName.Bind(IsEnabledProperty, bindingImages);
        cmbxImageFileFormat.Bind(IsEnabledProperty, bindingImages);

        // Data-related
        ckbxExportData.ViewModel = new ViewModels.CheckBoxControlViewModel("Export Data as Table");
        txtDataFileName.ViewModel = new ViewModels.LabelInputControlViewModel("Data File Name", _mctcPlotDto.Name + " - Data");
        cmbxDataFileFormat.ViewModel = new ViewModels.LabelComboBoxControlViewModel()
        {
            Label = "Data File Format",
            Items = new System.Collections.ObjectModel.ObservableCollection<Models.ListableItem>()
            {
                new Models.ListableItem(FileExtensionType.Txt),
            }
        };

        var bindingData = new Binding
        {
            Source = ckbxExportData.ViewModel,
            Path = nameof(ckbxExportData.ViewModel.IsChecked),
            Mode = BindingMode.OneWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
        };
        txtDataFileName.Bind(IsEnabledProperty, bindingData);
        cmbxDataFileFormat.Bind(IsEnabledProperty, bindingData);

        //
        ckbxExportSeparateFiles.ViewModel = new ViewModels.CheckBoxControlViewModel("Individual Exports")
        {
            ToolTip = "In addition to the main exports, individual files will be exported for each item."
        };
    }

    private void btnCancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        this.Close(DialogResultType.Cancel);
    }
    private void btnExport_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        IndiviualExport = ckbxExportSeparateFiles.IsChecked;
        ExportData = ckbxExportData.IsChecked;
        ExportImages = ckbxExportPlots.IsChecked;
        ImageFilesName = txtImageFileName.Value;
        DataFilesName = txtDataFileName.Value;
        ImageFileType = (FileExtensionType)cmbxImageFileFormat.ViewModel.SelectedItem.Tag;
        DataFileType = (FileExtensionType)cmbxDataFileFormat.ViewModel.SelectedItem.Tag;
        this.Close(DialogResultType.OK);
    }
}