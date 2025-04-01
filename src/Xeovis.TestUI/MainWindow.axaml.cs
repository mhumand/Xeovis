using Avalonia.Controls;

namespace Xeovis.TestUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLabelInputControl_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new FrmUserInputControl();
            dialog.ShowDialog(this);
        }

        private void btnLabelListControl_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new FrmLabelListControl();
            dialog.ShowDialog(this);
        }
    }
}