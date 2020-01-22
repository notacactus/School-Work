/*  Project:    COIS Assignment 3
 *  Author:     Ryland Whillans
 *  SN:         0618427
 *  Date:       2018-05-28
 *  Desription: Implements a circularly linked list then times Append and DeleteAll functions for each of ArrayList, DoublyLinkedList, CircularArrayList, CircularLinkedList.
 *              Also implements basic hash table and tests insertion and quadratic probing
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace COIS_2020_Assignment_3
{
    class Program
    {
        static void Main(string[] args)
        {
            // determines the number of tests to run, takes about 5 seconds per test pre structure type (5 * 4 * numTests seconds)
            // change this to something low to speed up execution time
            int numTests = 10;
            
            // Unit testing for circular data structures
            CircularArrayTests();
            CircularLinkedListTests();

            // Tests insertion and probing of hash table
            HashTableTest();

            // Times append and DeleteAll for each structure type then outputs results
            Console.WriteLine("\n\nTiming Tests-------------------------------------------------------");
            Console.WriteLine("Running " + numTests + " trials (Will take Approx " + 20 * numTests + " seconds to complete)");
            string[] arrayListOutput = TimeArrayList(numTests);
            string[] linkedListOutput = TimeLinkedList(numTests);
            string[] circularArrayOutput = TimeCircularArray(numTests);
            string[] circularListOutput = TimeCircularList(numTests);
            Console.WriteLine(arrayListOutput[0]);
            Console.WriteLine(linkedListOutput[0]);
            Console.WriteLine(circularArrayOutput[0]);
            Console.WriteLine(circularListOutput[0]);
            Console.WriteLine();
            Console.WriteLine(arrayListOutput[1]);
            Console.WriteLine(linkedListOutput[1]);
            Console.WriteLine(circularArrayOutput[1]);
            Console.WriteLine(circularListOutput[1]);
            
            Console.ReadLine();
        }

        // Tests and outputs results for various methods of CircularArray
        public static void CircularArrayTests()
        {
            Console.WriteLine("CircularArray Tests-------------------------------------------------------");
            CircularArray<int> circularArray;

            // Testing Constructors
            Console.WriteLine("\nAttempt to create CircularArray with capacity 0:");
            try
            {
                circularArray = new CircularArray<int>();
                Console.WriteLine("Attempt Succeeded");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\nAttempt to create CircularArray with negative capacity:");
            try
            {
                circularArray = new CircularArray<int>(-2);
                Console.WriteLine("Attempt Succeeded");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\nAttempt to create CircularArray with posative capacity:");
            try
            {
                circularArray = new CircularArray<int>(10);
                Console.WriteLine("Attempt Succeeded");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

            // Testing AddBack(), also tests Grow()
            Console.WriteLine("\n\nAttempt to AddBack to list:");
            circularArray = new CircularArray<int>(1);
            Console.WriteLine("Element count before AddBack: " + circularArray.Count());
            circularArray.AddBack(5);
            Console.WriteLine("Element count after AddBack: " + circularArray.Count());

            Console.WriteLine("\nAttempt to AddBack to list at capactiy:");
            Console.WriteLine("Capacity before AddBack: " + circularArray.Capacity());
            Console.WriteLine("Element count before AddBack: " + circularArray.Count());
            circularArray.AddBack(10);
            Console.WriteLine("Capacity after AddBack: " + circularArray.Capacity());
            Console.WriteLine("Element count after AddBack: " + circularArray.Count());

            // Testing Print() and ToString(), also tests looping for AddBack()
            circularArray.AddBack(3);
            circularArray.AddBack(4);
            circularArray.AddBack(-7);
            Console.Write("\n\nAttempt to print using Print: ");
            circularArray.Print();

            Console.Write("\nAttempt to print using ToString: ");
            Console.Write(circularArray.ToString());

            for (int i = 1; i < 6; i++)
            {
                circularArray.RemoveFront();
                circularArray.AddBack(i * 2);
            }
            Console.Write("\n\nAttempt to print with looped list:" +
                "\nPrint output: ");
            circularArray.Print();
            Console.Write("\nToString output: ");
            Console.Write(circularArray.ToString());

            // Testing RemoveFront()
            Console.WriteLine("\n\n\nAttempt to delete front element:");
            Console.WriteLine("Element count before delete: " + circularArray.Count());
            circularArray.Print();
            circularArray.RemoveFront();
            Console.WriteLine("\nElement count after delete: " + circularArray.Count());
            circularArray.Print();

            Console.WriteLine("\n\nAttempt to delete front elements looping to beginning of list:");
            Console.WriteLine("Element count before deletes: " + circularArray.Count());
            circularArray.Print();
            circularArray.RemoveFront();
            circularArray.RemoveFront();
            circularArray.RemoveFront();
            Console.WriteLine("\nElement count after deletes: " + circularArray.Count());
            circularArray.Print();

            Console.WriteLine("\n\nAttempt to delete front element from CircularArray with no elements:");
            circularArray = new CircularArray<int>(5);
            Console.WriteLine("Element count before delete " + circularArray.Count());
            circularArray.RemoveFront();
            Console.WriteLine("Element count after delete: " + circularArray.Count());

            // Testing RemoveBack()
            circularArray.AddBack(1);
            circularArray.AddBack(2);
            circularArray.AddBack(3);
            circularArray.AddBack(4);
            for (int i = 0; i < 3; i++)
            {
                circularArray.RemoveFront();
                circularArray.AddBack(i * 2);
            }
            Console.WriteLine("\n\nAttempt to delete back element:");
            Console.WriteLine("Element count before delete: " + circularArray.Count());
            circularArray.Print();
            circularArray.RemoveBack();
            Console.WriteLine("\nElement count after delete: " + circularArray.Count());
            circularArray.Print();

            Console.WriteLine("\n\nAttempt to delete back elements looping to beginning of list:");
            Console.WriteLine("Element count before deletes: " + circularArray.Count());
            circularArray.Print();
            circularArray.RemoveBack();
            circularArray.RemoveBack();
            Console.WriteLine("\nElement count after deletes: " + circularArray.Count());
            circularArray.Print();

            Console.WriteLine("\n\nAttempt to delete back element from CircularArray with no elements:");
            circularArray = new CircularArray<int>(5);
            Console.WriteLine("Element count before delete " + circularArray.Count());
            circularArray.RemoveBack();
            Console.WriteLine("Element count after delete: " + circularArray.Count());

            //Testing DeleteAll()
            circularArray.AddBack(1);
            circularArray.AddBack(2);
            circularArray.AddBack(3);
            circularArray.AddBack(4);
            for (int i = 0; i < 3; i++)
            {
                circularArray.RemoveFront();
                circularArray.AddBack(i * 2);
            }
            Console.WriteLine("\n\nAttempt to delete all elements from CircularArray");
            Console.WriteLine("Element count before delete " + circularArray.Count());
            circularArray.Print();
            circularArray.DeleteAll();
            Console.WriteLine("\nElement count after delete: " + circularArray.Count());
            circularArray.Print();

            Console.WriteLine("\n\nAttempt to delete all elements from empty CircularArray");
            Console.WriteLine("Element count before delete " + circularArray.Count());
            circularArray.DeleteAll();
            Console.WriteLine("Element count after delete: " + circularArray.Count());
        }

        // Tests and outputs results for various methods of CircularLinkedList
        public static void CircularLinkedListTests()
        {
            Console.WriteLine("\n\nCircularLinkedList Tests-------------------------------------------------------");
            CircularLinkedList<int> circularList;

            // Testing Constructor
            Console.WriteLine("\nAttempt to create CircularLinkedList:");
            try
            {
                circularList = new CircularLinkedList<int>();
                Console.WriteLine("Attempt Succeeded");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Testing PrintAllForward() and PrintAllReverse()
            circularList = new CircularLinkedList<int>();
            circularList.AddFirst(2);
            circularList.AddFirst(4);
            circularList.AddFirst(-7);
            circularList.AddFirst(22);
            Console.WriteLine("Attempt to print all elements:");
            Console.WriteLine("Number of elements in list: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            Console.WriteLine("\nAttempt to print all elements in reverse:");
            Console.WriteLine("Number of elements in list: " + circularList.Count);
            Console.Write(circularList.PrintAllReverse());

            // Testing AddFront()
            Console.WriteLine("\n\nAttempt to add to front of empty CircularLinkedList:");
            circularList = new CircularLinkedList<int>();
            Console.WriteLine("Number of elements in list before add: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());
            circularList.AddFirst(2);
            Console.WriteLine("Number of elements in list after add: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            Console.WriteLine("\nAttempt to add to front of CircularLinkedList:");
            Console.WriteLine("Number of elements in list before add: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());
            circularList.AddFirst(5);
            Console.WriteLine("Number of elements in list after add: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            // Testing AddLast()
            Console.WriteLine("\n\nAttempt to add to end of empty CircularLinkedList:");
            circularList = new CircularLinkedList<int>();
            Console.WriteLine("Number of elements in list before add: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());
            circularList.AddLast(7);
            Console.WriteLine("Number of elements in list after add: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            Console.WriteLine("\nAttempt to add to end of CircularLinkedList:");
            Console.WriteLine("Number of elements in list before add: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());
            circularList.AddLast(-2);
            Console.WriteLine("Number of elements in list after add: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            // Testing DeleteFirst()
            Console.WriteLine("\n\nAttempt to delete first element of CircularLinkedList:");
            Console.WriteLine("Number of elements in list before delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());
            circularList.DeleteFirst();
            Console.WriteLine("Number of elements in list after delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            Console.WriteLine("\nAttempt to delete first element of CircularLinkedList with one element:");
            Console.WriteLine("Number of elements in list before delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());
            circularList.DeleteFirst();
            Console.WriteLine("Number of elements in list after delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            Console.WriteLine("\nAttempt to delete first element of CircularLinkedList with no elements:");
            Console.WriteLine("Number of elements in list before delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());
            circularList.DeleteFirst();
            Console.WriteLine("Number of elements in list after delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            // Testing DeleteLast()
            circularList.AddFirst(4);
            circularList.AddFirst(2);
            Console.WriteLine("\n\nAttempt to delete last element of CircularLinkedList:");
            Console.WriteLine("Number of elements in list before delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());
            circularList.DeleteLast();
            Console.WriteLine("Number of elements in list after delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            Console.WriteLine("\nAttempt to delete last element of CircularLinkedList with one element:");
            Console.WriteLine("Number of elements in list before delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());
            circularList.DeleteLast();
            Console.WriteLine("Number of elements in list after delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            Console.WriteLine("\nAttempt to delete last element of CircularLinkedList with no elements:");
            Console.WriteLine("Number of elements in list before delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());
            circularList.DeleteLast();
            Console.WriteLine("Number of elements in list after delete: " + circularList.Count);
            Console.Write(circularList.PrintAllForward());

            // Testing DeleteAll()
            circularList.AddFirst(2);
            circularList.AddFirst(4);
            circularList.AddFirst(-7);
            Console.WriteLine("\n\nAttempt to delete all elements of CircularLinkedList:");
            Console.WriteLine("Number of elements in list before delete: " + circularList.Count);
            circularList.DeleteAll();
            Console.WriteLine("Number of elements in list after delete: " + circularList.Count);
            Console.WriteLine("\nAttempt to delete all elements of empty CircularLinkedList:");
            Console.WriteLine("Number of elements in list before delete: " + circularList.Count);
            circularList.DeleteAll();
            Console.WriteLine("Number of elements in list after delete: " + circularList.Count);
        }


        // Given an int n determines runtime for Appending/Removing 2000000 MobileObjects to an ArrayList
        // runs n trials and returns 2 strings, the first contains the output of all of the runtimes for Appending as well as the average runtime, the second contains thee same information for removing all items
        static string[] TimeArrayList(int n)
        {
            string[] names = new string[10] { "Toast", "Toasty", "Toaster", "Cactus", "Notacactus", "Pancreas", "ToastMaster", "ToastApprentice", "NotToast", "Bread" };
            string insertOutput = "ArrayList Append 2000000 Runtimes: ";
            string deleteOutput = "ArrayList DeleteAll 2000000 Runtimes: ";
            long insertTotal = 0;
            long deleteTotal = 0;
            ArrayList<MobileObject> arrayList = new ArrayList<MobileObject>(2000000);
            Random rnd = new Random();

            Console.Write("Timing ArrayList Append/Delete");
            for (int i = 0; i < n; i++)
            {
                var watch = Stopwatch.StartNew();
                for (int j = 0; j < arrayList.Capacity(); j++)
                {
                    if (j % 2 == 0)
                    {
                        arrayList.Append(new NPC(names[j % 10] + j / 10, j, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                    }
                    else
                    {
                        arrayList.Append(new Vehicle(names[j % 10] + j / 10, j, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                    }
                }
                watch.Stop();
                insertTotal += watch.ElapsedMilliseconds;
                insertOutput += watch.ElapsedMilliseconds + "MS ";

                watch = Stopwatch.StartNew();
                arrayList.DeleteAll();
                watch.Stop();
                deleteTotal += watch.ElapsedMilliseconds;
                deleteOutput += watch.ElapsedMilliseconds + "MS ";
                Console.Write("  .  ");
            }
            Console.WriteLine();
            insertTotal /= n;
            deleteTotal /= n;
            insertOutput += "\nAverage Runtime: " + insertTotal + "MS";
            deleteOutput += "\nAverage Runtime: " + deleteTotal + "MS";
            return new string[] { insertOutput, deleteOutput };
        }

        // Given an int n, determines runtime for Appending/Removing 2000000 MobileObjects to a DoublyLinkedList
        // runs n trials and returns 2 strings, the first contains the output of all of the runtimes for Appending as well as the average runtime, the second contains thee same information for removing all items
        static string[] TimeLinkedList(int n)
        {
            string[] names = new string[10] { "Toast", "Toasty", "Toaster", "Cactus", "Notacactus", "Pancreas", "ToastMaster", "ToastApprentice", "NotToast", "Bread" };
            string insertOutput = "LinkedList Append 2000000 Runtimes: ";
            string deleteOutput = "LinkedList DeleteAll 2000000 Runtimes: ";
            long insertTotal = 0;
            long deleteTotal = 0;
            DoublyLinkedList<MobileObject> linkedList = new DoublyLinkedList<MobileObject>();
            Random rnd = new Random();

            Console.Write("Timing LinkedList Append/Delete");
            for (int i = 0; i < n; i++)
            {
                var watch = Stopwatch.StartNew();
                for (int j = 0; j < 2000000; j++)
                {
                    if (j % 2 == 0)
                    {
                        linkedList.AddLast(new NPC(names[j % 10] + j / 10, j, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                    }
                    else
                    {
                        linkedList.AddLast(new Vehicle(names[j % 10] + j / 10, j, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                    }
                }
                watch.Stop();
                insertTotal += watch.ElapsedMilliseconds;
                insertOutput += watch.ElapsedMilliseconds + "MS ";

                watch = Stopwatch.StartNew();
                linkedList.DeleteAll();
                watch.Stop();
                deleteTotal += watch.ElapsedMilliseconds;
                deleteOutput += watch.ElapsedMilliseconds + "MS ";
                Console.Write("  .  ");
            }
            Console.WriteLine();
            insertTotal /= n;
            deleteTotal /= n;
            insertOutput += "\nAverage Runtime: " + insertTotal + "MS";
            deleteOutput += "\nAverage Runtime: " + deleteTotal + "MS";
            return new string[] { insertOutput, deleteOutput };
        }

        // Given an int n, determines runtime for Appending/Removing 2000000 MobileObjects to a CircularArray
        // runs n trials and returns 2 strings, the first contains the output of all of the runtimes for Appending as well as the average runtime, the second contains thee same information for removing all items
        static string[] TimeCircularArray(int n)
        {
            string[] names = new string[10] { "Toast", "Toasty", "Toaster", "Cactus", "Notacactus", "Pancreas", "ToastMaster", "ToastApprentice", "NotToast", "Bread" };
            string insertOutput = "CircularArray Append 2000000 Runtimes: ";
            string deleteOutput = "CircularArray DeleteAll 2000000 Runtimes: ";
            long insertTotal = 0;
            long deleteTotal = 0;
            CircularArray<MobileObject> circularArray = new CircularArray<MobileObject>(2000000);
            Random rnd = new Random();

            Console.Write("Timing CircularArray Append/Delete");
            for (int i = 0; i < n; i++)
            {
                var watch = Stopwatch.StartNew();
                for (int j = 0; j < circularArray.Capacity(); j++)
                {
                    if (j % 2 == 0)
                    {
                        circularArray.AddBack(new NPC(names[j % 10] + j / 10, j, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                    }
                    else
                    {
                        circularArray.AddBack(new Vehicle(names[j % 10] + j / 10, j, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                    }
                }
                watch.Stop();
                insertTotal += watch.ElapsedMilliseconds;
                insertOutput += watch.ElapsedMilliseconds + "MS ";

                watch = Stopwatch.StartNew();
                circularArray.DeleteAll();
                watch.Stop();
                deleteTotal += watch.ElapsedMilliseconds;
                deleteOutput += watch.ElapsedMilliseconds + "MS ";
                Console.Write("  .  ");
            }
            Console.WriteLine();
            insertTotal /= n;
            deleteTotal /= n;
            insertOutput += "\nAverage Runtime: " + insertTotal + "MS";
            deleteOutput += "\nAverage Runtime: " + deleteTotal + "MS";
            return new string[] { insertOutput, deleteOutput };
        }

        // Given an int n, determines runtime for Appending/Removing 2000000 MobileObjects to a CircularLinkedList
        // runs 10 trial and returns 2 strings, the first contains the output of all of the runtimes for Appending as well as the average runtime, the second contains thee same information for removing all items
        static string[] TimeCircularList(int n)
        {
            string[] names = new string[10] { "Toast", "Toasty", "Toaster", "Cactus", "Notacactus", "Pancreas", "ToastMaster", "ToastApprentice", "NotToast", "Bread" };
            string insertOutput = "CircularList Append 2000000 Runtimes: ";
            string deleteOutput = "CircularList DeleteAll 2000000 Runtimes: ";
            long insertTotal = 0;
            long deleteTotal = 0;
            CircularLinkedList<MobileObject> circularList = new CircularLinkedList<MobileObject>();
            Random rnd = new Random();

            Console.Write("Timing CircularList Append/Delete");
            for (int i = 0; i < n; i++)
            {
                var watch = Stopwatch.StartNew();
                for (int j = 0; j < 2000000; j++)
                {
                    if (j % 2 == 0)
                    {
                        circularList.AddLast(new NPC(names[j % 10] + j / 10, j, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                    }
                    else
                    {
                        circularList.AddLast(new Vehicle(names[j % 10] + j / 10, j, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                    }
                }
                watch.Stop();
                insertTotal += watch.ElapsedMilliseconds;
                insertOutput += watch.ElapsedMilliseconds + "MS ";

                watch = Stopwatch.StartNew();
                circularList.DeleteAll();
                watch.Stop();
                deleteTotal += watch.ElapsedMilliseconds;
                deleteOutput += watch.ElapsedMilliseconds + "MS ";
                Console.Write("  .  ");
            }
            Console.WriteLine();
            insertTotal /= n;
            deleteTotal /= n;
            insertOutput += "\nAverage Runtime: " + insertTotal + "MS";
            deleteOutput += "\nAverage Runtime: " + deleteTotal + "MS";
            return new string[] { insertOutput, deleteOutput };
        }

        static void HashTableTest()
        {
            Console.WriteLine("\n\nHashTable Tests-------------------------------------------------------");
            // Creates a hashtable of size 50 and inserts 15 strings from array names
            HashTable table = new HashTable(50);
            string[] names = new string[] { "toast", "moon", "toasting", "abcdefghijk", "apocalypse", "ho", "flak", "pancreatic", "a", "penguins", "narwhal", "emu", "toaster", "owl", "Cactus" };
            Console.WriteLine("Testing hash table insert and probing.");
            Console.Write("Adding strings to hashtable: ");
            foreach (string name in names)
            {
                Console.Write(name + " ");
                table.AddItem(name);
            }

            // Probes the hash table for strings in the array names2 (includes first and last string added to hashtable, string not in hash table, and strings sharing the same length/key
            string[] names2 = new string[] { "toast", "toaster", "toas", "Cactus", "narwhal" };
            Console.WriteLine("\nProbing for strings in hashtable: ");
            foreach (string name in names2)
            {
                Console.WriteLine("Probing for \"" + name + (table.FindItem(name) ? "\": Found" : "\": Not Found"));
            }
        }
    }
}
