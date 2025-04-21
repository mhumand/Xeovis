using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Threading.Tasks;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.Models.FileReaders;
using Xeovis.UI.ViewModels.DialogsViewModels;
using Xeovis.UI.Views;

namespace Xeovis.UI;

public partial class MainMenu : UserControl
{
    public event EventHandler<ExperimentsLoadedEventArgs> ExperimentsLoaded;
    public event EventHandler<mctcBundleLoadedEventArgs> mctcBundleLoaded;

    public MainWindow? ParentWindow;

    private DataManagerLoadDataViewModel _dataManagerLoadDataViewModel;
    private mctcLoadDataViewModel _mctcLoadDataViewModel;

    public MainMenu()
    {
        InitializeComponent();
        DataContext = this;
    }

    // Project
    public void Project_New()
    {

    }

    public void Project_Open()
    {

    }

    public void Project_Save()
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
        var dialog = new DataManagerLoadDataDialog();
        dialog.ViewModel = _dataManagerLoadDataViewModel;
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            _dataManagerLoadDataViewModel = dialog.ViewModel;
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

    public void DataManager_Settings()
    {

    }

    // MCTC Reader
    public async void MctcPlots_LoadData()
    {
        if (ParentWindow == null) return;
        var dialog = new mctcLoadDataDialog();
        dialog.ViewModel = _mctcLoadDataViewModel;
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            _mctcLoadDataViewModel = dialog.ViewModel;
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

    public async void MctcPlots_NewPlot()
    {
        if (ParentWindow == null) return;
        var hasMctcBundle = ParentWindow.ViewModel.mctcBundles.Count > 0;
        if (!hasMctcBundle)
        {
            var msgBox = MessageBoxManager.GetMessageBoxStandard("Error", "No mctc bundle was found! Please load a bundle first.", ButtonEnum.Ok, Icon.Error);
            msgBox.ShowWindowDialogAsync(ParentWindow);
            return;
        }

        var dialog = new mctcNewPlotDialog(ParentWindow.ViewModel.mctcBundles);
        var result = await dialog.ShowDialog<DialogResultType>(ParentWindow);

        if (result == DialogResultType.OK)
        {
            dialog.mctcPlotDto.TimeCreated = DateTime.Now;
            ParentWindow.CreateMctcPlot(dialog.mctcPlotDto);
        }
    }

    public void MctcPlots_Settings()
    {

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