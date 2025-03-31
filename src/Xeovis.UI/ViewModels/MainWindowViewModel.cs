using Avalonia;
using System.Collections.ObjectModel;
using Xeovis.UI.Models;

namespace Xeovis.UI.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<TreeNode> DataManagerNodes { get; }
        public ObservableCollection<TreeNode> MctcPlotNodes { get; }

        public MainWindowViewModel()
        {

        }
    }
}
