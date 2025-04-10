using Avalonia.Controls;
using Xeovis.UI.Models;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel = new MainWindowViewModel();

        public MainWindowViewModel ViewModel
        {
            get { return _viewModel; }
            set { _viewModel = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PrepareUI();
        }

        private void MainWindow_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            mainMenu.Parent = this;
        }

        private void PrepareUI()
        {
            mainMenu.ExperimentsLoaded += MainMenu_ExperimentsLoaded;
        }

        private void MainMenu_ExperimentsLoaded(object? sender, Models.Events.ExperimentsLoadedEventArgs e)
        {
            tvDataManager.Items.Clear();
            foreach (var exp in e.Experiments)
            {
                var expNode = new TreeNode(exp.Name);
                foreach (var phase in exp.PhasesWithData)
                {
                    var phaseNode = new TreeNode(phase.ToString());
                    expNode.Children.Add(phaseNode);
                }

                tvDataManager.Items.Add(expNode);
            }
        }
    }
}