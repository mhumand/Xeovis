using Avalonia.Controls;

namespace Xeovis.UI;

public partial class About : Window
{
    public string Description => GetDescriptionText();
    public About()
    {
        InitializeComponent();
    }

    private string GetDescriptionText()
    {
        string description = "Developed by Mohammad Humand.\nEmail: humandm@student.unimelb.edu.au";
        return description;
    }
}