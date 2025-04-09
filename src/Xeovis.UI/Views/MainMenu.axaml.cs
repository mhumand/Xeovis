using Avalonia.Controls;
using System;
using Xeovis.UI.Models.Enums;
using Xeovis.UI.Models.Events;
using Xeovis.UI.Models.FileReaders;

namespace Xeovis.UI;

public partial class MainMenu : UserControl
{
    public event EventHandler<ExperimentsLoadedEventArgs> ExperimentsLoaded;

    public Window Parent;

    public MainMenu()
    {
        InitializeComponent();
        DataContext = this;
    }

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

    public async void DataManager_LoadData()
    {
        if (Parent == null) return;
        var dialog = new DataManagerLoadDataDialog();
        var result = await dialog.ShowDialog<DialogResultType>(Parent);

        if (result == DialogResultType.OK)
        {
            var expReader = new ExperimentReader(dialog.ViewModel.SelectedFolders, dialog.ViewModel.SubfolderDirectory);
            var experiments = expReader.GetExperiments();
            ExperimentsLoaded?.Invoke(this, new ExperimentsLoadedEventArgs(experiments, dialog.ViewModel));
        }
    }

    public void DataManager_Settings()
    {

    }

    public void MctcReader_NewPlot()
    {

    }

    public void MctcReader_Settings()
    {

    }

    public void About()
    {
        if (Parent == null) return;
        var dialog = new AboutDialog();
        dialog.ShowDialog(Parent);
    }
}