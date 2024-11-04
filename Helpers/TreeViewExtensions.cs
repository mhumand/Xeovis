using System.Collections.Generic;
using System.Windows.Forms;

namespace DatabaseReader.Helpers
{
    public static class TreeViewExtensions
    {
        public static List<TreeNode> GetAllNodes(this TreeNode node)
        {
            var nodes = new List<TreeNode>();
            foreach (TreeNode childNode in node.Nodes)
            {
                nodes.Add(childNode);

                if (childNode.Nodes.Count > 0)
                    nodes.AddRange(childNode.GetAllNodes());
            }

            return nodes;
        }

        public static List<TreeNode> GetAllNodes(this TreeView treeView)
        {
            var nodes = new List<TreeNode>();
            foreach(TreeNode node in treeView.Nodes)
                nodes.AddRange(node.GetAllNodes());

            return nodes;
        }
    }
}
