using Avalonia.Controls;
using Xeovis.UI.ViewModels.DialogsViewModels;

namespace Xeovis.UI;

public partial class DataManagerLoadDataDialog : Window
{
    private DataManagerLoadDataViewModel _viewModel;

    public DataManagerLoadDataViewModel ViewModel
    {
        get { return _viewModel; }
        set
        {
            _viewModel = value;
            DataContext = value;
        }
    }


    public DataManagerLoadDataDialog()
    {
        InitializeComponent();
    }
}