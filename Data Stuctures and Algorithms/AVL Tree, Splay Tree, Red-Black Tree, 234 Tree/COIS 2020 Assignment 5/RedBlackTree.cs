// Inspired by: https://en.wikipedia.org/wiki/Red%E2%80%93black_tree

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_5
{
    class RedBlackTreeNode<T> : IEnumerable<T>
    {
        //public int key;  //  we combine our key and value as the same thing but they could be distinct
        public T value;
        public RedBlackTreeNode<T> left;
        public RedBlackTreeNode<T> right;
        public RedBlackTreeNode<T> parent;
        public bool isBlack;
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

    class RedBlackTree<T> : IEnumerable<T> where T : IComparable<T>
    {
        private RedBlackTreeNode<T> root;
        public void Insert(T value)
        {
            RedBlackTreeNode<T> inserted;
            root = Insert(root, value, out inserted);
            balanceInsert(inserted);
        }
        private RedBlackTreeNode<T> Insert(RedBlackTreeNode<T> root, T value, out RedBlackTreeNode<T> inserted)
        {
            if (root == null)
            {
                root = new RedBlackTreeNode<T>();
                root.value = value;
                inserted = root;
            }
            else if (value.CompareTo(root.value) < 0)
            {
                root.left = Insert(root.left, value, out inserted);
                root.left.parent = root;
            }
            else
            {
                root.right = Insert(root.right, value, out inserted);
                root.right.parent = root;
            }
            return root;
        }

        private void balanceInsert(RedBlackTreeNode<T> root)
        {
            RedBlackTreeNode<T> grandparent;
            RedBlackTreeNode<T> parent;
            RedBlackTreeNode<T> uncle;

            while (root.parent != null && !root.parent.isBlack)
            {
                grandparent = root.parent.parent;
                parent = root.parent;
                uncle = grandparent.left == parent ? grandparent.right : grandparent.left;
                if (uncle != null && !uncle.isBlack)
                {
                    parent.isBlack = true;
                    uncle.isBlack = true;
                    grandparent.isBlack = false;
                    root = grandparent;
                }
                else
                {
                    if (grandparent.left == parent)
                    {
                        if (parent.left == root)
                            RotateRight(grandparent);
                        else
                        {
                            RotateLeftRight(grandparent);
                            parent = root;
                        }
                    }
                    else
                    {
                        if (parent.right == root)
                            RotateLeft(grandparent);
                        else
                        {
                            RotateRightLeft(grandparent);
                            parent = root;
                        }
                    }
                    parent.isBlack = true;
                    grandparent.isBlack = false;
                }
            }
            GetNewRoot();
            this.root.isBlack = true;
        }

        public string InOrder()
        {
            return InOrder(root);
        }
        private string InOrder(RedBlackTreeNode<T> root)
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
        public bool Find(T item, RedBlackTreeNode<T> root)
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

        // public delete, calls private delete on given item, then if item was found fixes colouring
        public void Delete(T item)
        {
            RedBlackTreeNode<T> deleted;
            root = Delete(item, root, out deleted);
            if (deleted != null && deleted.isBlack && deleted.left == null && deleted.right == null)
            {
                balanceDelete(deleted);
                GetNewRoot();
            }
        }
        // private delete, finds if item is in tree and if so deletes it
        private RedBlackTreeNode<T> Delete(T item, RedBlackTreeNode<T> root, out RedBlackTreeNode<T> deleted)
        {
            // if current node is null end search
            if (root != null)
            {
                // if current node is item, delete it
                if (item.CompareTo(root.value) == 0)
                {
                    deleted = root;
                    return DeleteNode(root, ref deleted);
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
        private RedBlackTreeNode<T> DeleteNode(RedBlackTreeNode<T> root, ref RedBlackTreeNode<T> deleted)
        {
            // if node has no children simply delete
            if (root.left == null && root.right == null)
                return null;
            // if node has only one child replace it with child
            if (root.right == null)
            {
                root.left.parent = root.parent;
                root.left.isBlack = true;
                return root.left;
            }
            if (root.left == null)
            {
                root.right.parent = root.parent;
                root.right.isBlack = true;
                return root.right;
            }
            // if node has 2 children replace with next smallest node
            return ReplaceNode(root, ref deleted);
        }
        // replaces a node with the next smallest node
        private RedBlackTreeNode<T> ReplaceNode(RedBlackTreeNode<T> root, ref RedBlackTreeNode<T> deleted)
        {
            // traverses to the rightmost node in the left subtree of node to be deleted
            RedBlackTreeNode<T> current = root.left;
            while (current.right != null)
                current = current.right;
            // if rightmost node is root of subtree attach left child to left of root, otherwise to right of it's parent
            if (current == root.left)
                root.left = current.left;
            else
                current.parent.right = current.left;
            // attach parent and recolour left child if necessary
            if (current.left != null)
            {
                current.left.parent = current.parent;
                current.left.isBlack = true;
            }
            else if (current.isBlack)
                deleted = current;
            // replace value of node to be deleted with rightmost node
            root.value = current.value;
            return root;
        }
        private void balanceDelete(RedBlackTreeNode<T> root)
        {
            RedBlackTreeNode<T> parent;
            if (root.parent != null)
            {
                parent = root.parent;
                RedBlackTreeNode<T> sibling;
                if (root.value.CompareTo(parent.value) <= 0)
                    sibling = parent.right;
                else
                    sibling = parent.left;

                if (sibling != null && sibling.isBlack == false)
                {
                    sibling.isBlack = true;
                    parent.isBlack = false;
                    if (parent.left == sibling)
                    {
                        RotateRight(parent);
                        sibling = parent.left;
                    }
                    else
                    {
                        RotateLeft(parent);
                        sibling = parent.right;
                    }
                }
                if (parent.isBlack && sibling.isBlack && sibling.left == null && sibling.right == null)
                {
                    sibling.isBlack = false;
                    balanceDelete(root.parent);
                }
                else if (!parent.isBlack && sibling.isBlack && sibling.left == null && sibling.right == null)
                {
                    sibling.isBlack = false;
                    parent.isBlack = true;
                }
                else
                {
                    if (sibling.isBlack)
                    {
                        if (parent.right == sibling && sibling.right == null && sibling.left != null)
                        {
                            sibling.isBlack = false;
                            sibling.left.isBlack = true;
                            sibling = RotateRight(sibling);
                        }
                        else if (parent.left == sibling && sibling.left == null && sibling.right != null)
                        {
                            sibling.isBlack = false;
                            sibling.right.isBlack = true;
                            sibling = RotateLeft(sibling);
                        }
                    }
                    sibling.isBlack = parent.isBlack;
                    parent.isBlack = true;
                    if (parent.right == sibling)
                    {
                        sibling.right.isBlack = true;
                        RotateLeft(parent);
                    }
                    else
                    {
                        sibling.left.isBlack = true;
                        RotateRight(parent);
                    }
                }

            }
        }
        private void GetNewRoot()
        {
            while (root.parent != null)
                root = root.parent;
        }
        // Displays tree with proper spacing
        public string DisplayTree()
        {
            if (root == null)
                return "";
            string output = "";
            // list containing all nodes on current level, begins with just root
            LinkedList<RedBlackTreeNode<T>> currentNodes = new LinkedList<RedBlackTreeNode<T>>();
            LinkedList<RedBlackTreeNode<T>> sortedNodes = new LinkedList<RedBlackTreeNode<T>>();
            currentNodes.AddLast(root);
            bool empty = false;
            // iterates until no nodes on next floor
            while (empty == false)
            {
                // keeps track if any non-null nodes were added to next floor
                empty = true;
                // list of nodes on next floor
                LinkedList<RedBlackTreeNode<T>> next = new LinkedList<RedBlackTreeNode<T>>();
                // iterates through nodes on current floor
                foreach (RedBlackTreeNode<T> node in currentNodes)
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
            foreach (RedBlackTreeNode<T> node in sortedNodes)
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


        public string DisplayTreeRB()
        {
            if (root == null)
                return "";
            string output = "";
            // list containing all nodes on current level, begins with just root
            LinkedList<RedBlackTreeNode<T>> currentNodes = new LinkedList<RedBlackTreeNode<T>>();
            LinkedList<RedBlackTreeNode<T>> sortedNodes = new LinkedList<RedBlackTreeNode<T>>();
            currentNodes.AddLast(root);
            bool empty = false;
            // iterates until no nodes on next floor
            while (empty == false)
            {
                // keeps track if any non-null nodes were added to next floor
                empty = true;
                // list of nodes on next floor
                LinkedList<RedBlackTreeNode<T>> next = new LinkedList<RedBlackTreeNode<T>>();
                // iterates through nodes on current floor
                foreach (RedBlackTreeNode<T> node in currentNodes)
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
            foreach (RedBlackTreeNode<T> node in sortedNodes)
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
                    if (node.isBlack)
                        output += "BB";
                    else
                        output += "RR";
                }
                else
                    output += "[]";
                output += new string(' ', 2 * (int)((sortedNodes.Count - 1) / Math.Pow(2, level - 1)));
                nodesAdded++;
            }
            return output;
        }
        //AVL Lab

        //this is actually the rotate to the left, it's insert in the
        // right sub tree of a right subtree case
        public RedBlackTreeNode<T> RotateLeft(RedBlackTreeNode<T> root)
        {
            RedBlackTreeNode<T> pivot = root.right;
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
        public RedBlackTreeNode<T> RotateRight(RedBlackTreeNode<T> root)
        {
            RedBlackTreeNode<T> pivot = root.left;
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
        public RedBlackTreeNode<T> RotateLeftRight(RedBlackTreeNode<T> root)
        {
            root.left = RotateLeft(root.left);
            return RotateRight(root);
        }
        public RedBlackTreeNode<T> RotateRightLeft(RedBlackTreeNode<T> root)
        {
            root.right = RotateRight(root.right);
            return RotateLeft(root);
        }

    }

}
