using Avalonia.Controls;

namespace Xeovis.TestUI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new FrmUserInputControl();
            dialog.ShowDialog(this);
        }
    }
}