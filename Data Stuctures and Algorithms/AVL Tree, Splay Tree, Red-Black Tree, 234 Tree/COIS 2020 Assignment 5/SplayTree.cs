// Inspired by: https://en.wikipedia.org/wiki/Splay_tree

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_5
{
    public class SplayTreeNode<T> : IEnumerable<T>
    {
        public T value;
        public SplayTreeNode<T> left;
        public SplayTreeNode<T> right;
        public SplayTreeNode<T> parent;

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
    class SplayTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private SplayTreeNode<T> root;

        // public insert, takes an item and calls private insert on item at root, then splays inserted node to root
        public void Insert(T item)
        {
            SplayTreeNode<T> inserted;
            root = Insert(root, item, out inserted);
            root = Splay(inserted);
        }
        //private insert, takes a node and item and attempts to insert at that node
        // if node is not empty recurses on left/right node if item is smaller/larger than item at current position
        private SplayTreeNode<T> Insert(SplayTreeNode<T> root, T item, out SplayTreeNode<T> inserted)
        {
            if (root == null)
            {
                root = new SplayTreeNode<T>();
                root.value = item;
                inserted = root;
            }
            // insertion logic, if the value (v )is < root, insert to the root.left
            // otherwise it's >=, so insert to the right
            // connects node to parent after
            else if (item.CompareTo(root.value) < 0)
            {
                root.left = Insert(root.left, item, out inserted);
                root.left.parent = root;

            }
            else
            {
                root.right = Insert(root.right, item, out inserted);
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
        // if item is found, splay node to root
        private bool Find(T item, SplayTreeNode<T> root)
        {
            if (root == null)
                return false;
            else if (item.CompareTo(root.value) == 0)
            {
                this.root = Splay(root);
                return true;
            }
            else if (item.CompareTo(root.value) < 0)
                return Find(item, root.left);
            else
                return Find(item, root.right);
        }

        // public delete, calls private delete on given item, then if item was found and was not root splays its parent to root
        public void Delete(T item)
        {
            SplayTreeNode<T> deleted;
            root = Delete(item, root, out deleted);
            if (deleted != null && deleted.parent != null)
                root = Splay(deleted.parent);
        }
        // private delete, finds if item is in tree and if so deletes it
        private SplayTreeNode<T> Delete(T item, SplayTreeNode<T> root, out SplayTreeNode<T> deleted)
        {
            // if current node is null end search
            if (root != null)
            {
                // if current node is item, delete it
                if (item.CompareTo(root.value) == 0)
                {
                    deleted = root;
                    return DeleteNode(root);
                }
                // otherwise attempt to delete from left/right subtree if item is lesser/greater than current node
                if (item.CompareTo(root.value) < 0)
                    root.left = Delete(item, root.left, out deleted);
                else
                    root.right = Delete(item, root.right, out deleted);
            }
            else
                deleted = null;
            return root;
        }
        // Deletes given node
        private SplayTreeNode<T> DeleteNode(SplayTreeNode<T> root)
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
        private SplayTreeNode<T> ReplaceNode(SplayTreeNode<T> root)
        {
            // traverses to the rightmost node in the left subtree of node to be deleted
            SplayTreeNode<T> current = root.left;
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
        private string InOrder(SplayTreeNode<T> root)
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
        private string PreOrder(SplayTreeNode<T> root)
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
        private string PostOrder(SplayTreeNode<T> root)
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
            LinkedList<SplayTreeNode<T>> nodes = new LinkedList<SplayTreeNode<T>>();
            nodes.AddLast(root);
            // iterates until no nodes on next floor
            while (nodes.Count > 0)
            {
                // list of nodes on next floor
                LinkedList<SplayTreeNode<T>> next = new LinkedList<SplayTreeNode<T>>();
                // iterates through nodes on current floor
                foreach (SplayTreeNode<T> node in nodes)
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

        // starting at given node rotates it upwards until it reaches root
        private SplayTreeNode<T> Splay(SplayTreeNode<T> root)
        {
            if (root.parent == null)
                return root;
            if (root.parent.parent == null)
                return root.parent.left == root ? RotateLeftLeft(root.parent) : RotateRightRight(root.parent);
            if (root.parent.parent.left == root.parent)
                return root == root.parent.parent.left.left ? Splay(RotateLeftLeft(RotateLeftLeft(root.parent.parent))) : Splay(RotateLeftRight(root.parent.parent));
            return root == root.parent.parent.right.right ?  Splay(RotateRightRight(RotateRightRight(root.parent.parent))) : Splay(RotateRightLeft(root.parent.parent));
        }

        // left rotation
        public SplayTreeNode<T> RotateRightRight(SplayTreeNode<T> root)
        {
            SplayTreeNode<T> pivot = root.right;
            root.right = pivot.left;
            pivot.left = root;
            pivot.parent = root.parent;
            if (root.parent != null)
            {
                if (root.parent.right == root)
                    root.parent.right = pivot;
                else
                    root.parent.left = pivot;
            }
            root.parent = pivot;
            if (root.right != null)
                root.right.parent = root;
            return pivot;
        }
        // right rotation
        public SplayTreeNode<T> RotateLeftLeft(SplayTreeNode<T> root)
        {
            SplayTreeNode<T> pivot = root.left;
            root.left = pivot.right;
            pivot.right = root;
            pivot.parent = root.parent;
            if (root.parent != null)
            {
                if (root.parent.right == root)
                    root.parent.right = pivot;
                else
                    root.parent.left = pivot;
            }
            root.parent = pivot;
            if (root.left != null)
                root.left.parent = root;
            return pivot;
        }
        // rotates left child to left, then current to right
        public SplayTreeNode<T> RotateLeftRight(SplayTreeNode<T> root)
        {
            root.left = RotateRightRight(root.left);
            return RotateLeftLeft(root);
        }
        // rotates right child to right ,then current to left
        public SplayTreeNode<T> RotateRightLeft(SplayTreeNode<T> root)
        {
            root.right = RotateLeftLeft(root.right);
            return RotateRightRight(root);
        }

        // Displays tree with proper spacing
        public string DisplayTree()
        {
            if (root == null)
                return "";
            string output = "";
            // list containing all nodes on current level, begins with just root
            LinkedList<SplayTreeNode<T>> currentNodes = new LinkedList<SplayTreeNode<T>>();
            LinkedList<SplayTreeNode<T>> sortedNodes = new LinkedList<SplayTreeNode<T>>();
            currentNodes.AddLast(root);
            bool empty = false;
            // iterates until no nodes on next floor
            while (empty == false)
            {
                // keeps track if any non-null nodes were added to next floor
                empty = true;
                // list of nodes on next floor
                LinkedList<SplayTreeNode<T>> next = new LinkedList<SplayTreeNode<T>>();
                // iterates through nodes on current floor
                foreach (SplayTreeNode<T> node in currentNodes)
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
            foreach (SplayTreeNode<T> node in sortedNodes)
            {
                // if number of nodes added exceeds 2^level increase the level and outputs initial spacing for level
                if (nodesAdded == Math.Pow(2, level))
                {
                    level++;
                    output += "\n\n";
                    output += new string(' ', (int)((sortedNodes.Count - 1) / Math.Pow(2, level - 1)) - 1);
                }
                // adds value of node/empty node indicator to level and appropriate spacing based on level and total number of nodes
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

        // in order traversal, returns a string containing all elements
        // implemented making use of the parent nodes to test their functionality
        public string ParentTraverse()
        {
            SplayTreeNode<T> current = root;
            SplayTreeNode<T> previous;
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
