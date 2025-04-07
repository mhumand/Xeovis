using Avalonia.Controls;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI;

public partial class DataManagerLoadDataDialog : Window
{
    private DataManagerLoadDataViewModel _viewModel = new DataManagerLoadDataViewModel();

    public DataManagerLoadDataDialog()
    {
        InitializeComponent();
    }
}