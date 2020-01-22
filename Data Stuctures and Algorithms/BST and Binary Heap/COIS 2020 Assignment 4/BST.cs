// Enumerator inspired by: https://msdn.microsoft.com/en-us/magazine/mt809121.aspx

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_4
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
        
        // public insert, takes an item and if list is empty inserts it at root
        // otherwise calls private insert on item
        public void Insert(T item)
        {
            if (root == null)
            {
                root = new BinarySearchTreeNode<T>();
                root.value = item;
            }
            else
            {
                Insert(root, item);
            }
        }
        //private insert, takes a node and item and attempts to insert at that node
        // if node is not empty recurses on left/right node if item is smaller/larger than item at current position
        private BinarySearchTreeNode<T> Insert(BinarySearchTreeNode<T> child, T item)
        {
            if (child == null)
            {
                child = new BinarySearchTreeNode<T>();
                child.value = item;
            }
            // insertion logic, if the value (v )is < root, insert to the root.left
            // otherwise it's >=, so insert to the right
            // connects node to parent after
            else if (item.CompareTo(child.value) < 0)
            {
                child.left = Insert(child.left, item);
                child.left.parent = child;
            }
            else
            {
                child.right = Insert(child.right, item);
                child.right.parent = child;
            }
            return child;
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
            if (root == null)
                return "";
            return InOrder(root.left) + root.value.ToString() + " " + InOrder(root.right);
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
            if (root == null)
                return "";
            return root.value.ToString() + " " + PreOrder(root.left) + PreOrder(root.right);
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
            if (root == null)
                return "";
            return PostOrder(root.left) + PostOrder(root.right) + root.value.ToString() + " ";
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
