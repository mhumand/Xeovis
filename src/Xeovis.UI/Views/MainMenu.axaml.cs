using Avalonia.Controls;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;
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

    public MainMenu()
    {
        InitializeComponent();
        DataContext = this;
    }

    // Project
    public void Project_New()
    {

    }

    public async void Project_Open()
    {
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

        ProjectLoaded?.Invoke(this, new ProjectFileLoadedEventArgs(projectFile, path));
    }

    public async void Project_Save()
    {
        var projectFile = GetProjectFile();
        if (projectFile == null) return;

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
        var path = Uri.UnescapeDataString(files.Path.AbsolutePath);
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

    public void Project_SaveAs()
    {

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
    public async void MctcPlots_LoadBundle()
    {
        if (ParentWindow == null) return;
        var viewModel = new mctcLoadDataViewModel();
        viewModel.ExisitingBundleNames = ParentWindow.ViewModel.mctcBundles.Select(x => x.Name).ToList();
        var dialog = new mctcLoadDataDialog() { ViewModel = viewModel };

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
                var mctcReader = new mctcDataReader(vm.FolderDirectory);
                var mctcBundle = await Task.Run(() => mctcReader.ReadData(progress, token));
                if (mctcBundle == null)
                    throw new OperationCanceledException();

                mctcBundle.Name = vm.MctcBundleName;
                mctcBundle.FilesDirectory = vm.FolderDirectory;
                mctcBundle.TimeLoaded = DateTime.Now;
                mctcBundleLoaded?.Invoke(this, new mctcBundleLoadedEventArgs(mctcBundle));
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

    public void LoadMctcBundle(string folderDirectory)
    {
        // todo - M.H. 01/05/2025
    }

    public void MctcPlots_NewPlot()
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