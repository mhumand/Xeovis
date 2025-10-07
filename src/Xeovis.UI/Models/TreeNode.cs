using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Xeovis.UI.Models
{
    public partial class TreeNode : ListableItem
    {
        [ObservableProperty]
        private ObservableCollection<TreeNode> children;

        [ObservableProperty]
        private bool isExpanded;

        [ObservableProperty]
        private IBrush borderColor;

        public bool HasChildren => Children != null && Children.Count > 0;

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
