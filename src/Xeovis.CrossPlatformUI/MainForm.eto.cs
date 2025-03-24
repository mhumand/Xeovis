using Eto.Drawing;
using Eto.Forms;
using Eto.Wpf;
using System;
using System.IO;
using System.Reflection;

namespace Xeovis.CrossPlatformUI
{
    partial class MainForm : Form
    {
        void InitializeComponent()
        {
            Title = "My Eto Form";
            MinimumSize = new Size(200, 200);
            Padding = 10;
            WindowState = WindowState.Maximized;
            Icon = new Icon(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName + "\\Resources\\Xeovis_32x32.png");

            MainMenuBar();

            Content = new StackLayout
            {
                Items =
                {
                    "Hello World!",
					// add more controls here
				}
            };
        }

        void MainMenuBar()
        {
            var quitCommand = new Command { MenuText = "Quit", Shortcut = Application.Instance.CommonModifier | Keys.Q };
            quitCommand.Executed += (sender, e) => Application.Instance.Quit();

            var aboutCommand = new Command { MenuText = "About..." };
            aboutCommand.Executed += (sender, e) => new AboutDialog().ShowDialog(this);


            Menu = new MenuBar()
            {
                Items =
                {
                    FileMenu(),
                    //DataManagerMenu(),
                    //MctcReaderMenu()
                },

                ApplicationItems =
                {
					// application (OS X) or file menu (others)
					//new ButtonMenuItem { Text = "&Preferences..." },
                },

                QuitItem = quitCommand,
                AboutItem = aboutCommand
            };
        }

        SubMenuItem FileMenu()
        {
            var newProject = new ButtonMenuItem() { Text = "New", Shortcut = Application.Instance.CommonModifier | Keys.N };
            newProject.Click += NewProject_Click;

            var openProject = new ButtonMenuItem() { Text = "Open", Shortcut = Application.Instance.CommonModifier | Keys.O };
            openProject.Click += OpenProject_Click;

            var saveProject = new ButtonMenuItem() { Text = "Save", Shortcut = Application.Instance.CommonModifier | Keys.S };
            saveProject.Click += SaveProject_Click;

            var menu = new SubMenuItem()
            {
                Text = "File",
                Items = { newProject, openProject, saveProject }
            };

            return menu;
        }

        private void SaveProject_Click(object sender, EventArgs e)
        {
            
        }

        private void OpenProject_Click(object sender, EventArgs e)
        {
            
        }

        private void NewProject_Click(object sender, EventArgs e)
        {
            
        }

        
    }
}
