using Avalonia.Controls;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}