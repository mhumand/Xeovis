using System;
using System.Collections.ObjectModel;

namespace Xeovis.UI.Models
{
    public class TreeNode
    {
        public ObservableCollection<TreeNode>? Children { get; }
        public string Title { get; }
        public bool IsChecked { get; } = false;

        public TreeNode(string title)
        {
            Title = title;
        }

        public TreeNode(string title, ObservableCollection<TreeNode> children)
        {
            Title = title;
            Children = children;
        }
    }
}
