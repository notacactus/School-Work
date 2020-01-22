/* Project: COIS 2020 Assignment 4
 * Author:  Ryland whillans
 * SN:      0618437
 * Date:    2018-06-06
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_4
{
    class Program
    {
        static void Main(string[] args)
        {
            // populates a heap with integers a outputs the resulting heap as well as the results of 2 forms of heapsort
            int[] numbers = new int[10] { 4, 3, 5, 6, 9, 2, 1, 8, 0, 7 };
            BinaryHeap<int> heap1 = new BinaryHeap<int>(numbers.Length + 1);
            heap1.AddItems(numbers);
            Console.Write("Numbers to be added to heap/sorted: ");
            foreach (int num in numbers)
                Console.Write(num + " ");
            Console.Write("\n\nResult of heapsort on array: ");
            foreach (int num in BinaryHeap<int>.HeapSort(numbers))
                Console.Write(num + " ");
            Console.Write("\n\nNumbers stored in heap: ");
            foreach (int num in heap1)
                Console.Write(num + " ");
            Console.Write("\n\nResult of heapsort on heap: ");
            foreach (int num in heap1.HeapSort())
                Console.Write(num + " ");
            Console.WriteLine();

            // populates a binary tree with MobileObjects sorted by distance from origin and outputs them in order
            BinarySearchTree<MobileObject> tree = new BinarySearchTree<MobileObject>();
            MobileObject[] mObjects = GenerateMobileObjects(16);
            foreach (MobileObject item in mObjects)
                tree.Insert(item);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("Tree of MobileObjects sorted by distance from origin:");
            foreach (MobileObject item in tree)
            {
                Console.WriteLine("\nObject Type: " + item.GetType()
                    + "\nID: " + item.ID
                    + "\nDistance From Origin: " + item.DisOrigin);
            }

            // populates a binary tree with integers and ouputs them using a normal in order traversal as well as an in order traversal relying on nodes tracking parents to demonstrate functionality
            BinarySearchTree<int> tree2 = new BinarySearchTree<int>();
            foreach (int num in numbers)
                tree2.Insert(num);
            Console.WriteLine("-----------------------------------------------------------------------------------------------------------------");
            Console.WriteLine("\nItems in tree in order (normal traversal): " + tree2.InOrder());
            Console.WriteLine("\nItems in tree in order (parent based traversal): " + tree2.ParentTraverse());
            
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
        
    }
}
