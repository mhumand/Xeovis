using Avalonia.Controls;
using System;

namespace Xeovis.UI;

public partial class MainMenu : UserControl
{
    private Window _parent;

    public MainMenu()
    {
        InitializeComponent();
    }

    public MainMenu(Window parnetWindow) : this()
    {
        DataContext = this;
        _parent = parnetWindow;
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

    public void DataManager_LoadData()
    {
        var loadData = new DataManagerLoadDataDialog();
        loadData.ShowDialog(_parent);
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
        var about = new AboutDialog();
        about.ShowDialog(_parent);
    }
}