// Enumerator inspired by: https://msdn.microsoft.com/en-us/magazine/mt809121.aspx

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_5
{
    public class BinarySearchTreeNode<T> : IEnumerable<T>
    {
        public T value;
        public BinarySearchTreeNode<T> left;
        public BinarySearchTreeNode<T> right;
        public BinarySearchTreeNode<T> parent;

        // enumerator that returns all values in current and child nodes in order from leftmost to rightmost nodes
        public IEnumerator<T> GetEnumerator()
        {
            if (left != null)
                foreach (T item in left)
                    yield return item;
            yield return value;
            if (right != null)
                foreach (T item in right)
                    yield return item;
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    // basic binary search tree
    class BinarySearchTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private BinarySearchTreeNode<T> root;

        // public insert, takes an item and calls private insert on item at root
        public void Insert(T item)
        {
            root = Insert(root, item);
        }
        //private insert, takes a node and item and attempts to insert at that node
        // if node is not empty recurses on left/right node if item is smaller/larger than item at current position
        private BinarySearchTreeNode<T> Insert(BinarySearchTreeNode<T> root, T item)
        {
            if (root == null)
            {
                root = new BinarySearchTreeNode<T>();
                root.value = item;
            }
            // insertion logic, if the value (v )is < root, insert to the root.left
            // otherwise it's >=, so insert to the right
            // connects node to parent after
            else if (item.CompareTo(root.value) < 0)
            {
                root.left = Insert(root.left, item);
                root.left.parent = root;
            }
            else
            {
                root.right = Insert(root.right, item);
                root.right.parent = root;
            }
            return root;
        }

        // public find function, calls private find on given item
        public bool Find(T item)
        {
            return Find(item, root);
        }
        // private find, if current node is null return false,if current node is item return true
        // if current node is greater than item call find on left subtree, if lesser call find on right subtree
        private bool Find(T item, BinarySearchTreeNode<T> root)
        {
            if (root == null)
                return false;
            else if (item.CompareTo(root.value) == 0)
                return true;
            else if (item.CompareTo(root.value) < 0)
                return Find(item, root.left);
            else
                return Find(item, root.right);
        }

        // public delete, calls private delete on given item
        public void Delete(T item)
        {
            root = Delete(item, root);
        }
        // private delete, finds if item is in tree and if so deletes it
        private BinarySearchTreeNode<T> Delete(T item, BinarySearchTreeNode<T> root)
        {
            // if current node is null end search
            if (root != null)
            {
                // if current node is item, delete it
                if (item.CompareTo(root.value) == 0)
                    return DeleteNode(root);
                // otherwise attempt to delete from left/right subtree if item is lesser/greater than current node
                if (item.CompareTo(root.value) < 0)
                    root.left = Delete(item, root.left);
                else
                    root.right = Delete(item, root.right);
            }
            return root;
        }
        // Deletes given node
        private BinarySearchTreeNode<T> DeleteNode(BinarySearchTreeNode<T> root)
        {
            // if node has no children simply delete
            if (root.left == null && root.right == null)
                return null;
            // if node has only one child replace it with child
            if (root.right == null)
            {
                root.left.parent = root.parent;
                return root.left;
            }
            if (root.left == null)
            {
                root.right.parent = root.parent;
                return root.right;
            }
            // if node has 2 children replace with next smallest node
            return ReplaceNode(root);
        }
        // replaces a node with the next smallest node
        private BinarySearchTreeNode<T> ReplaceNode(BinarySearchTreeNode<T> root)
        {
            // traverses to the rightmost node in the left subtree of node to be deleted
            BinarySearchTreeNode<T> current = root.left;
            while (current.right != null)
                current = current.right;
            // if rightmost node is root of subtree attach left child to left of root, otherwise to right of it's parent
            if (current == root.left)
                root.left = current.left;
            else
                current.parent.right = current.left;
            // attach parent if necessary
            if (current.left != null)
                current.left.parent = current.parent;
            // replace value of node to be deleted with rightmost node
            root.value = current.value;
            return root;
        }

        // Uses emumerator for root node to enumerate tree in order samllest to largest
        public IEnumerator<T> GetEnumerator()
        {
            if (root != null)
                foreach (T item in root)
                    yield return item;
        }
        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        // In order traversal, returns a string containing all elements
        // recursively calls on left node and appends the value returned to the value of current node and the value from recursively calling on the right node
        public string InOrder()
        {
            return InOrder(root);
        }
        private string InOrder(BinarySearchTreeNode<T> root)
        {
            if (root == null)
                return "";
            return InOrder(root.left) + root.value.ToString() + " " + InOrder(root.right);
        }

        // Pre order traversal, returns a string containing all elements
        // appends the value of current node, the value from recursively calling on left node, and the value from recursively calling on the right node
        public string PreOrder()
        {
            return InOrder(root);
        }
        private string PreOrder(BinarySearchTreeNode<T> root)
        {
            if (root == null)
                return "";
            return root.value.ToString() + " " + PreOrder(root.left) + PreOrder(root.right);
        }

        // post order traversal, returns a string containing all elements
        // appends the value from recursively calling on left node, the value from recursively calling on the right node, and the value of current node, 
        public string PostOrder()
        {
            return PostOrder(root);
        }
        private string PostOrder(BinarySearchTreeNode<T> root)
        {
            if (root == null)
                return "";
            return PostOrder(root.left) + PostOrder(root.right) + root.value.ToString() + " ";
        }

        // Breadth first traversal, returns a string containing all elements
        public string BreadthFirst()
        {
            if (root == null)
                return "";
            string output = "";
            // list containing all nodes on current level, begins with just root
            LinkedList<BinarySearchTreeNode<T>> nodes = new LinkedList<BinarySearchTreeNode<T>>();
            nodes.AddLast(root);
            // iterates until no nodes on next floor
            while (nodes.Count > 0)
            {
                // list of nodes on next floor
                LinkedList<BinarySearchTreeNode<T>> next = new LinkedList<BinarySearchTreeNode<T>>();
                // iterates through nodes on current floor
                foreach (BinarySearchTreeNode<T> node in nodes)
                {
                    // adds each node's value to output then enqueues its children into list of nodes on next floor
                    output += node.value + " ";
                    if (node.left != null)
                        next.AddLast(node.left);
                    if (node.right != null)
                        next.AddLast(node.right);
                }
                // replaces list of current floor nodes with list of next floor nodes
                nodes = next;
            }
            return output;
        }

        // Displays tree with proper spacing
        public string DisplayTree()
        {
            if (root == null)
                return "";
            string output = "";
            // list containing all nodes on current level, begins with just root
            LinkedList<BinarySearchTreeNode<T>> currentNodes = new LinkedList<BinarySearchTreeNode<T>>();
            LinkedList<BinarySearchTreeNode<T>> sortedNodes = new LinkedList<BinarySearchTreeNode<T>>();
            currentNodes.AddLast(root);
            bool empty = false;
            // iterates until no nodes on next floor
            while (empty == false)
            {
                // keeps track if any non-null nodes were added to next floor
                empty = true;
                // list of nodes on next floor
                LinkedList<BinarySearchTreeNode<T>> next = new LinkedList<BinarySearchTreeNode<T>>();
                // iterates through nodes on current floor
                foreach (BinarySearchTreeNode<T> node in currentNodes)
                {
                    // enqueues each node's children into list of nodes on next floor
                    // enqueues 2 null nodes when encountering a null node
                    if (node != null)
                    {
                        if (node.left != null || node.right != null)
                            empty = false;
                        next.AddLast(node.left);
                        next.AddLast(node.right);
                    }
                    else
                    {
                        next.AddLast(node);
                        next.AddLast(node);
                    }
                    // enqueues current node into list of all nodes
                    sortedNodes.AddLast(node);
                }
                // replaces list of current floor nodes with list of next floor nodes
                currentNodes = next;
            }
            // counters to keep track of current level and number of nodes added
            int level = 1;
            int nodesAdded = 1;
            // outputs blank spaces for first level
            output += new string(' ', (int)((sortedNodes.Count - 1) / Math.Pow(2, level - 1)));
            //iterates through list of all nodes
            foreach (BinarySearchTreeNode<T> node in sortedNodes)
            {
                // if number of nodes added exceeds 2^level increase the level and outputs initial spacing for level
                if (nodesAdded == Math.Pow(2, level))
                {
                    level++;
                    output += "\n\n";
                    output += new string(' ', (int)((sortedNodes.Count - 1) / Math.Pow(2, level - 1)) - 1);
                }
                // adds value of node(trunicated to length 2) or empty node indicator to output and appropriate spacing based on level and total number of nodes
                if (node != null)
                {
                    string value = node.value.ToString();
                    if (value.Length > 2)
                        value = value.Substring(0, 2);
                    while (value.Length < 2)
                        value = " " + value;
                    output += value;
                }
                else
                    output += "[]";
                output += new string(' ', 2 * (int)((sortedNodes.Count - 1) / Math.Pow(2, level - 1)));
                nodesAdded++;
            }
            return output;
        }

        // finds smallest node by traversing tree to left until there is no left node
        public T FindSmallest()
        {
            if (root == null)
                return default(T);
            BinarySearchTreeNode<T> current = root;
            while (current.left != null)
                current = current.left;
            return current.value;
        }

        // in order traversal, returns a string containing all elements
        // implemented making use of the parent nodes to test their functionality
        public string ParentTraverse()
        {
            BinarySearchTreeNode<T> current = root;
            BinarySearchTreeNode<T> previous;
            string output = "";
            if (current == null)
                return output;
            // loops until returning to root from the right or from left if there is no right branch
            do
            {
                // moves to furthest left child of current and adds its value to output, sets previous to current 
                while (current.left != null)
                    current = current.left;
                previous = current;
                output += current.value + " ";

                // traverses parent nodes until reaching a node with an unvisited right node or reaching the root
                while ((current.right == null || current.right == previous) && current.parent != null)
                {
                    // sets previous to current then sets current to parent and adds value to output if previous was not on right side
                    previous = current;
                    current = current.parent;
                    if (current.right != previous)
                    {
                        output += current.value + " ";
                    }

                }
                // if there is an unvisited branch to the right, moves current to right
                if (current.right != previous && current.right != null)
                {
                    current = current.right;
                }
            } while (current.parent != null);
            return output;
        }
    }
}
