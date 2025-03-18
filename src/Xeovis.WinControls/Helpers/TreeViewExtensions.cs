using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Xeovis.WinControls
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
            {
                nodes.Add(node);
                nodes.AddRange(node.GetAllNodes());
            }

            return nodes;
        }

        public static List<TreeNode> GetCheckedNodes(this TreeView treeView)
        {
            var nodes = treeView.GetAllNodes();
            var checkedNodes = nodes.Where(n => n.Checked).ToList();
            return checkedNodes;
        }

        public static List<int> GetCheckedNodesIndex(this TreeView treeView)
        {
            var checkedNodes = treeView.GetCheckedNodes();
            var allNodes = treeView.GetAllNodes();
            var indexes = new List<int>();
            foreach (var node in checkedNodes)
                indexes.Add(allNodes.IndexOf(node));

            return indexes;
        }
    }
}
