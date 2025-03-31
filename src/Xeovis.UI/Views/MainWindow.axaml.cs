using Avalonia.Controls;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI.Views
{
    public partial class MainWindow : Window
    {
        private MainMenu _menu;
        private MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;

            PrepareUI();
        }

        private void PrepareUI()
        {
            _menu = new MainMenu(this);
            MenuGrid.Children.Add(_menu);
        }
    }
}