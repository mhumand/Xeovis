using Avalonia.Controls;
using System;

namespace Xeovis.UI.ViewModels
{
    public partial class MainMenuViewModel : ViewModelBase
    {
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
            about.Show();

        }
    }
}
