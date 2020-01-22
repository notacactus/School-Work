using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_3020_Assignment_2
{
    class RopeNode
    {
        private RopeNode left;  // left subtree
        private RopeNode right; // right subtree
        private string value;   // value stored at node
        private int length;     // length of value in node

        public RopeNode()
        {
            length = 0;
        }
        public RopeNode(string value)
        {
            this.value = value;
            length = value.Length;
        }

        public string Value { get => value; set => this.value = value; }
        public int Length { get => length; set => length = value; }
        public RopeNode Left { get => left; set => left = value; }
        public RopeNode Right { get => right; set => right = value; }
    }

    class Rope
    {
        const int MAX_LEAF = 10;        // Max length of leaf node
        private RopeNode root;          // root of rope

        // public constructor to create Rope from string
        // Parameters:
        //  string value - string to store in rope
        public Rope(string value)
        {
            // if string within maximum capacity set root new new leaf node
            if (value.Length <= MAX_LEAF)
                root = new RopeNode(value);
            else
            {
                // determine half the maximum length of the bottom row of balanced rope with givne length  
                int halfLength = (int)(Math.Pow(2, Math.Ceiling(Math.Log(((double)value.Length / MAX_LEAF), 2))) * (MAX_LEAF / 2));
                // if string too long, recursively create the rope by concatenating new Ropes made from substrings
                root = Concatenate(new Rope(value.Substring(0, halfLength)).root, new Rope(value.Substring(halfLength)).root);
            }

        }
        // Private Constructor to create Rope from RopeNode
        // Parameters:
        //  RopeNode root - node to set as root
        private Rope(RopeNode root)
        {
            this.root = root;
        }

        // inserts a string at index i
        // Parameters:
        //  string value - string to insert
        //  int index    - index to insert at
        // returns true on success, false if index out of range
        public bool Insert(string value, int index)
        {
            // return false if index out of range
            if (index > root.Length || index < 0)
                return false;
            // Splits before index i
            Rope rightSplit = Split(index - 1);
            // concat given string to first half of split, then concat second half to result and return true
            root = Concatenate(root, (new Rope(value)).root);
            root = Concatenate(root, rightSplit.root);
            return true;
        }

        // deletes a substring from index i to j (inclusive)
        // Parameters:
        //  int startIndex - start index for deletion
        //  int endIndex   - end index for deletion (inclusive)
        // returns true on success, false if index out of range
        public bool Delete(int startIndex, int endIndex)
        {
            // return false if either index is out of range
            if (startIndex > endIndex || startIndex < 0 || endIndex > root.Length - 1)
                return false;
            // Splits after index j
            Rope rightSplit = Split(endIndex);
            // splits before index i
            Split(startIndex - 1);
            // concats Rope after j to Rope before i
            root = Concatenate(root, rightSplit.root);
            return true;
        }

        // public Substring
        // returns the substring from index i to j (inclusive)
        // Parameters:
        //  int startIndex - start index of substring
        //  int endIndex   - end index of substring (inclusive)
        public string Substring(int startIndex, int endIndex)
        {
            // return null if either index is out of range
            if (startIndex > endIndex || startIndex < 0 || endIndex > root.Length - 1)
                return null;
            // otherwise call recursive private substring
            return Substring(root, startIndex, endIndex);
        }

        // private Substring
        // returns substring within index range in subtree rooted in given node
        // Parameters:
        //  RopeNode root - root of subtree
        //  int startIndex - start index of substring relative to subtree
        //  int endIndex   - end index of substring (inclusive) relative to subtree

        private string Substring(RopeNode root, int startIndex, int endIndex)
        {
            // if at leaf node return substring from startindex to endindex
            if (root.Value != null)
                return root.Value.Substring(startIndex, endIndex - startIndex + 1);
            // otherwise return the results of recursively calling substring on left subtree (if within index range) concatenated to the result from the right (if within range)
            return (startIndex < root.Left.Length ?
                        Substring(root.Left, startIndex, Math.Min(endIndex, root.Left.Length - 1)) : "") +
                   (endIndex >= root.Left.Length ?
                        Substring(root.Right, Math.Max(startIndex - root.Left.Length, 0), endIndex - root.Left.Length) : "");
        }

        // public ChatAt
        // returns the character at given index
        // Parameters:
        //  int index - index of char to return
        public char CharAt(int index)
        {
            // return null char if index out of range
            if (index > root.Length - 1 || index < 0)
                return '\0';
            // otherwise call private CharAt
            else return CharAt(root, index);
        }

        // private CharAt
        // returns character at given relative index
        // Parameters:
        //  RopeNode root - root of subtree
        //  int index     - index relative to subtree
        private char CharAt(RopeNode root, int index)
        {
            // if at leaf node return char at given index
            if (root.Value != null)
                return root.Value[index];
            // otherwise return the results of recursively calling ChatAt on left or right subtree, depending on index
            return (index < root.Left.Length ? CharAt(root.Left, index) : CharAt(root.Right, index - root.Left.Length));
        }


        // public Find
        // returns the starting index of the first instance of a string within the rope, returns -1 if not found
        // Parameters:
        //  string value - string to search for
        public int Find(string value)
        {
            // return -1 if null or empty string
            if (value == null || value == "")
                return -1;
            // creates stringbuiler with same length as given string to substring for current position in rope
            StringBuilder current = new StringBuilder(0, value.Length);
            int pos = 0;    // current index
            // return -1 if given string loger than rope, otherwise returns position after recursive Find
            return value.Length <= root.Length && Find(root, ref pos, current, value) ? pos - value.Length : -1;
        }

        // private Find
        // traverses rope in order and returns the starting index of the first instance of a given string, returns true if found
        // Parameters:
        //  RopeNode root        - root of current subtree
        //  ref int pos          - current index in rope
        // StringBuilder current - current substring in rope
        //  string value         - string to search for
        private bool Find(RopeNode root, ref int pos, StringBuilder current, string value)
        {
            // check if at leaf node
            if (root.Value != null)
            {
                // iterate through string in leaf node and add each char to stringbuilder 1 by 1
                foreach (char ch in root.Value)
                {

                    current.Append(ch);
                    pos++;
                    // if stringbuilder full, compare to given string and return true if matching, otherwise remove first char from stringbuilder
                    if (current.Length == current.MaxCapacity)
                    {
                        if (current.ToString() == value)
                            return true;
                        current.Remove(0, 1);
                    }
                }
                // return false if not found
                return false;
            }
            else
            {
                // returns result of recursive find on left and right subtrees
                return (root.Left != null && Find(root.Left, ref pos, current, value)
                    || root.Right != null && Find(root.Right, ref pos, current, value));
            }
        }


        // public Split
        // splits Rope into 2 ropes after given index
        // sets root to left split rope and returns right rope
        // Parameters:
        //  int index - index to split after
        public Rope Split(int index)
        {
            // returns null if index out of range
            if (index >= root.Length || index < -1)
                return null;

            RopeNode split = new RopeNode("");  // node to split values into
            // if index -1 (splitting before index 0) set root to empty string and return old root
            if (index == -1)
            {
                RopeNode oldRoot = root;
                root = split;
                return new Rope(oldRoot);
            }
            // if not splitting at end call private Split
            if (index < root.Length - 1)
            {
                int removed = 0;
                root = Split(root, index, ref removed, ref split);
            }
            // after (or if index was last index) return result
            return new Rope(split);
        }

        // private Split
        // recursively traverses rope to given index then splits off nodes to the right into second rope on the way back
        // Parameters:
        //  RopeNode root          - root of current subtree
        // int index               - index to split relative to current subtree
        // ref int removed         - length of removed string for rebuilding node lengths
        // ref RopeNode rightSplit - root of right Rope to attch split nodes to
        private RopeNode Split(RopeNode root, int index, ref int removed, ref RopeNode rightSplit)
        {
            // check if leaf node
            if (root.Value != null)
            {
                // if index at end, return root
                if (index == root.Length - 1)
                    return root;
                // if index in the middle, divide into 2 nodes and place left and right of current root then recursively split
                else
                {
                    root.Left = new RopeNode(root.Value.Substring(0, index + 1));
                    root.Right = new RopeNode(root.Value.Substring(index + 1));
                    root.Value = null;
                    return Split(root, index, ref removed, ref rightSplit);
                }
            }
            else
            {
                // if index to left recurse left
                if (index < root.Left.Length)
                {
                    // store result of recursing left then concatenate right node to right Rope add length to removed counter
                    RopeNode leftSplit = Split(root.Left, index, ref removed, ref rightSplit);
                    removed += root.Right.Length;
                    rightSplit = Concatenate(rightSplit, root.Right);
                    // set root to left node to clean up leftover edges
                    root = leftSplit;
                }
                // otherwise recurse right
                else
                {
                    // decrease length by length of removed strings
                    root.Right = Split(root.Right, index - root.Left.Length, ref removed, ref rightSplit);
                    root.Length -= removed;
                }
                return root;
            }
        }

        // Public Concatenate
        // returns a Rope created by concatenating two Ropes
        // Parameters:
        //  Rope left  - left rope to concat
        //  Rope right - right rope to concat
        public static Rope Concatenate(Rope left, Rope right)
        {
            // Calls private Concatenate
            return new Rope(Concatenate(left.root, right.root));
        }

        // Public Concatenate
        // returns a RopeNode created by concatenating two RopeNodes
        // Parameters:
        //  RopeNode left  - left RopeNode to concat
        //  RopeNode right - right RopeNode to concat
        private static RopeNode Concatenate(RopeNode left, RopeNode right)
        {
            // if left or right node contains empty string, return the other
            if (left.Length == 0)
                return right;
            if (right.Length == 0)
                return left;
            // check if right node is leaf
            if (right.Value != null)
            {
                // check if left node is leaf
                if (left.Value != null)
                {
                    // if both can fit in a single node combine them and return
                    if (left.Value.Length + right.Value.Length <= MAX_LEAF)
                        return new RopeNode(left.Value + right.Value);
                }
                // if right child of left node can fit in node with right node, combine them and concat to left child of left node and return
                else if (left.Right.Value != null && left.Right.Value.Length + right.Value.Length <= MAX_LEAF)
                    return Concatenate(left.Left, new RopeNode(left.Right.Value + right.Value));
            }
            // otherwise create new node with left node as left child and right node as right child
            RopeNode root = new RopeNode();
            root.Left = left;
            root.Right = right;
            // then sum the lengths to get length for root and return it
            root.Length = left.Length + right.Length;
            return root;
        }

        
        // public Print
        // prints the string represented by the rope
        public void Print()
        {
            // calls private print
            Print(root);
            Console.WriteLine();
        }

        // private Print
        // prints the string in rope via in-order traversal
        // Parameters:
        //  RopeNode root - root of current subtree
        private void Print(RopeNode root)
        {
            // recurse to left if not null
            if (root.Left != null)
                Print(root.Left);
            // print value if not null
            if (root.Value != null)
                Console.Write(root.Value);
            // recurse right if not null
            if (root.Right != null)
                Print(root.Right);
        }

        // private PrintStructure
        // prints the structure of the rope
        public void PrintStructure()
        {
            // calls private printstructure
            PrintStructure(root, 0);
        }

        // private PrintStructure
        // prints the structure of the rope via reverse in-order traversal
        // Parameters:
        //  RopeNode root - root of current subtree
        //  int index     - depth in tree x offset
        private void PrintStructure(RopeNode root, int index)
        {
            // recurse to left if not null
            if (root.Right != null)
                PrintStructure(root.Right, index + 15);
            // print value or empty value offset by depth
                Console.WriteLine($"{new String(' ', index)}[{(root.Value == null ? new String('-', 10) : root.Value + new String('-', MAX_LEAF - root.Value.Length)), 10} {root.Length, 2}]");
            // recurse to right if not null
            if (root.Left != null)
                PrintStructure(root.Left, index + 15);
        }
    }
}
