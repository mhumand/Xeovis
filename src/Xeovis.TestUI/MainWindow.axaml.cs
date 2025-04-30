using Avalonia.Controls;
using System.IO;
using Xeovis.UI;
using Xeovis.UI.Models.Enums;

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

        private void btnCheckBoxControl_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new FrmCheckBoxControl();
            dialog.ShowDialog(this);
        }

        private void btnLabelComboBoxControl_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new FrmLabelComboBoxControl();
            dialog.ShowDialog(this);
        }

        private void btnLabelSpinnerControl_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new FrmLabelSpinnerControl();
            dialog.ShowDialog(this);
        }

        private void btnLabelBrowseDirectoryControl_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new FrmLabelBrowseDirectoryControl();
            dialog.ShowDialog(this);
        }

        private void btnTableControl_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new FrmTableControl();
            dialog.ShowDialog(this);
        }

        private void btnChartControl_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new FrmChartControl();
            dialog.ShowDialog(this);
        }

        private async void btnRenameDialog_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new RenameDialog("test name", new System.Collections.Generic.List<string>() { "Plot", "plot", "check", "test" });
            var result = await dialog.ShowDialog<DialogResultType>(this);
        }
    }
}