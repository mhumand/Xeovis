using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Serilog;
using Xeovis.Base.Utilities;
using Xeovis.Domain;
using Xeovis.Persistence;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.Models.FileReaders;
using Xeovis.UI.ViewModels.DialogsViewModels;
using Xeovis.UI.Views;
using Xeovis.UI.Views.Helpers;
using Xeovis.Domain.Definitions;
using Xeovis.Domain.Definitions.Globals;

namespace Xeovis.UI;

public partial class MainMenu : UserControl
{
    public event EventHandler<ExperimentsLoadedEventArgs>? ExperimentsLoaded;
    public event EventHandler<mctcBundleLoadedEventArgs>? mctcBundleLoaded;
    public event EventHandler<ProjectFileLoadedEventArgs>? ProjectLoaded;

    public MainWindow? ParentWindow;

    private string _lastFilePath;

    public MainMenu()
    {
        InitializeComponent();
        DataContext = this;
        _lastFilePath = string.Empty;
        SetMenuShortcuts();
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
        var goAhead = await CheckAndRaiseUnsavedChangesMessage(askAnyWay: true);
        if (!goAhead) return;

        _lastFilePath = string.Empty;
        ProjectLoaded?.Invoke(this, new ProjectFileLoadedEventArgs(null));
        Log.Information("New project created.");
    }

    public async void File_Open()
    {
        var goAhead = await CheckAndRaiseUnsavedChangesMessage();
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
        var path = Uri.UnescapeDataString(files[0].Path.AbsolutePath);
        var projectFile = ProjectFileManager.Load(path);
        if (projectFile == null)
        {
            var ms = MessageBoxManager.GetMessageBoxStandard("Error",
                "Failed to open the project file!", ButtonEnum.Ok, Icon.Error);
            ms.ShowWindowDialogAsync(ParentWindow);
            return;
        }

        _lastFilePath = path;
        ProjectLoaded?.Invoke(this, new ProjectFileLoadedEventArgs(projectFile, path));
        Log.Information("Project file opened successfully.");
    }

    private async Task<bool> CheckAndRaiseUnsavedChangesMessage(bool askAnyWay = false)
    {
        if ((File.Exists(_lastFilePath) || askAnyWay) && ParentWindow != null)
        {
            var mb = MessageBoxManager.GetMessageBoxStandard("Unsaved Changes",
                "You have unsaved changes.\nDo you want to save before continuing?", ButtonEnum.YesNoCancel,
                Icon.Warning);
            var result = await mb.ShowWindowDialogAsync(ParentWindow);
            if (result == ButtonResult.Yes)
                await Save(isSaveAs: false);
            else if (result == ButtonResult.No)
                Log.Information("Project file not saved.");
            else
                return false;
        }

        return true;
    }

    public async void File_Save()
    {
        await Save(isSaveAs: false);
    }

    private async Task Save(bool isSaveAs = false)
    {
        var projectFile = GetProjectFile();
        if (projectFile == null) return;

        string path;
        if (isSaveAs || !File.Exists(_lastFilePath))
        {
            var topLevel = TopLevel.GetTopLevel(this);
            if (topLevel == null) return;
            var options = new FilePickerSaveOptions()
            {
                Title = "Save",
                ShowOverwritePrompt = true,
                FileTypeChoices = [FileExtentionHelper.GetFilePickerFileType(FileExtensionType.XVS)]
            };

            var files = await topLevel.StorageProvider.SaveFilePickerAsync(options);
            if (files == null) return;
            path = Uri.UnescapeDataString(files.Path.AbsolutePath);
        }
        else
            path = _lastFilePath;

        projectFile.ProjectName = Path.GetFileNameWithoutExtension(path);
        var result = ProjectFileManager.Save(projectFile, path);
        if (result)
        {
            ParentWindow.Title = $"Xeovis - {projectFile.ProjectName}";
            Log.Information("Project file saved.");
        }
    }

    private ProjectFile GetProjectFile()
    {
        if (ParentWindow == null) return null;
        var projectFile = new ProjectFile();
        projectFile.Experiments = ParentWindow.ViewModel.Experiments;
        projectFile.mctcBundles = ParentWindow.ViewModel.mctcBundles;
        projectFile.mctcPlotDtos = ParentWindow.ViewModel.mctcPlotDtos;
        projectFile.ProjectSettings.AllowedPhaseNames = PhaseNameList.PhaseNames;
        projectFile.ProjectSettings.PhaseXeosVariables = xeosVariableNameList.VariableNames;
        projectFile.ProjectSettings.PhaseOxides = PhaseOxideList.OxideList;
        projectFile.ProjectSettings.xeosVariableCations = xeosVariableCationList.CationList;
        return projectFile;
    }

    public async void File_SaveAs()
    {
        await Save(isSaveAs: true);
    }

    public void File_Quit()
    {
        Environment.Exit(0);
    }

