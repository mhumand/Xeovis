using Avalonia.Controls;
using Xeovis.UI.ViewModels;

namespace Xeovis.UI;

public partial class MainMenu : UserControl
{
    public MainMenu()
    {
        InitializeComponent();
        DataContext = new MainMenuViewModel();
    }

    private void MenuItem_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
    }
}