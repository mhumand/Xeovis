using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Serilog;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.ViewModels.DialogsViewModels;
using Xeovis.UI.Views;
using Xeovis.UI.Views.Dialogs;
using Xeovis.UI.Views.Helpers;

namespace Xeovis.UI;

public partial class MainMenu : UserControl
{
    public MainMenu()
    {
        InitializeComponent();
        DataContext = this;
        SetMenuShortcuts();
        btnMctcCalibration.IsEnabled = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
    }

    private void SetMenuShortcuts()
    {
        var ctrlModifier = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? KeyModifiers.Meta : KeyModifiers.Control;

        btnNew.HotKey = new KeyGesture(Key.N, ctrlModifier);
        btnOpen.HotKey = new KeyGesture(Key.O, ctrlModifier);
        btnSave.HotKey = new KeyGesture(Key.S, ctrlModifier);
        btnSaveAs.HotKey = new KeyGesture(Key.S, ctrlModifier | KeyModifiers.Shift);
        btnQuit.HotKey = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? new KeyGesture(Key.F4, KeyModifiers.Alt)
            : new KeyGesture(Key.Q, KeyModifiers.Meta);

        SetInputGestureFromHotKey(btnOpen);
        SetInputGestureFromHotKey(btnNew);
        SetInputGestureFromHotKey(btnSave);
        SetInputGestureFromHotKey(btnSaveAs);
        SetInputGestureFromHotKey(btnQuit);
    }

    private void SetInputGestureFromHotKey(MenuItem menuItem)
    {
        menuItem.InputGesture = menuItem.HotKey;
    }

    // Project
    public async void File_New()
    {
        var goAhead = await UIServices.CheckAndRaiseUnsavedChangesMessage(askAnyWay: true);
        if (!goAhead) return;

        UIServices.LastFilePath = string.Empty;
        UIServices.InvokeProjectLoaded(this, new ProjectFileLoadedEventArgs(null));
        Log.Information("New project created.");
    }

    public async void File_Open()
    {
        var goAhead = await UIServices.CheckAndRaiseUnsavedChangesMessage();
        if (!goAhead) return;

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null)
        {
            Log.Error("Couldn't find TopLevel.");
            return;
        }

        var options = new FilePickerOpenOptions()
        {
            Title = "Save",
            AllowMultiple = false,
            FileTypeFilter = [FileExtentionHelper.GetFilePickerFileType(FileExtensionType.XVS)]
        };

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
        if (files == null || files.Count == 0) return;
        Log.Information("Opening project file...");
        var file = files[0];
        var path = Uri.UnescapeDataString(file.Path.AbsolutePath);
        UIServices.LoadProjectFile(path);
    }

    public async void File_Save()
    {
        await UIServices.Save(isSaveAs: false);
    }

    public async void File_SaveAs()
    {
        await UIServices.Save(isSaveAs: true);
    }

    public async Task File_Quit()
    {
        if (await UIServices.CheckAndRaiseUnsavedChangesMessage(askAnyWay: true))
            Environment.Exit(exitCode: 0);
    }

    // Data Manager
    public async void DataManager_LoadData()
    {
        await UIServices.LoadExperiments();
    }

    public void DataManager_Export()
    {
    }

    // MCTC
    public async void Mctc_LoadBundle()
    {
        await UIServices.LoadMctcBundle();
    }

    public void Mctc_NewPlot()
    {
        UIServices.LoadNewPlot(Guid.Empty);
    }

    public void Mctc_Calibration()
    {
        return;
        var viewModel = new mctcCalibrationViewModel();
        
        string exePath = "/Users/mohammadhumand/Library/CloudStorage/" +
                         "GoogleDrive-mohammadhumand@gmail.com/" +
                         "My Drive/PhD @ Unimelb/PhD/PhD - Project/" +
                         "Project - Calculations/set_up_fitting/TC RUN/tc360";
        viewModel.RunThermocalc(exePath);
        
        return;
        var dialog = new mctcCalibrationDialog() { ViewModel = viewModel };
        dialog.ShowDialog(UIServices.ParentWindow);
    }
    
    // Settings
    public async void Settings_PhaseNames()
    {
        var vm = new SettingsPhaseNamesViewModel();
        var dialog = new SettingsPhaseNamesDialog() { ViewModel = vm };
        var result = await dialog.ShowDialog<DialogResultType>(UIServices.ParentWindow);
    }

    public async void Settings_XeosVariables()
    {
        var vm = new SettingsXeosVariablesViewModel();
        var dialog = new SettingsXeosVariablesDialog() { ViewModel = vm };
        var result = await dialog.ShowDialog<DialogResultType>(UIServices.ParentWindow);
    }

    public async void Settings_PhaseCompositions()
    {
        var vm = new SettingsPhaseCompositionsViewModel();
        var dialog = new SettingsPhaseCompositionsDialog() { ViewModel = vm };
        var result = await dialog.ShowDialog<DialogResultType>(UIServices.ParentWindow);
    }

    public async void Settings_VariableCations()
    {
        var vm = new SettingsVariableCationsViewModel();
        var dialog = new SettingsVariableCationsDialog() { ViewModel = vm };
        var result = await dialog.ShowDialog<DialogResultType>(UIServices.ParentWindow);
    }

    // Tools
    public async void Tools_API()
    {
        var apiDialog = new ApiSettingsDialog();
        await apiDialog.ShowDialog(UIServices.ParentWindow);
    }
    
    // View
    public void View_ShowLeftPanel()
    {
        UIServices.ParentWindow.ToggleLeftPanelVisibility(itemShowLeftPanel.IsChecked);
        Log.Information(itemShowLeftPanel.IsChecked.ToString());
    }

    public void View_OpenLogFolder()
    {
        if (!Directory.Exists(Program.LogFolder)) return;
        DirectoryHelper.GoToDirectory(Program.LogFolder);
    }
    
    // Help
    public void Help()
    {
    }

    public void About()
    {
        var dialog = new AboutDialog();
        dialog.ShowDialog(UIServices.ParentWindow);
    }
}