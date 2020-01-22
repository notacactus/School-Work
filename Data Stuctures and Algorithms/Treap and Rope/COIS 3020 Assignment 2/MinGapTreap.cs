using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_3020_Assignment_2
{
    public class MinGapNode
    {
        private static Random R = new Random();

        // Read/write properties

        public int Value { get; set; }       // number stored
        public int Priority { get; set; }   // priority
        public int NumItems { get; set; }   // number of items in subtree
        public int MinGap { get; set; }     // minimum gap in subtree
        public int MinVal { get; set; }     // minimum value in subtree
        public int maxVal { get; set; }     // maximum value in subtree
        public MinGapNode Left { get; set; }    // left subtree
        public MinGapNode Right { get; set; }   // right subtree

        // Constructor
        public MinGapNode(int item)
        {
            Value = item;
            Priority = R.Next(10, 100);
            NumItems = 1;
            Left = Right = null;
            MinGap = Int32.MaxValue;    // sets minGap to maximum value
            MinVal = item;
            maxVal = item;
        }
    }

    public class MinGapTreap
    {
        private MinGapNode Root;  // Reference to the root of the Treap

        // Constructor Treap
        // Creates an empty Treap
        // Time complexity:  O(1)

        public MinGapTreap()
        {
            MakeEmpty();
        }

        // public MinGap
        // returns the minimum gap between any two values in the treap
        public int MinGap()
        {
            // returns value stored in Rott minGap or -1 if no value/no root
            return Root != null && Root.MinGap != Int32.MaxValue ? Root.MinGap : -1;
        }

        // CalcSize
        // Determines the number of items in the tree at root
        // Time complexity:  O(1)

        private void CalcSize(MinGapNode root)
        {
            root.NumItems = 1;
            if (root.Left != null)
                root.NumItems += root.Left.NumItems;
            if (root.Right != null)
                root.NumItems += root.Right.NumItems;
            // resets minGap to max value
            root.MinGap = Int32.MaxValue;
            // sets min value to minValue of left subtree or to own value if no left subtree
            root.MinVal = root.Left != null ? root.Left.MinVal : root.Value;
            // sets max value to maxValue of right subtree or to own value if no right subtree
            root.maxVal = root.Right != null ? root.Right.maxVal : root.Value;
            // sets minGap to minimum of left subtree minGap, right subtree minGap, item - left subtree maxValue, and right subtree minValue - item
            root.MinGap = Math.Min(root.Left != null ? Math.Min(root.Left.MinGap, root.Value - root.Left.maxVal) : Int32.MaxValue, root.Right != null ? Math.Min(root.Right.MinGap, root.Right.MinVal - root.Value) : Int32.MaxValue);
        }

        // LeftRotate
        // Performs a left rotation around the given root
        // Time complexity:  O(1)

        private MinGapNode LeftRotate(MinGapNode root)
        {
            MinGapNode temp = root.Right;
            root.Right = temp.Left;
            CalcSize(root);
            temp.Left = root;
            return temp;
        }

        // RightRotate
        // Performs a right rotation around the given root
        // Time complexity:  O(1)

        private MinGapNode RightRotate(MinGapNode root)
        {
            MinGapNode temp = root.Left;
            root.Left = temp.Right;
            CalcSize(root);
            temp.Right = root;
            return temp;
        }

        // Public Add
        // Inserts the given item into the Treap
        // Calls Private Add to carry out the actual insertion
        // Expected time complexity:  O(log n)

        public void Add(int item)
        {
            Root = Add(item, Root);
        }

        // Add 
        // Inserts item into the Treap and returns a reference to the root
        // Duplicate items are not inserted
        // Expected time complexity:  O(log n)

        private MinGapNode Add(int item, MinGapNode root)
        {
            int cmp;  // Result of a comparison

            if (root == null)
                return new MinGapNode(item);
            else
            {
                cmp = item.CompareTo(root.Value);
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

        public void Remove(int item)
        {
            Root = Remove(item, Root);
        }

        // Remove 
        // Removes the given item from the Treap
        // Nothing is performed if the item is not found
        // Time complexity:  O(log n)

        private MinGapNode Remove(int item, MinGapNode root)
        {
            int cmp;  // Result of a comparison

            if (root == null)   // Item not found
                return null;
            else
            {
                cmp = item.CompareTo(root.Value);
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

        public bool Contains(int item)
        {
            MinGapNode curr = Root;

            while (curr != null)
            {
                if (item.CompareTo(curr.Value) == 0)     // Found
                    return true;
                else
                    if (item.CompareTo(curr.Value) < 0)
                    curr = curr.Left;               // Move left
                else
                    curr = curr.Right;              // Move right
            }
            return false;
        }


        // Public Rank
        // Calls private Rank which returns the item with rank i
        // Expected time complexity:  O(log n)

        public int Rank(int i)
        {
            return Rank(Root, i);
        }

        // Private Rank
        // Returns the item with the given rank i
        // Expected time complexity:  O(log n)

        public int Rank(MinGapNode root, int i)
        {
            int r;

            if (i <= root.NumItems)
            {
                if (root.Left != null)
                    r = root.Left.NumItems + 1;
                else
                    r = 1;

                if (i == r)
                    return root.Value;
                else if (i < r)
                    return Rank(root.Left, i);
                else
                    return Rank(root.Right, i - r);
            }
            else
                // i out of range
                return -1;
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

        private int Height(MinGapNode root)
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

        private void Print(MinGapNode root, int index)
        {
            if (root != null)
            {
                Print(root.Right, index + 12);
                Console.WriteLine($"{new String(' ', index)}Val: {root.Value.ToString()} Pri: {root.Priority.ToString()} Gap: {(root.MinGap == Int32.MaxValue ? "--" : root.MinGap + "")} Min: {root.MinVal} Max: {root.maxVal}");
                Print(root.Left, index + 12);
            }
            else
            {
                Console.WriteLine(new String(' ', index));
            }
        }
    }
}
