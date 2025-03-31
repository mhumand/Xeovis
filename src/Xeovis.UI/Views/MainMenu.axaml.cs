using Avalonia.Controls;
using System;

namespace Xeovis.UI;

public partial class MainMenu : UserControl
{
    private Window _parent;

    public MainMenu()
    {

    }

    public MainMenu(Window parnetWindow) : base()
    {
        InitializeComponent();
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

    public void DataManager_ReadData()
    {

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
        var about = new About();
        about.ShowDialog(_parent);
    }
}