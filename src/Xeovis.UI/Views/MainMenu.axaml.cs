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
using Xeovis.Domain;
using Xeovis.Persistence;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.Models.FileReaders;
using Xeovis.UI.ViewModels.DialogsViewModels;
using Xeovis.UI.Views;
using Xeovis.UI.Views.Helpers;

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
        btnQuit.HotKey = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? new KeyGesture(Key.F4, KeyModifiers.Alt) : new KeyGesture(Key.Q, KeyModifiers.Meta);

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
    public async void Project_New()
    {
        var goAhead = await CheckAndRaiseUnsavedChangesMessage();
        if (!goAhead) return;

        _lastFilePath = string.Empty;
        ProjectLoaded?.Invoke(this, new ProjectFileLoadedEventArgs(null));
    }

    public async void Project_Open()
    {
        var goAhead = await CheckAndRaiseUnsavedChangesMessage();
        if (!goAhead) return;

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;
        var options = new FilePickerOpenOptions()
        {
            Title = "Save",
            AllowMultiple = false,
            FileTypeFilter = [FileExtentionHelper.GetFilePickerFileType(FileExtensionType.XVS)]
        };

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(options);
        if (files == null || files.Count == 0) return;
        var path = Uri.UnescapeDataString(files[0].Path.AbsolutePath);
        var projectFile = ProjectFileManager.Load(path);
        if (projectFile == null)
        {
            // log
            return;
        }

        _lastFilePath = path;
        ProjectLoaded?.Invoke(this, new ProjectFileLoadedEventArgs(projectFile, path));
    }

    private async Task<bool> CheckAndRaiseUnsavedChangesMessage()
    {
        if (File.Exists(_lastFilePath) && ParentWindow != null)
        {
            var mb = MessageBoxManager.GetMessageBoxStandard("Unsaved Changes", "You have unsaved changes.\nDo you want to save before continuing?", ButtonEnum.YesNoCancel, Icon.Warning);
            var result = await mb.ShowWindowDialogAsync(ParentWindow);
            if (result == ButtonResult.Yes)
                await Save(isSaveAs: false);
            else if (result == ButtonResult.Cancel)
                return false;
        }

        return true;
    }

    public async void Project_Save()
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

        var result = ProjectFileManager.Save(projectFile, path);
        if (!result)
        {
            //log
        }
    }

    private ProjectFile GetProjectFile()
    {
        if (ParentWindow == null) return null;
        var projectFile = new ProjectFile();
        projectFile.Experiments = ParentWindow.ViewModel.Experiments;
        projectFile.mctcBundles = ParentWindow.ViewModel.mctcBundles;
        projectFile.mctcPlotDtos = ParentWindow.ViewModel.mctcPlotDtos;
        return projectFile;
    }

    public async void Project_SaveAs()
    {
        await Save(isSaveAs: true);
    }

    public void Project_Quit()
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
                var experiments = await Task.Run(() => expReader.GetExperiments(progress, token));
                ExperimentsLoaded?.Invoke(this, new ExperimentsLoadedEventArgs(experiments, dialog.ViewModel));
            }
            catch (OperationCanceledException)
            {

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

    public void DataManager_Settings()
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
            var mctcBundle = await Task.Run(() => mctcReader.ReadData(progress, token)) ?? throw new OperationCanceledException();
            mctcBundle.TimeLoaded = DateTime.Now;
            mctcBundle.FilesDirectory = folderDirectory;
            return mctcBundle;
        }
        catch (OperationCanceledException)
        {
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
            var msgBox = MessageBoxManager.GetMessageBoxStandard("Error", "No mctc bundle was found! Please load a bundle first.", ButtonEnum.Ok, Icon.Error);
            msgBox.ShowWindowDialogAsync(ParentWindow);
            return;
        }

        var viewModel = new mctcNewPlotViewModel();
        viewModel.mctcBundles = ParentWindow.ViewModel.mctcBundles;
        viewModel.ExistingPlotNames = ParentWindow.ViewModel.MctcBundleNodes.SelectMany(b => b.Children.Select(x => x.Label)).ToList();
        viewModel.SelectedBundleId = selectedBundleId;

        var dialog = new mctcNewPlotDialog() { ViewModel = viewModel };
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            dialog.ViewModel.mctcPlotDto.TimeCreated = DateTime.Now;
            ParentWindow.CreateMctcPlot(dialog.ViewModel.mctcPlotDto);
        }
    }

    // View
    public void View_ShowLeftPanel()
    {
        ParentWindow?.ToggleLeftPanelVisibility(itemShowLeftPanel.IsChecked);
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