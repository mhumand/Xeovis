using Avalonia.Controls;
using System;
using System.Threading.Tasks;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.Models.FileReaders;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI;

public partial class MainMenu : UserControl
{
    public event EventHandler<ExperimentsLoadedEventArgs> ExperimentsLoaded;
    public event EventHandler<mctcBundleLoadedEventArgs> mctcBundleLoaded;

    public Window Parent;

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
        if (Parent == null) return;
        var dialog = new DataManagerLoadDataDialog();
        dialog.ViewModel = _dataManagerLoadDataViewModel;
        var result = await dialog.ShowDialog<DialogResultType>(Parent);

        if (result == DialogResultType.OK)
        {
            _dataManagerLoadDataViewModel = dialog.ViewModel;
            var progressDialog = new ProgressBarDialog();
            progressDialog.ShowDialog(Parent);

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
        if (Parent == null) return;
        var dialog = new mctcLoadDataDialog();
        dialog.ViewModel = _mctcLoadDataViewModel;
        var result = await dialog.ShowDialog<DialogResultType>(Parent);

        if (result == DialogResultType.OK)
        {
            _mctcLoadDataViewModel = dialog.ViewModel;
            var progressDialog = new ProgressBarDialog();
            progressDialog.ShowDialog(Parent);

            var progress = progressDialog.ProgressReporter;
            var token = progressDialog.Token;

            try
            {
                var vm = dialog.ViewModel;
                var mctcReader = new mctcDataReader(vm.FolderDirectory);
                var mctcBundle = await Task.Run(() => mctcReader.ReadData(progress, token));
                mctcBundle.Name = vm.MctcBundleName;
                mctcBundle.FilesDirectory = vm.FolderDirectory;
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

    public void MctcPlots_NewPlot()
    {

    }

    public void MctcPlots_Settings()
    {

    }

    // About
    public void About()
    {
        if (Parent == null) return;
        var dialog = new AboutDialog();
        dialog.ShowDialog(Parent);
    }
}