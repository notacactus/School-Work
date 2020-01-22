using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_2
{
    class Program
    {
        static void Main(string[] args)
        {
            // calls methods for testing arraylist/linkedlist
            ArrayListTests();
            DoublyLinkedListTests();
            // list of names to populate arraylist/linkedlist
            string[] names = new string[10] { "Toast", "Toasty", "Toaster", "Cactus", "Notacactus", "Pancreas", "ToastMaster", "ToastApprentice", "NotToast", "Bread" };
            
            // generates arraylist/linkedlist of mobileobjects using methods
            ArrayList<MobileObject> arrayList = GenerateMobileObjectArray(names);
            DoublyLinkedList<MobileObject> linkedList = GenerateMobileObjectList(names);

            //outputs contents of arraylist/linkedlist
            Console.WriteLine("\n\nMobileObject ArrayList:------------------------------------------------------------------------------\n\n"
                + arrayList.PrintAllForward());
            Console.WriteLine("\n\nMobileObject LinkedList:------------------------------------------------------------------------------\n\n"
                + linkedList.PrintAllForward());

            Console.ReadKey();
        }


        // generates an ArrayList<MobileObject> of size n given an array of n strings
        public static ArrayList<MobileObject> GenerateMobileObjectArray(string[] names)
        {
            ArrayList<MobileObject> arrayList = new ArrayList<MobileObject>(names.Length);
            Random rnd = new Random();
            // Iterates through the array populating it with MobileObjects (NPCs on even indices, Vehicles on odd)
            // uses iterator of for loop to assign ID and to pull names from names array
            // uses Random object to generate dimensions in range from 1-100
            for (int i = 0; i < names.Length; i++)
            {
                if (i % 2 == 0)
                {
                    arrayList.Append(new NPC(names[i], i, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                }
                else
                {
                    arrayList.Append(new Vehicle(names[i], i, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                }
            }
            return arrayList;
        }

        // generates an LinkedList<MobileObject> of size n given an array of n strings
        public static DoublyLinkedList<MobileObject> GenerateMobileObjectList(string[] names)
        {
            DoublyLinkedList<MobileObject> linkedList = new DoublyLinkedList<MobileObject>();
            Random rnd = new Random();
            // Iterates through the list populating it with MobileObjects (NPCs on odd indices, Vehicles on even)
            // uses iterator of for loop to assign ID and to pull names from names array in reverse
            // uses Random object to generate dimensions in range from 1-100
            for (int i = 0; i < names.Length; i++)
            {
                if (i % 2 == 1)
                {
                    linkedList.AddLast(new NPC(names[names.Length - 1 - i], i, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                }
                else
                {
                    linkedList.AddLast(new Vehicle(names[names.Length - 1 - i], i, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99, 1 + rnd.NextDouble() * 99));
                }
            }
            return linkedList;
        }

        // Tests and outputs results for various methods of ArrayList
        public static void ArrayListTests()
        {
            Console.WriteLine("ArrayList Tests-------------------------------------------------------");
            ArrayList<int> arrayList;

            // Testing Constructors
            Console.WriteLine("\nAttempt to create Arraylist with capacity 0:");
            try
            {
                arrayList = new ArrayList<int>();
                Console.WriteLine("Attempt Succeeded");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\nAttempt to create Arraylist with negative capacity:");
            try
            {
                arrayList = new ArrayList<int>(-2);
                Console.WriteLine("Attempt Succeeded");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine("\nAttempt to create Arraylist with posative capacity:");
            try
            {
                arrayList = new ArrayList<int>(10);
                Console.WriteLine("Attempt Succeeded");
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.WriteLine(e.Message);
            }

            // Testing Grow()
            Console.WriteLine("\n\nAttempt to grow Arraylist with capacity 0:");
            arrayList = new ArrayList<int>();
            Console.WriteLine("Capacity before growth: " + arrayList.Capacity());
            arrayList.Grow();
            Console.WriteLine("Capacity after growth: " + arrayList.Capacity());

            Console.WriteLine("\nAttempt to grow Arraylist with posative capacity:");
            arrayList = new ArrayList<int>(5);
            Console.WriteLine("Capacity before growth: " + arrayList.Capacity());
            arrayList.Grow();
            Console.WriteLine("Capacity after growth: " + arrayList.Capacity());

            // Testing Append()
            Console.WriteLine("\n\nAttempt to append to list:");
            arrayList = new ArrayList<int>(1);
            Console.WriteLine("Element count before append: " + arrayList.Count());
            arrayList.Append(5);
            Console.WriteLine("Element count after append: " + arrayList.Count());

            Console.WriteLine("\nAttempt to append to list at capactiy:");
            Console.WriteLine("Capacity before append: " + arrayList.Capacity());
            Console.WriteLine("Element count before append: " + arrayList.Count());
            arrayList.Append(10);
            Console.WriteLine("Capacity after append: " + arrayList.Capacity());
            Console.WriteLine("Element count after append: " + arrayList.Count());

            // Testing PrintAllForward() and PrintAllReverse()
            arrayList.Append(3);
            arrayList.Append(4);
            arrayList.Append(-7);
            Console.WriteLine("\n\nAttempt to print all elements forward:");
            Console.Write(arrayList.PrintAllForward());

            Console.WriteLine("\nAttempt to print all elements in reverse:");
            Console.Write(arrayList.PrintAllReverse());

            // Testing DeleteLast()
            Console.WriteLine("\n\nAttempt to delete last element:");
            Console.WriteLine("Capacity before delete: " + arrayList.Capacity());
            Console.WriteLine("Element count before delete " + arrayList.Count());
            Console.Write(arrayList.PrintAllForward());
            arrayList.DeleteLast();
            Console.WriteLine("Capacity after delete: " + arrayList.Capacity());
            Console.WriteLine("Element count after delete: " + arrayList.Count());
            Console.Write(arrayList.PrintAllForward());

            Console.WriteLine("\nAttempt to delete last element from ArrayList with no elements");
            arrayList = new ArrayList<int>(4);
            Console.WriteLine("Capacity before delete: " + arrayList.Capacity());
            Console.WriteLine("Element count before delete " + arrayList.Count());
            arrayList.DeleteLast();
            Console.WriteLine("Capacity after delete: " + arrayList.Capacity());
            Console.WriteLine("Element count after delete: " + arrayList.Count());

            //Testing DeleteAll()
            Console.WriteLine("\n\nAttempt to delete all elements from ArrayList");
            arrayList.Append(5);
            arrayList.Append(3);
            arrayList.Append(-2);
            arrayList.Append(7);
            Console.WriteLine("Capacity before delete: " + arrayList.Capacity());
            Console.WriteLine("Element count before delete " + arrayList.Count());
            arrayList.DeleteAll();
            Console.WriteLine("Capacity after delete: " + arrayList.Capacity());
            Console.WriteLine("Element count after delete: " + arrayList.Count());

            Console.WriteLine("\nAttempt to delete all elements from empty ArrayList");
            Console.WriteLine("Capacity before delete: " + arrayList.Capacity());
            Console.WriteLine("Element count before delete " + arrayList.Count());
            arrayList.DeleteAll();
            Console.WriteLine("Capacity after delete: " + arrayList.Capacity());
            Console.WriteLine("Element count after delete: " + arrayList.Count());
        }

        // Tests and outputs results for various methods of DoublyLinkedList
        public static void DoublyLinkedListTests()
        {
            Console.WriteLine("\n\nDoublyLinkedList Tests-------------------------------------------------------");
            DoublyLinkedList<int> linkedList;

            // Testing Constructors
            Console.WriteLine("\nAttempt to create DoublyLinkedList:");
            try
            {
                linkedList = new DoublyLinkedList<int>();
                Console.WriteLine("Attempt Succeeded");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // Testing PrintAllForward() and PrintAllReverse()
            linkedList = new DoublyLinkedList<int>();
            linkedList.AddFirst(2);
            linkedList.AddFirst(4);
            linkedList.AddFirst(-7);
            linkedList.AddFirst(22);
            Console.WriteLine("Attempt to print all elements:");
            Console.WriteLine("Number of elements in list: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            Console.WriteLine("\nAttempt to print all elements in reverse:");
            Console.WriteLine("Number of elements in list: " + linkedList.Length());
            Console.Write(linkedList.PrintAllReverse());

            // Testing AddFront()
            Console.WriteLine("\n\nAttempt to add to front of empty LinkedList:");
            linkedList = new DoublyLinkedList<int>();
            Console.WriteLine("Number of elements in list before add: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());
            linkedList.AddFirst(2);
            Console.WriteLine("Number of elements in list after add: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            Console.WriteLine("\nAttempt to add to front of LinkedList:");
            Console.WriteLine("Number of elements in list before add: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());
            linkedList.AddFirst(5);
            Console.WriteLine("Number of elements in list after add: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            // Testing AddLast()
            Console.WriteLine("\n\nAttempt to add to end of empty LinkedList:");
            linkedList = new DoublyLinkedList<int>();
            Console.WriteLine("Number of elements in list before add: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());
            linkedList.AddLast(7);
            Console.WriteLine("Number of elements in list after add: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            Console.WriteLine("\nAttempt to add to end of LinkedList:");
            Console.WriteLine("Number of elements in list before add: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());
            linkedList.AddLast(-2);
            Console.WriteLine("Number of elements in list after add: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            // Testing DeleteFirst()
            Console.WriteLine("\n\nAttempt to delete first element of LinkedList:");
            Console.WriteLine("Number of elements in list before delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());
            linkedList.DeleteFirst();
            Console.WriteLine("Number of elements in list after delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            Console.WriteLine("\nAttempt to delete first element of LinkedList with one element:");
            Console.WriteLine("Number of elements in list before delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());
            linkedList.DeleteFirst();
            Console.WriteLine("Number of elements in list after delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            Console.WriteLine("\nAttempt to delete first element of LinkedList with no elements:");
            Console.WriteLine("Number of elements in list before delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());
            linkedList.DeleteFirst();
            Console.WriteLine("Number of elements in list after delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            // Testing DeleteLast()
            linkedList.AddFirst(4);
            linkedList.AddFirst(2);
            Console.WriteLine("\n\nAttempt to delete last element of LinkedList:");
            Console.WriteLine("Number of elements in list before delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());
            linkedList.DeleteLast();
            Console.WriteLine("Number of elements in list after delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            Console.WriteLine("\nAttempt to delete last element of LinkedList with one element:");
            Console.WriteLine("Number of elements in list before delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());
            linkedList.DeleteLast();
            Console.WriteLine("Number of elements in list after delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            Console.WriteLine("\nAttempt to delete last element of LinkedList with no elements:");
            Console.WriteLine("Number of elements in list before delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());
            linkedList.DeleteLast();
            Console.WriteLine("Number of elements in list after delete: " + linkedList.Length());
            Console.Write(linkedList.PrintAllForward());

            // Testing DeleteAll()
            linkedList.AddFirst(2);
            linkedList.AddFirst(4);
            linkedList.AddFirst(-7);
            Console.WriteLine("\n\nAttempt to delete all elements of LinkedList:");
            Console.WriteLine("Number of elements in list before delete: " + linkedList.Length());
            linkedList.DeleteAll();
            Console.WriteLine("Number of elements in list after delete: " + linkedList.Length());
            Console.WriteLine("\nAttempt to delete all elements of empty LinkedList:");
            Console.WriteLine("Number of elements in list before delete: " + linkedList.Length());
            linkedList.DeleteAll();
            Console.WriteLine("Number of elements in list after delete: " + linkedList.Length());
        }
    }
}
