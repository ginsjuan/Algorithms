using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTrees
{
    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }

        /// <summary>
        /// Create a tree based on a string of array i.e [1,null,2,3]
        /// where first index is root, even index is left node, and odd index is rigth node
        /// </summary>
        /// <param name="tree">A string representation of the tree</param>
        /// <returns>Return a well-formed tree with its values</returns>
        public static TreeNode CreateTree(string tree)
        {
            TreeNode root = new TreeNode();
            tree = tree.Replace("[", "");
            tree = tree.Replace("]", "");

            var mapTree = tree.Split(',').ToList();

            Queue<(string, int)> queue = new Queue<(string, int)>();

            for (int i = 0; i < mapTree.Count; i++)
            {
                var node = (node: mapTree[i], index: i);
                queue.Enqueue(node);
            }

            if (queue.Count >= 1)
            {
                var r = queue.Dequeue();
                //Parse
                var refVal = -1;
                int.TryParse(r.Item1, out refVal);

                if (refVal > -1)
                {
                    root.val = refVal;
                    root = CreateNode(root, queue);
                }

            }

            return root;
        }

        /// <summary>
        /// Create a node passing its root, and childs as a queue
        /// </summary>
        /// <param name="root">The root node</param>
        /// <param name="childs">The childs that are going to be added, left or rigth</param>
        /// <returns>A node with its children added</returns>
        private static TreeNode CreateNode(TreeNode root, Queue<(string, int)> childs)
        {
            if (childs.Count == 0)
                return root;

            var node = childs.Dequeue();

            int? val = -1;

            if (node.Item1 == "null")
                val = null;
            else
                val = int.Parse(node.Item1);

            var i = node.Item2;

            //Add left node
            if (i % 2 != 0) //If not is pair then is a left node
            {
                if (val.HasValue)
                    root.left = CreateNode(new TreeNode(val.Value), childs);
            }

            //Add rigth node
            if (i % 2 == 0)   //If is pair then is a right node
            {
                if (val.HasValue)
                    root.right = CreateNode(new TreeNode(val.Value), childs);
            }

            //If stills nodes deque in root
            if (childs.Count >= 1)
                root = CreateNode(root, childs);

            return root;

        }
    }
}
