using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_5
{
    class AVLTreeNode<T> : IEnumerable<T>
    {
        public T value;
        public AVLTreeNode<T> left;
        public AVLTreeNode<T> right;
        public AVLTreeNode<T> parent;
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

    class AVLTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private AVLTreeNode<T> root;

        // public insert, calls private insert on given item
        public void Insert(T value)
        {
            root = Insert(root, value);
        }
        // private insert, inserts item at given location and rebalances
        private AVLTreeNode<T> Insert(AVLTreeNode<T> root, T value)
        {
            // if root is empty returns new node with given value
            if (root == null)
            {
                root = new AVLTreeNode<T>();
                root.value = value;
            }
            // if item is less than root, inserts into left subtree, attaches its parent to root, then rebalances
            // otherwise does the same for right subtree
            else if (value.CompareTo(root.value) < 0)
            {
                root.left = Insert(root.left, value);
                root.left.parent = root;
                root = balance_tree(root);
            }
            else
            {
                root.right = Insert(root.right, value);
                root.right.parent = root;
                root = balance_tree(root);
            }
            return root;
        }

        // determines if tree is unbalnced and rebalances if so
        private AVLTreeNode<T> balance_tree(AVLTreeNode<T> root)
        {
            // if unbalanced to left of left performs right rotation
            // if right of left, left rotates left child then right rotates at root
            // if unbalanced to right of right performs left rotation
            // if left of right, right rotates right child then leftt rotates at root
            int b_factor = balance_factor(root);
            if (b_factor > 1)
            {
                if (balance_factor(root.left) > 0)
                {
                    root = RotateLeftLeft(root);
                }
                else
                {
                    root = RotateLeftRight(root);
                }
            }
            else if (b_factor < -1)
            {
                if (balance_factor(root.right) > 0)
                {
                    root = RotateRightLeft(root);
                }
                else
                {
                    root = RotateRightRight(root);
                }
            }
            return root;
        }
        // gets height of tree
        public int getHeight(AVLTreeNode<T> current)
        {
            int height = 0;
            if (current != null)
            {
                int left = getHeight(current.left);
                int right = getHeight(current.right);
                height = Math.Max(left, right) + 1;
            }
            return height;
        }
        // determines if unbalanced
        public int balance_factor(AVLTreeNode<T> current)
        {
            int left = getHeight(current.left);
            int right = getHeight(current.right);
            return left - right;
        }
        // in order traversal, returns string of items
        public string InOrder()
        {
            return InOrder(root);
        }
        private string InOrder(AVLTreeNode<T> root)
        {
            if (root == null)
            {
                return "";
            }
            return InOrder(root.left) + root.value.ToString() + " " + InOrder(root.right);
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


        // public find function, calls private find on given item
        public bool Find(T item)
        {
            return Find(item, root);
        }
        // private find, if current node is null return false,if current node is item return true
        // if current node is greater than item call find on left subtree, if lesser call find on right subtree
        public bool Find(T item, AVLTreeNode<T> root)
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
        private AVLTreeNode<T> Delete(T item, AVLTreeNode<T> root)
        {
            // if current node is null end search
            if (root != null)
            {

                // if current node is item, delete it
                if (item.CompareTo(root.value) == 0)
                    root = DeleteNode(root);
                // otherwise attempt to delete from left/right subtree if item is lesser/greater than current node then balance tree at current node
                else if (item.CompareTo(root.value) < 0)
                {
                    root.left = Delete(item, root.left);
                    root = balance_tree(root);
                }
                else
                {
                    root.right = Delete(item, root.right);
                    root = balance_tree(root);
                }
            }
            return root;
        }
        // deletes given node
        private AVLTreeNode<T> DeleteNode(AVLTreeNode<T> root)
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
        // replaces a node with the next smallest node then balances along path to replacing node
        private AVLTreeNode<T> ReplaceNode(AVLTreeNode<T> root)
        {
            // traverses to the rightmost node in the left subtree of node to be deleted
            AVLTreeNode<T> current = root.left;
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
            // replace value of node to be deleted with rightnmost node
            root.value = current.value;
            // traverses tree from replacing node up to replaced node and balances tree at each node
            current = current.parent;
            while (current != root)
            {
                if (current.parent.left == current)
                    current.parent.left = balance_tree(current.parent.left);
                else
                    current.parent.right = balance_tree(current.parent.right);
                current = current.parent;
            }
            root = balance_tree(root);
            return root;
        }

        // Displays tree with proper spacing
        public string DisplayTree()
        {
            if (root == null)
                return "";
            string output = "";
            // list containing all nodes on current level, begins with just root
            LinkedList<AVLTreeNode<T>> currentNodes = new LinkedList<AVLTreeNode<T>>();
            LinkedList<AVLTreeNode<T>> sortedNodes = new LinkedList<AVLTreeNode<T>>();
            currentNodes.AddLast(root);
            bool empty = false;
            // iterates until no nodes on next floor
            while (empty == false)
            {
                // keeps track if any non-null nodes were added to next floor
                empty = true;
                // list of nodes on next floor
                LinkedList<AVLTreeNode<T>> next = new LinkedList<AVLTreeNode<T>>();
                // iterates through nodes on current floor
                foreach (AVLTreeNode<T> node in currentNodes)
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
            foreach (AVLTreeNode<T> node in sortedNodes)
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
        
        // left rotation
        public AVLTreeNode<T> RotateRightRight(AVLTreeNode<T> root)
        {
            AVLTreeNode<T> pivot = root.right;
            root.right = pivot.left;
            pivot.left = root;
            pivot.parent = root.parent;
            root.parent = pivot;
            if (root.right != null)
                root.right.parent = root;
            return pivot;
        }
        // right rotation
        public AVLTreeNode<T> RotateLeftLeft(AVLTreeNode<T> root)
        {
            AVLTreeNode<T> pivot = root.left;
            root.left = pivot.right;
            pivot.right = root;
            pivot.parent = root.parent;
            root.parent = pivot;
            if (root.left != null)
                root.left.parent = root;
            return pivot;
        }
        // rotates left child to left, then current to right
        public AVLTreeNode<T> RotateLeftRight(AVLTreeNode<T> root)
        {
            root.left = RotateRightRight(root.left);
            return RotateLeftLeft(root);
        }
        // rotates right child to right ,then current to left
        public AVLTreeNode<T> RotateRightLeft(AVLTreeNode<T> root)
        {
            root.right = RotateLeftLeft(root.right);
            return RotateRightRight(root);
        }

    }
}
