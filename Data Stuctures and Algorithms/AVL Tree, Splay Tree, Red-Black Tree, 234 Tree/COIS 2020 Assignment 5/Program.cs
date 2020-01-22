// inspired by  : https://www.geeksforgeeks.org/b-tree-set-3delete/
//              : https://azrael.digipen.edu/~mmead/www/Courses/CS280/Trees-2-3-4-delete.html

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_5
{
    class Program
    {
        static void Main(string[] args)
        {
            BSTTest();
            AVLTest();
            RedBlackTest();
            SplayTest();
            _234Test();
            TimingTest(7000, 5);
            Console.ReadLine();
        }

        // Generates/returns an array of given size with randomly generated MobileObjects
        public static MobileObject[] GenerateMobileObjects(int size)
        {
            Random rnd = new Random();
            string[] names = new string[10] { "Toast", "Toasty", "Toaster", "Cactus", "Notacactus", "Pancreas", "ToastMaster", "ToastApprentice", "NotToast", "Bread" };
            MobileObject[] mObjects = new MobileObject[size];
            for (int i = 0; i < size; i++)
            {
                if (i % 2 == 1)
                    mObjects[i] = new NPC(rnd, names[i % names.Length], i, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99);
                else
                    mObjects[i] = new Vehicle(rnd, names[i % names.Length], i, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99);
            }
            return mObjects;
        }
        public static int[] GenerateInts(int size)
        {
            Random rnd = new Random();
            int[] ints = new int[size];
            for (int i = 0; i < size; i++)
            {
                ints[i] = rnd.Next(1,100);
            }
            return ints;
        }

        // Tests bst insert/delete/traverse/find
        public static void BSTTest()
        {
            int[] numbers = { 50, 25, 75, 15, 40, 60, 95 };
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Testing BST Functions");

            Console.WriteLine("----------------");

            Console.WriteLine("Testing BST Insertion" +
                "\nItems to be inserted:");
            foreach (int num in numbers)
                Console.Write(" " + num);
            Console.WriteLine("\nTree after insertion:");
            foreach (int item in numbers)
                bst.Insert(item);
            Console.WriteLine(bst.DisplayTree());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing BST Traversal:");
            Console.WriteLine("In order traversal: " + bst.InOrder());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing BST Deletion (no children)" +
                "\nItem to be deleted: 60");
            bst.Delete(60);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(bst.DisplayTree());

            Console.WriteLine("\nTesting BST Deletion (2 children)" +
                "\nItem to be deleted: 50");
            bst.Delete(50);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(bst.DisplayTree());

            Console.WriteLine("\nTesting BST Deletion (left child)" +
                "\nItem to be deleted: 25");
            bst.Delete(25);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(bst.DisplayTree());

            Console.WriteLine("\nTesting BST Deletion (right child)" +
                "\nItem to be deleted: 75");
            bst.Delete(75);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(bst.DisplayTree());

            Console.WriteLine("\nTesting AVL Deletion (not in tree)" +
                "\nItem to be deleted: 500");
            bst.Delete(500);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(bst.DisplayTree());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing BST Find:");
            Console.WriteLine("Attempting to find 40: " + (bst.Find(40) ? "Found" : "Not Found"));
            Console.WriteLine("Attempting to find 50: " + (bst.Find(50) ? "Found" : "Not Found"));
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
        }

        // Tests avl tree insert/delete/traverse/find
        public static void AVLTest()
        {
            AVLTree<int> avl = new AVLTree<int>();
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Testing AVL Functions");

            Console.WriteLine("----------------");

            Console.WriteLine("Testing AVL Insertion(no rotation)" +
                "\nItems to be inserted: 50, 25, 75");
            avl.Insert(50);
            avl.Insert(25);
            avl.Insert(75);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(avl.DisplayTree());

            Console.WriteLine("\nTesting AVL Insertion(left-left)" +
                "\nItems to be inserted: 20, 10");
            avl.Insert(20);
            avl.Insert(10);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(avl.DisplayTree());

            Console.WriteLine("\nTesting AVL Insertion(left-right)" +
                "\nItem to be inserted: 30");
            avl.Insert(30);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(avl.DisplayTree());

            Console.WriteLine("\nTesting AVL Insertion(right-right)" +
                "\nItems to be inserted: 80, 90");
            avl.Insert(80);
            avl.Insert(90);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(avl.DisplayTree());

            Console.WriteLine("\nTesting AVL Insertion(right-left)" +
                "\nItem to be inserted: 60");
            avl.Insert(60);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(avl.DisplayTree());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing AVL Traversal:");
            Console.WriteLine("In order traversal: " + avl.InOrder());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing AVL Deletion (no children)" +
                "\nItem to be deleted: 60");
            avl.Delete(60);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(avl.DisplayTree());

            Console.WriteLine("\nTesting AVL Deletion (2 children)" +
                "\nItem to be deleted: 75");
            avl.Delete(75);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(avl.DisplayTree());

            Console.WriteLine("\nTesting AVL Deletion (left child)" +
                "\nItem to be deleted: 20");
            avl.Delete(20);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(avl.DisplayTree());

            Console.WriteLine("\nTesting AVL Deletion (right child)" +
                "\nItem to be deleted: 80");
            avl.Delete(80);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(avl.DisplayTree());

            Console.WriteLine("\nTesting AVL Deletion (not in tree)" +
                "\nItem to be deleted: 500");
            avl.Delete(500);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(avl.DisplayTree());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing AVL Find:");
            Console.WriteLine("Attempting to find 40: " + (avl.Find(40) ? "Found" : "Not Found"));
            Console.WriteLine("Attempting to find 50: " + (avl.Find(50) ? "Found" : "Not Found"));
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
        }

        // Tests red black tree insert/delete/traverse/find
        public static void RedBlackTest()
        {
            RedBlackTree<int> redBlack = new RedBlackTree<int>();
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Testing RedBlack Functions");

            Console.WriteLine("----------------");

            Console.WriteLine("Testing RedBlack Insertion(root)" +
                "\nItem to be inserted: 50");
            redBlack.Insert(50);
            Console.WriteLine("\nTree after insertion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours after insertion:");
            Console.WriteLine(redBlack.DisplayTreeRB());

            Console.WriteLine("\nTesting RedBlack Insertion(black parent)" +
                "\nItems to be inserted: 25, 70");
            redBlack.Insert(25);
            redBlack.Insert(70);
            Console.WriteLine("\nTree after insertion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours after insertion:");
            Console.WriteLine(redBlack.DisplayTreeRB());

            Console.WriteLine("\nTesting RedBlack Insertion(red uncle)" +
                "\nItem to be inserted: 80");
            redBlack.Insert(80);
            Console.WriteLine("\nTree after insertion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours after insertion:");
            Console.WriteLine(redBlack.DisplayTreeRB());

            Console.WriteLine("\nTesting RedBlack Insertion(black uncle)" +
                "\nItems to be inserted: 75");
            redBlack.Insert(75);
            Console.WriteLine("\nTree after insertion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours after insertion:");
            Console.WriteLine(redBlack.DisplayTreeRB());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing RedBlack Traversal:");
            Console.WriteLine("In order traversal: " + redBlack.InOrder());

            Console.WriteLine("----------------");
            redBlack.Insert(30);
            redBlack.Insert(20);
            Console.WriteLine("Testing RedBlack Deletion (2 children)" +
                "\nItem to be deleted: 50");
            Console.WriteLine("Tree before deletion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours before deletion:");
            Console.WriteLine(redBlack.DisplayTreeRB());
            redBlack.Delete(50);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours after deletion:");
            Console.WriteLine(redBlack.DisplayTreeRB());

            Console.WriteLine("\nTesting RedBlack Deletion (red no children)" +
                "\nItem to be deleted: 70");
            redBlack.Delete(70);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours after deletion:");
            Console.WriteLine(redBlack.DisplayTreeRB());

            Console.WriteLine("\nTesting RedBlack Deletion (left child)" +
                "\nItem to be deleted: 20");
            redBlack.Delete(20);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours after deletion:");
            Console.WriteLine(redBlack.DisplayTreeRB());

            Console.WriteLine("\nTesting RedBlack Deletion (right child)" +
                "\nItem to be deleted: 80");
            redBlack.Delete(80);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours after deletion:");
            Console.WriteLine(redBlack.DisplayTreeRB());

            Console.WriteLine("\nTesting RedBlack Deletion (black no children)" +
                "\nItem to be deleted: 75");
            redBlack.Delete(75);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours after deletion:");
            Console.WriteLine(redBlack.DisplayTreeRB());

            Console.WriteLine("\nTesting RedBlack Deletion (not in tree)" +
                "\nItem to be deleted: 500");
            redBlack.Delete(500);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(redBlack.DisplayTree());
            Console.WriteLine("\nTree colours after deletion:");
            Console.WriteLine(redBlack.DisplayTreeRB());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing RedBlack Find:");
            Console.WriteLine("Attempting to find 25: " + (redBlack.Find(25) ? "Found" : "Not Found"));
            Console.WriteLine("Attempting to find 50: " + (redBlack.Find(50) ? "Found" : "Not Found"));
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
        }

        // Tests splay tree insert/delete/traverse/find
        public static void SplayTest()
        {
            SplayTree<int> splay = new SplayTree<int>();
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Testing Splay Functions");

            Console.WriteLine("----------------");

            Console.WriteLine("Testing Splay Insertion(left)" +
                "\nItems to be inserted: 50, 25");
            splay.Insert(50);
            splay.Insert(25);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(splay.DisplayTree());
            Console.WriteLine("\nTesting Splay Insertion(right-right)" +
                "\nItem to be inserted: 75");
            splay.Insert(75);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("\nTesting Splay Insertion(right)" +
                "\nItem to be inserted: 90");
            splay.Insert(90);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("\nTesting Splay Insertion(left-right)" +
                "\nItem to be inserted: 60");
            splay.Insert(60);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("\nTesting Splay Insertion(left-left)" +
                "\nItem to be inserted: 10");
            splay.Insert(10);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("\nTesting Splay Insertion(right-left)" +
                "\nItems to be inserted: 30");
            splay.Insert(30);
            Console.WriteLine("Tree after insertion:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing Splay Traversal:");
            Console.WriteLine("In order traversal: " + splay.InOrder());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing Splay Deletion (no children)" +
                "\nItem to be deleted: 50");
            splay.Delete(50);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("\nTesting Splay Deletion (2 children)" +
                "\nItem to be deleted: 60");
            splay.Delete(60);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("\nTesting Splay Deletion (left child)" +
                "\nItem to be deleted: 10");
            splay.Delete(10);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("\nTesting Splay Deletion (right child)" +
                "\nItem to be deleted: 90");
            splay.Delete(90);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("\nTesting Splay Deletion (not in tree)" +
                "\nItem to be deleted: 500");
            splay.Delete(500);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing Splay Find:");
            Console.WriteLine("Attempting to find 75: " + (splay.Find(75) ? "Found" : "Not Found"));
            Console.WriteLine("Tree after Find:");
            Console.WriteLine(splay.DisplayTree());

            Console.WriteLine("\nAttempting to find 50: " + (splay.Find(50) ? "Found" : "Not Found"));
            Console.WriteLine("Tree after Find:");
            Console.WriteLine(splay.DisplayTree());
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
        }
        public static void _234Test()
        {

            int[] numbers = { 50, 25, 75, 15, 40, 60, 95, 80, 55, 27, 72, 11, 45, 67, 99, 8 };
            BTree234<int> _234 = new BTree234<int>();
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Testing 234 Functions");

            Console.WriteLine("----------------");

            Console.WriteLine("Testing 234 Insertion" +
                "\nItems to be inserted:");
            foreach (int num in numbers)
                Console.Write(" " + num);
            Console.WriteLine("\nTree after insertion:");
            foreach (int item in numbers)
                _234.Insert(item);
            Console.WriteLine(_234.DisplayTree());
            
            Console.WriteLine("----------------");

            Console.WriteLine("Testing 234 Traversal:");
            Console.WriteLine("In order traversal: " + _234.InOrder());

            Console.WriteLine("----------------");

            Console.WriteLine("\nTesting 234 Deletion" +
                "\nItem to be deleted: 25");
            _234.Delete(25);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(_234.DisplayTree());

            Console.WriteLine("\nTesting 234 Deletion" +
                "\nItem to be deleted: 72");
            _234.Delete(72);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(_234.DisplayTree());

            Console.WriteLine("\nTesting 234 Deletion" +
                "\nItem to be deleted: 55");
            _234.Delete(55);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(_234.DisplayTree());

            Console.WriteLine("\nTesting 234 Deletion" +
                "\nItem to be deleted: 99");
            _234.Delete(80);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(_234.DisplayTree());

            Console.WriteLine("\nTesting 234 Deletion (not in tree)" +
                "\nItem to be deleted: 500");
            _234.Delete(500);
            Console.WriteLine("Tree after deletion:");
            Console.WriteLine(_234.DisplayTree());

            Console.WriteLine("----------------");

            Console.WriteLine("Testing 234 Find:");
            Console.WriteLine("Attempting to find 75: " + (_234.Find(75) ? "Found" : "Not Found"));
            Console.WriteLine("Attempting to find 12: " + (_234.Find(12) ? "Found" : "Not Found"));
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
        }

        // Times inserting given number of objects into each tree, performs given number of trial and outputs average runtimes in MS
        public static void TimingTest(int numElements, int numTrials)
        {
            MobileObject[] mObjects = GenerateMobileObjects(numElements);
            MobileObject[] sortedObjects = new MobileObject[mObjects.Length];
            mObjects.CopyTo(sortedObjects, 0);
            Array.Sort(sortedObjects);
            long elapsedTicBST = 0;
            long elapsedTicBSTSorted = 0;
            long elapsedTicAVL = 0;
            long elapsedTicAVLSorted = 0;
            long elapsedTicRedBlack = 0;
            long elapsedTicRedBlackSorted = 0;
            long elapsedTicSplay = 0;
            long elapsedTicSplaySorted = 0;
            long elapsedTic234 = 0;
            long elapsedTic234Sorted = 0;
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.Write("Timing insertion of " + numElements + " elements(sorted and unsorted)");
            for (int i = 1; i <= numTrials; i++)
            {
                BinarySearchTree<MobileObject> binarySearchTree = new BinarySearchTree<MobileObject>();
                BinarySearchTree<MobileObject> binarySearchTree2 = new BinarySearchTree<MobileObject>();
                AVLTree<MobileObject> avlTree = new AVLTree<MobileObject>();
                AVLTree<MobileObject> avlTree2 = new AVLTree<MobileObject>();
                RedBlackTree<MobileObject> redBlackTree = new RedBlackTree<MobileObject>();
                RedBlackTree<MobileObject> redBlackTree2 = new RedBlackTree<MobileObject>();
                SplayTree<MobileObject> splayTree = new SplayTree<MobileObject>();
                SplayTree<MobileObject> splayTree2 = new SplayTree<MobileObject>();
                BTree234<MobileObject> _234Tree = new BTree234<MobileObject>();
                BTree234<MobileObject> _234Tree2 = new BTree234<MobileObject>();
                Console.Write("\nTrial " + i + ":");
                var watch = System.Diagnostics.Stopwatch.StartNew();
                foreach (MobileObject obj in mObjects)
                    binarySearchTree.Insert(obj);
                watch.Stop();
                elapsedTicBST += watch.ElapsedTicks;
                Console.Write(" .");
                watch = System.Diagnostics.Stopwatch.StartNew();
                foreach (MobileObject obj in sortedObjects)
                    binarySearchTree2.Insert(obj);
                watch.Stop();
                elapsedTicBSTSorted += watch.ElapsedTicks;
                Console.Write(" .");

                watch = System.Diagnostics.Stopwatch.StartNew();
                foreach (MobileObject obj in mObjects)
                    avlTree.Insert(obj);
                watch.Stop();
                elapsedTicAVL += watch.ElapsedTicks;
                Console.Write(" .");
                watch = System.Diagnostics.Stopwatch.StartNew();
                foreach (MobileObject obj in sortedObjects)
                    avlTree2.Insert(obj);
                watch.Stop();
                elapsedTicAVLSorted += watch.ElapsedTicks;
                Console.Write(" .");

                watch = System.Diagnostics.Stopwatch.StartNew();
                foreach (MobileObject obj in mObjects)
                    redBlackTree.Insert(obj);
                watch.Stop();
                elapsedTicRedBlack += watch.ElapsedTicks;
                Console.Write(" .");
                watch = System.Diagnostics.Stopwatch.StartNew();
                foreach (MobileObject obj in sortedObjects)
                    redBlackTree2.Insert(obj);
                watch.Stop();
                elapsedTicRedBlackSorted += watch.ElapsedTicks;
                Console.Write(" .");

                watch = System.Diagnostics.Stopwatch.StartNew();
                foreach (MobileObject obj in mObjects)
                    splayTree.Insert(obj);
                watch.Stop();
                elapsedTicSplay += watch.ElapsedTicks;
                Console.Write(" .");
                watch = System.Diagnostics.Stopwatch.StartNew();
                foreach (MobileObject obj in sortedObjects)
                    splayTree2.Insert(obj);
                watch.Stop();
                elapsedTicSplaySorted += watch.ElapsedTicks;
                Console.Write(" .");

                watch = System.Diagnostics.Stopwatch.StartNew();
                foreach (MobileObject obj in mObjects)
                    _234Tree.Insert(obj);
                watch.Stop();
                elapsedTic234 += watch.ElapsedTicks;
                Console.Write(" .");
                watch = System.Diagnostics.Stopwatch.StartNew();
                foreach (MobileObject obj in sortedObjects)
                    _234Tree2.Insert(obj);
                watch.Stop();
                elapsedTic234Sorted += watch.ElapsedTicks;
                Console.Write(" .");
            }
            elapsedTicBST /= numTrials;
            elapsedTicBSTSorted /= numTrials;
            elapsedTicAVL /= numTrials;
            elapsedTicAVLSorted /= numTrials;
            elapsedTicRedBlack /= numTrials;
            elapsedTicRedBlackSorted /= numTrials;
            elapsedTicSplay /= numTrials;
            elapsedTicSplaySorted /= numTrials;
            elapsedTic234 /= numTrials;
            elapsedTic234Sorted /= numTrials;

            Console.WriteLine("\n{0, -55}" + elapsedTicBST + " MS"
                + "\n{1, -55}" + elapsedTicBSTSorted + " MS"
                + "\n{2, -55}" + elapsedTicAVL + " MS"
                + "\n{3, -55}" + elapsedTicAVLSorted + " MS"
                + "\n{4, -55}" + elapsedTicRedBlack + " MS"
                + "\n{5, -55}" + elapsedTicRedBlackSorted + " MS"
                + "\n{6, -55}" + elapsedTicSplay + " MS"
                + "\n{7, -55}" + elapsedTicSplaySorted + " MS"
                + "\n{8, -55}" + elapsedTic234 + " MS"
                + "\n{9, -55}" + elapsedTic234Sorted + " MS"
                , "BinarySearchTree Insert " + numElements + " MobileObjects:"
                , "BinarySearchTree Insert " + numElements + " MobileObjects(sorted):"
                , "AVLTree Insert " + numElements + " MobileObjects:"
                , "AVLTree Insert " + numElements + " MobileObjects (sorted):"
                , "RedBlackTree Insert " + numElements + " MobileObjects:"
                , "RedBlackTree Insert " + numElements + " MobileObjects (sorted):"
                , "SplayTree Insert " + numElements + " MobileObjects:"
                , "SplayTree Insert " + numElements + " MobileObjects (sorted):"
                , "234Tree Insert " + numElements + " MobileObjects:"
                , "234LTree Insert " + numElements + " MobileObjects (sorted):");
        }
    }
}
