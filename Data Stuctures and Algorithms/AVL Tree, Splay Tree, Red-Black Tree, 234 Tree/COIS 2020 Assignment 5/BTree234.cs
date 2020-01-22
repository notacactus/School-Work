using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_5
{
    public class BTree234Node<T>
    {
        public T[] values = new T[3];
        public BTree234Node<T>[] children = new BTree234Node<T>[4];
        public BTree234Node<T> parent;
        public int count;
    }

    class BTree234<T> where T : IComparable<T>
    {
        public BTree234Node<T> root;

        public void Insert(T item)
        {
            if (root == null)
            {
                root = new BTree234Node<T>();
                root.values[0] = item;
                root.count = 1;
            }
            else
                Insert(item, root);
        }
        private void Insert(T item, BTree234Node<T> root)
        {
            if (root.count >= 3)
                root = Split(root);
            if (root.children[0] != null)
            {
                for (int i = 0; i < root.count; i++)
                {
                    if (item.CompareTo(root.values[i]) < 0)
                    {
                        Insert(item, root.children[i]);
                        return;
                    }
                    else if (item.CompareTo(root.values[i]) == 0)
                        return;
                }
                Insert(item, root.children[root.count]);
            }
            else
            {
                for (int i = 0; i < root.count; i++)
                {
                    if (item.CompareTo(root.values[i]) < 0)
                    {
                        for (int j = root.count; j > i; j--)
                        {
                            root.values[j] = root.values[j - 1];
                        }
                        root.values[i] = item;
                        root.count++;
                        return;
                    }
                    else if (item.CompareTo(root.values[i]) == 0)
                        return;
                }
                root.values[root.count] = item;
                root.count++;
            }

        }

        private BTree234Node<T> Split(BTree234Node<T> root)
        {
            BTree234Node<T> sibling = new BTree234Node<T>();
            sibling.count = 1;
            root.count = 1;
            sibling.values[0] = root.values[2];
            root.values[2] = default(T);
            if (root.children[0] != null)
            {
                sibling.children[0] = root.children[2];
                sibling.children[0].parent = sibling;
                sibling.children[1] = root.children[3];
                sibling.children[1].parent = sibling;
                root.children[2] = null;
                root.children[3] = null;
            }


            if (root.parent == null)
            {
                BTree234Node<T> parent = new BTree234Node<T>();
                parent.children[0] = root;
                parent.children[1] = sibling;
                parent.values[0] = root.values[1];
                root.values[1] = default(T);
                root.parent = parent;
                sibling.parent = parent;
                parent.count = 1;
                this.root = parent;
            }
            else
            {
                for (int i = 0; i < root.parent.count + 1; i++)
                {
                    if (root.parent.children[i] == root)
                    {
                        for (int j = root.parent.count; j > i; j--)
                        {
                            root.parent.children[j + 1] = root.parent.children[j];
                            root.parent.values[j] = root.parent.values[j - 1];
                        }
                        root.parent.children[i + 1] = sibling;
                        root.parent.values[i] = root.values[1];
                        sibling.parent = root.parent;
                        root.values[1] = default(T);
                        root.parent.count++;
                        break;
                    }
                }
            }
            return root.parent;
        }

        public void Delete(T item)
        {
            if (root != null)
            {
                if (root.children[0] == null && root.count == 1 && root.values[0].CompareTo(item) == 0)
                    root = null;
                else
                    Delete(item, root);
            }
        }

        private void Delete(T item, BTree234Node<T> root)
        {
            if (root == null)
                return;
            for (int i = 0; i <= root.count; i++)
            {
                if (i < root.count && item.CompareTo(root.values[i]) == 0)
                {
                    DeleteNode(root, i);
                    return;
                }
                else if (i == root.count || item.CompareTo(root.values[i]) < 0)
                {
                    if (root.children[0] != null && root.children[i].count == 1)
                        GrowNode(root, i);
                    Delete(item, root.children[i]);
                    return;
                }
            }
        }
        private void GrowNode(BTree234Node<T> root, int index)
        {
            BTree234Node<T> child = root.children[index];
            if (index > 0 && root.children[index - 1].count > 1)
            {
                BTree234Node<T> sibling = root.children[index - 1];
                child.values[1] = child.values[0];
                child.values[0] = root.values[index - 1];
                root.values[index - 1] = sibling.values[sibling.count - 1];
                sibling.values[sibling.count - 1] = default(T);
                child.children[2] = child.children[1];
                child.children[1] = child.children[0];
                child.children[0] = sibling.children[sibling.count];
                sibling.children[sibling.count] = null;
                sibling.count--;
                child.count++;
            }
            else if (index < root.count - 1 && root.children[index + 1].count > 1)
            {
                BTree234Node<T> sibling = root.children[index + 1];
                child.values[1] = root.values[index];
                root.values[index] = sibling.values[0];
                for (int i = 0; i < sibling.count - 1; i++)
                    sibling.values[i] = sibling.values[i + 1];
                sibling.values[sibling.count - 1] = default(T);
                child.children[2] = sibling.children[0];
                for (int i = 0; i < sibling.count - 1; i++)
                    sibling.children[i] = sibling.children[i + 1];
                sibling.children[sibling.count] = null;
                sibling.count--;
                child.count++;
            }
            else
            {
                if (index != root.count)
                    Merge(root, index);
                else
                    Merge(root, index - 1);
            }
        }
        private void DeleteNode(BTree234Node<T> root, int index)
        {
            if (root.children[0] == null)
            {
                root.count--;
                for (int i = index; i < root.count; i++)
                    root.values[i] = root.values[i + 1];
                root.values[root.count] = default(T);
            }
            else if (root.children[index].count > 1)
            {
                BTree234Node<T> child = root.children[index];
                while (child.children[0] != null)
                {
                    Console.WriteLine(child.children[child.count]);
                    child = child.children[child.count];
                    Console.WriteLine(child.values[child.count - 1]);
                }
                root.values[index] = child.values[child.count - 1];
                DeleteNode(child, child.count - 1);
            }
            else if (root.children[index + 1].count > 1)
            {
                BTree234Node<T> child = root.children[index + 1];
                while (child.children[0] != null)
                    child = child.children[0];
                root.values[index] = child.values[0];
                DeleteNode(child, 0);
            }
            else
            {
                Merge(root, index);
                DeleteNode(root.children[index], 1);
            }
        }
        private void Merge(BTree234Node<T> root, int index)
        {

            BTree234Node<T> left = root.children[index];
            BTree234Node<T> right = root.children[index + 1];
            left.values[1] = root.values[index];

            left.values[2] = right.values[0];
            if (left.children[0] != null)
            {
                left.children[2] = right.children[0];
                left.children[3] = right.children[1];
            }
            root.count--;
            for (int i = index; i < root.count; i++)
                root.values[i] = root.values[i + 1];
            root.values[root.count] = default(T);
            for (int i = index + 1; i <= root.count; i++)
                root.children[i] = root.children[i + 1];
            root.children[root.count + 1] = null;
            left.count = 3;
            if (root.count == 0)
                this.root = left;
        }

        public bool Find(T item)
        {
            return Find(item, root);
        }

        private bool Find(T item, BTree234Node<T> root)
        {
            if (root == null)
                return false;
            for (int i = 0; i < root.count; i++)
            {
                if (item.CompareTo(root.values[i]) == 0)
                    return true;
                if (item.CompareTo(root.values[i]) < 0)
                    return Find(item, root.children[i]);
            }
            return Find(item, root.children[root.count]);
        }


        public string InOrder()
        {
            return InOrder(root);
        }
        private string InOrder(BTree234Node<T> root)
        {
            if (root == null)
                return "";
            string output = "";
            for (int i = 0; i < root.count; i++)
                output += InOrder(root.children[i]) + root.values[i] + " ";
            return output + InOrder(root.children[root.count]);
        }

        public string DisplayTree()
        {
            if (root == null)
                return "";
            string output = "";
            // list containing all nodes on current level, begins with just root
            LinkedList<BTree234Node<T>> currentNodes = new LinkedList<BTree234Node<T>>();
            LinkedList<BTree234Node<T>> sortedNodes = new LinkedList<BTree234Node<T>>();
            currentNodes.AddLast(root);
            bool empty = false;
            // iterates until no nodes on next floor
            while (empty == false)
            {
                // keeps track if any non-null nodes were added to next floor
                empty = true;
                // list of nodes on next floor
                LinkedList<BTree234Node<T>> next = new LinkedList<BTree234Node<T>>();
                // iterates through nodes on current floor
                foreach (BTree234Node<T> node in currentNodes)
                {
                    // enqueues each node's children into list of nodes on next floor
                    // enqueues 2 null nodes when encountering a null node
                    if (node != null)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            next.AddLast(node.children[i]);
                        }
                        if (node.children[0] != null)
                            empty = false;
                    }
                    else
                    {
                        next.AddLast(node);
                        next.AddLast(node);
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
            int level = 0;
            int nodesAdded = 0;
            int maxLevel = 0;
            for (int i = 1; i < sortedNodes.Count; i += (int)Math.Pow(4, maxLevel))
                maxLevel++;
            // outputs blank spaces for first level
            if (maxLevel != 0)
                output += new string(' ', (int)(Math.Pow(4, maxLevel - 1) * 24 - 6));
            //iterates through list of all nodes
            foreach (BTree234Node<T> node in sortedNodes)
            {
                nodesAdded++;

                // adds value of node(trunicated to length 2) or empty node indicator to output and appropriate spacing based on level and total number of nodes
                if (node != null)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (node.values[i].CompareTo(default(T)) != 0)
                        {
                            string value = node.values[i].ToString();
                            if (value.Length > 2)
                                value = value.Substring(0, 2);
                            while (value.Length < 2)
                                value = " " + value;
                            output += value;
                        }
                        else
                            output += "[]";
                        if (i < 2)
                            output += "||";

                    }
                }
                else
                    output += "[]||[]||[]";
                if (level == maxLevel)
                    output += "  ";
                else
                    output += new string(' ', 2 * (int)(Math.Pow(4, maxLevel - level - 1) * 24) - 10);

                if (nodesAdded == Math.Pow(4, level))
                {
                    level++;
                    nodesAdded = 0;
                    output += "\n\n";
                    if (maxLevel > level)
                        output += new string(' ', (int)(Math.Pow(4, maxLevel - level - 1) * 24 - 6));
                }
            }
            return output;
        }
    }
}
