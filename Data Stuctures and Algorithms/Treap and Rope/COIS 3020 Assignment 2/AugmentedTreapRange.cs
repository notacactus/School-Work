using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_3020_Assignment_2
{
    public interface IContainer<T>
    {
        void MakeEmpty();
        bool Empty();
        int Size();
    }

    //-------------------------------------------------------------------------

    public interface ISearchable<T> : IContainer<T>
    {
        void Add(T item);
        void Remove(T item);
        bool Contains(T item);
        T Rank(int i);
    }

    //-------------------------------------------------------------------------

    // Generic node class for a Treap

    public class Node<T> where T : IComparable
    {
        private static Random R = new Random();

        // Read/write properties

        public T Item { get; set; }
        public int Priority { get; set; }
        public int NumItems { get; set; }
        public Node<T> Left { get; set; }
        public Node<T> Right { get; set; }

        public Node(T item)
        {
            Item = item;
            Priority = R.Next(10, 100);
            NumItems = 1;
            Left = Right = null;
        }
    }

    //-------------------------------------------------------------------------

    // Implementation:  Treap

    public class AugmentedTreapRange<T> : ISearchable<T> where T : IComparable
    {
        private Node<T> Root;  // Reference to the root of the Treap

        // Constructor Treap
        // Creates an empty Treap
        // Time complexity:  O(1)

        public AugmentedTreapRange()
        {
            MakeEmpty();
        }

        // InRange
        // returns number of values between parameters X and Y inclusive, -1 for invalid bounds
        // Parameters:
        //  T X - first bound on range to check
        //  T Y - second bound on range to check
        public int InRange(T X, T Y)
        {
            int xPrevRank;  // rank of first item < lower bound
            int yPrevRank;  // rank of first item <= upper bound
                            // determines which parameter is upper/lower bound and determines rank of preceding elements
            if (X.CompareTo(Y) < 0)
            {
                xPrevRank = PreviousRank(Root, 0, X);
                yPrevRank = PreviousRank(Root, 0, Y);
            }
            else
            {
                xPrevRank = PreviousRank(Root, 0, Y);
                yPrevRank = PreviousRank(Root, 0, X);
            }
            // if item on lower bound (negative return value) set to posative and subtract 1
            if (xPrevRank < 0)
                xPrevRank = (xPrevRank * -1) - 1;

            // return difference between number of items <= upper bound and number of items < lower bound
            return Math.Abs(yPrevRank) - xPrevRank;
        }

        // PreviousRank
        // returns the rank of the next item <= a given value
        // returns negative of value to signify item was equal to value
        // Parameters:
        //  Node<T> root - root of current subtree
        // int rankLeft  - number of items above and left the current node
        // T item        - item searching for rank of
        private int PreviousRank(Node<T> root, int rankLeft, T item)
        {
            // calculates rank within subtree
            int relativeRank = root.NumItems - (root.Right != null ? root.Right.NumItems : 0);
            // if current item is item searched for return negative of relative rank + items to the left above
            if (item.CompareTo(root.Item) == 0)
                return -(relativeRank + rankLeft);
            // check if item is less than current root
            else if (item.CompareTo(root.Item) < 0)
            {
                // if nothing left of current root, return calculated rank of current node - 1
                if (root.Left == null)
                    return relativeRank + rankLeft - 1;
                // otherwise recurse to the left
                else
                    return PreviousRank(root.Left, rankLeft, item);
            }
            // otherwise item > current root
            else
            {
                // if nothing right of current root, return calculated rank of current node
                if (root.Right == null)
                    return relativeRank + rankLeft;
                // otherwise recurse to the right
                else
                    return PreviousRank(root.Right, relativeRank + rankLeft, item);

            }
        }

        // CalcSize
        // Determines the number of items in the tree at root
        // Time complexity:  O(1)

        private void CalcSize(Node<T> root)
        {
            root.NumItems = 1;
            if (root.Left != null)
                root.NumItems += root.Left.NumItems;
            if (root.Right != null)
                root.NumItems += root.Right.NumItems;
        }

        // LeftRotate
        // Performs a left rotation around the given root
        // Time complexity:  O(1)

        private Node<T> LeftRotate(Node<T> root)
        {
            Node<T> temp = root.Right;
            root.Right = temp.Left;
            CalcSize(root);
            temp.Left = root;
            return temp;
        }

        // RightRotate
        // Performs a right rotation around the given root
        // Time complexity:  O(1)

        private Node<T> RightRotate(Node<T> root)
        {
            Node<T> temp = root.Left;
            root.Left = temp.Right;
            CalcSize(root);
            temp.Right = root;
            return temp;
        }

        // Public Add
        // Inserts the given item into the Treap
        // Calls Private Add to carry out the actual insertion
        // Expected time complexity:  O(log n)

        public void Add(T item)
        {
            Root = Add(item, Root);
        }

        // Add 
        // Inserts item into the Treap and returns a reference to the root
        // Duplicate items are not inserted
        // Expected time complexity:  O(log n)

        private Node<T> Add(T item, Node<T> root)
        {
            int cmp;  // Result of a comparison

            if (root == null)
                return new Node<T>(item);
            else
            {
                cmp = item.CompareTo(root.Item);
                if (cmp > 0)
                {
                    root.Right = Add(item, root.Right);       // Move right
                    if (root.Right.Priority > root.Priority)  // Rotate left
                        root = LeftRotate(root);              // (if necessary)
                }
                else if (cmp < 0)
                {
                    root.Left = Add(item, root.Left);         // Move left
                    if (root.Left.Priority > root.Priority)   // Rotate right
                        root = RightRotate(root);             // (if necessary)
                }
                CalcSize(root);
                return root;
            }
        }

        // Public Remove
        // Removes the given item from the Treap
        // Calls Private Remove to carry out the actual removal
        // Expected time complexity:  O(log n)

        public void Remove(T item)
        {
            Root = Remove(item, Root);
        }

        // Remove 
        // Removes the given item from the Treap
        // Nothing is performed if the item is not found
        // Time complexity:  O(log n)

        private Node<T> Remove(T item, Node<T> root)
        {
            int cmp;  // Result of a comparison

            if (root == null)   // Item not found
                return null;
            else
            {
                cmp = item.CompareTo(root.Item);
                if (cmp < 0)
                    root.Left = Remove(item, root.Left);      // Move left
                else if (cmp > 0)
                    root.Right = Remove(item, root.Right);    // Move right
                else if (cmp == 0)                            // Item found
                {
                    // Case: Two children
                    // Rotate the child with the higher priority to the given root
                    if (root.Left != null && root.Right != null)
                    {
                        if (root.Left.Priority > root.Right.Priority)
                            root = RightRotate(root);
                        else
                            root = LeftRotate(root);
                    }
                    // Case: One child
                    // Rotate the left child to the given root
                    else if (root.Left != null)
                        root = RightRotate(root);
                    // Rotate the right child to the given root
                    else if (root.Right != null)
                        root = LeftRotate(root);

                    // Case: No children (i.e. a leaf node)
                    // Snip off the leaf node containing item
                    else
                        return null;

                    // Recursively move item down the Treap
                    root = Remove(item, root);
                }
                CalcSize(root);
                return root;
            }
        }

        // Contains
        // Returns true if the given item is found in the Treap; false otherwise
        // Expected time complexity:  O(log n)

        public bool Contains(T item)
        {
            Node<T> curr = Root;

            while (curr != null)
            {
                if (item.CompareTo(curr.Item) == 0)     // Found
                    return true;
                else
                    if (item.CompareTo(curr.Item) < 0)
                    curr = curr.Left;               // Move left
                else
                    curr = curr.Right;              // Move right
            }
            return false;
        }


        // Public Rank
        // Calls private Rank which returns the item with rank i
        // Expected time complexity:  O(log n)

        public T Rank(int i)
        {
            return Rank(Root, i);
        }

        // Private Rank
        // Returns the item with the given rank i
        // Expected time complexity:  O(log n)

        public T Rank(Node<T> root, int i)
        {
            int r;

            if (i <= root.NumItems)
            {
                if (root.Left != null)
                    r = root.Left.NumItems + 1;
                else
                    r = 1;

                if (i == r)
                    return root.Item;
                else if (i < r)
                    return Rank(root.Left, i);
                else
                    return Rank(root.Right, i - r);
            }
            else
                // i out of range
                return default(T);
        }

        // MakeEmpty
        // Creates an empty Treap

        public void MakeEmpty()
        {
            Root = null;
        }

        // Empty
        // Returns true if the Treap is empty; false otherwise

        public bool Empty()
        {
            return Root == null;
        }

        // Public Size
        // Returns the number of items in the Treap
        // Time complexity:  O(1)

        public int Size()
        {
            return Root.NumItems;
        }

        // Public Height
        // Returns the height of the Treap
        // Calls Private Height to carry out the actual calculation
        // Time complexity:  O(n)

        public int Height()
        {
            return Height(Root);
        }

        // Private Height
        // Returns the height of the given Treap
        // Time complexity:  O(n)

        private int Height(Node<T> root)
        {
            if (root == null)
                return -1;    // By default for an empty Treap
            else
                return 1 + Math.Max(Height(root.Left), Height(root.Right));
        }

        // Public Print
        // Prints out the items of the Treap inorder
        // Calls Private Print to 

        public void Print()
        {
            Print(Root, 0);
        }

        // Print
        // Inorder traversal of the BST
        // Time complexity:  O(n)

        private void Print(Node<T> root, int index)
        {
            if (root != null)
            {
                Print(root.Right, index + 8);
                Console.WriteLine(new String(' ', index) + root.Item.ToString());
                Print(root.Left, index + 8);
            }
        }
    }

    //-----------------------------------------------------------------------------
    
}
