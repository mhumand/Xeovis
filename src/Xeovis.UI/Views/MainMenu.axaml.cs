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

    public Window Parent;

    private DataManagerLoadDataViewModel _loadDataViewModel;

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
        dialog.ViewModel = _loadDataViewModel;
        var result = await dialog.ShowDialog<DialogResultType>(Parent);

        if (result == DialogResultType.OK)
        {
            _loadDataViewModel = dialog.ViewModel;
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