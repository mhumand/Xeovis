using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Xeovis.UI.Models
{
    public partial class TreeNode : ListableItem
    {
        [ObservableProperty]
        private ObservableCollection<TreeNode> children;

        public TreeNode(string label)
        {
            Children = new ObservableCollection<TreeNode>();
            Label = label;
        }

        public TreeNode(string label, ObservableCollection<TreeNode> children)
        {
            Label = label;
            Children = children;
        }
    }
}