    // Data Manager
    public async void DataManager_LoadData()
    {
        if (ParentWindow == null) return;
        var viewModel = new DataManagerLoadDataViewModel();
        var dialog = new DataManagerLoadDataDialog();
        dialog.ViewModel = viewModel;
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            var progressDialog = new ProgressBarDialog();
            progressDialog.ShowDialog(ParentWindow);

            var progress = progressDialog.ProgressReporter;
            var token = progressDialog.Token;

            try
            {
                var vm = dialog.ViewModel;
                var expReader = new ExperimentReader(vm.SelectedFolders, vm.SubfolderDirectory, vm.IsSubfolderChecked);
                var expReaderResults = await Task.Run(() => expReader.GetExperiments(progress, token));
                
                // get confirmation
                
                var experiments = expReaderResults.Experiments;
                ExperimentsLoaded?.Invoke(this, new ExperimentsLoadedEventArgs(experiments, dialog.ViewModel));
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                    Log.Information("User cancelled loading experiments.");
                else if (ex is IOException)
                {
                    Log.Information(ex.Message);
                    var mb = MessageBoxManager.GetMessageBoxStandard("Failed to Access",
                        StringUtilities.WrapTextToLines(ex.Message, 100), ButtonEnum.Ok, Icon.Error);
                    mb.ShowWindowDialogAsync(ParentWindow);
                }
                else
                    Log.Error(ex.Message);
            }
            finally
            {
                progressDialog.Close();
            }
        }
    }

    public void DataManager_Export()
    {
    }

    // MCTC Reader
    public async void Mctc_LoadBundle()
    {
        if (ParentWindow == null) return;
        var viewModel = new mctcLoadDataViewModel();
        viewModel.ExisitingBundleNames = ParentWindow.ViewModel.mctcBundles.Select(x => x.Name).ToList();
        var dialog = new mctcLoadDataDialog() { ViewModel = viewModel };

        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            var vm = dialog.ViewModel;

            var mctcBundle = await LoadMctcBundle(vm.FolderDirectory);
            if (mctcBundle != null)
            {
                mctcBundle.Name = vm.MctcBundleName;
                mctcBundleLoaded?.Invoke(this, new mctcBundleLoadedEventArgs(mctcBundle));
            }
        }
    }

    public async Task<mctcBundle> LoadMctcBundle(string folderDirectory)
    {
        if (ParentWindow == null) return null;
        var progressDialog = new ProgressBarDialog();
        progressDialog.ShowDialog(ParentWindow);

        var progress = progressDialog.ProgressReporter;
        var token = progressDialog.Token;

        try
        {
            var mctcReader = new mctcDataReader(folderDirectory);
            var mctcBundle = await Task.Run(() => mctcReader.ReadData(progress, token)) ??
                             throw new NullReferenceException();
            mctcBundle.TimeLoaded = DateTime.Now;
            mctcBundle.FilesDirectory = folderDirectory;
            return mctcBundle;
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case OperationCanceledException:
                    Log.Information("User cancelled loading mctc bundle.");
                    break;
                case FileNotFoundException:
                    Log.Error("Couldn't read files from {ExMessage}", ex.Message);
                    break;
                case NullReferenceException:
                    Log.Error("Couldn't load mctc bundle.");
                    break;
                default:
                    Log.Error(ex.Message);
                    break;
            }

            return null;
        }
        finally
        {
            progressDialog.Close();
        }
    }

    public void Mctc_NewPlot()
    {
        LoadNewPlotDialog(Guid.Empty);
    }

    public async void LoadNewPlotDialog(Guid selectedBundleId)
    {
        if (ParentWindow == null) return;
        var hasMctcBundle = ParentWindow.ViewModel.mctcBundles.Count > 0;
        if (!hasMctcBundle)
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard("Error",
                "No mctc bundle was found! Please load a bundle first.", ButtonEnum.Ok, Icon.Error);
            msgBox.ShowWindowDialogAsync(ParentWindow);
            return;
        }

        var viewModel = new mctcNewPlotViewModel();
        viewModel.mctcBundles = ParentWindow.ViewModel.mctcBundles;
        viewModel.ExistingPlotNames = ParentWindow.ViewModel.MctcBundleNodes
            .SelectMany(b => b.Children.Select(x => x.Label)).ToList();
        viewModel.SelectedBundleId = selectedBundleId;

        var dialog = new mctcNewPlotDialog() { ViewModel = viewModel };
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            dialog.ViewModel.mctcPlotDto.TimeCreated = DateTime.Now;
            ParentWindow.CreateMctcPlot(dialog.ViewModel.mctcPlotDto);
        }
    }

    // Settingw
    public async void Settings_PhaseNames()
    {
        var vm = new SettingsPhaseNamesViewModel();
        var dialog = new SettingsPhaseNamesDialog() { ViewModel = vm };
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);
    }

    public async void Settings_XeosVariables()
    {
        var vm = new SettingsXeosVariablesViewModel();
        var dialog = new SettingsXeosVariablesDialog() { ViewModel = vm };
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);
    }

    public async void Settings_PhaseCompositions()
    {
        var vm = new SettingsPhaseCompositionsViewModel();
        var dialog = new SettingsPhaseCompositionsDialog() { ViewModel = vm };
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);
    }

    public async void Settings_VariableCations()
    {
        var vm = new SettingsVariableCationsViewModel();
        var dialog = new SettingsVariableCationsDialog() { ViewModel = vm };
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);
    }

    // View
    public void View_ShowLeftPanel()
    {
        ParentWindow?.ToggleLeftPanelVisibility(itemShowLeftPanel.IsChecked);
        Log.Information(itemShowLeftPanel.IsChecked.ToString());
    }

    // Help
    public void Help()
    {
    }

    public void About()
    {
        if (ParentWindow == null) return;
        var dialog = new AboutDialog();
        dialog.ShowDialog(ParentWindow);
    }
}