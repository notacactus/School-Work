/*  Program:    COIS 3020 Assignment 2
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-03-06
 *  Purpose:    
 *      Implements and tests an InRange function for augmented treap
 *      Implements/Augments and tests treap to support MinGap function and
 *      Implements and tests rope data structure
 *  Software/Language:
 *      C#
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_3020_Assignment_2
{
    public class Program
    {
        static Random V = new Random();

        static void Main(string[] args)
        {
            // Add comments before test functions to test structures individually
            RangeTests();
            MinGapTests();
            RopeTests();
            Console.ReadLine();
        }

        // Tests the functionality of the InRange function 
        static void RangeTests()
        {
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Testing Augmented Treap Range Functionality");
            AugmentedTreapRange<int> range = new AugmentedTreapRange<int>();
            // Generate/Display sample tree
            int[] values = new int[] { 50, 20, 70, 8, 33, 22, 49, 61, 57, 12, 30, 18, 89, 105, 76, 21 };
            foreach (int val in values)
                range.Add(val);
            range.Print();

            // Both bounds included
            Console.WriteLine("Items in range [22-61]: " + range.InRange(22, 61));
            // only right bound included
            Console.WriteLine("Items in range [9-50]: " + range.InRange(9, 50));
            // only left bound included
            Console.WriteLine("Items in range [57-71]: " + range.InRange(57, 71));
            // neither bound included
            Console.WriteLine("Items in range [25-35]: " + range.InRange(25, 35));
            // no items in range
            Console.WriteLine("Items in range [62-69]: " + range.InRange(62, 69));
            // no items in range, both bounds < all items
            Console.WriteLine("Items in range [1-5]: " + range.InRange(1, 5));
            // no items in range, both bounds > all items
            Console.WriteLine("Items in range [110-120]: " + range.InRange(110, 120));
            // same bounds
            Console.WriteLine("Items in range [8-8]: " + range.InRange(8, 8));
            // same bounds no item
            Console.WriteLine("Items in range [9-9]: " + range.InRange(9, 9));
            // bounds reverse order
            Console.WriteLine("Items in range [50-20]: " + range.InRange(50, 20));
            // negative bound
            Console.WriteLine("Items in range [50-20]: " + range.InRange(-10, 20));
            Console.WriteLine(new string('-', 100));
        }

        // Tests functionality of the MinGap function and the ability of the tree to maintain augmented data
        static void MinGapTests()
        {
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Testing Augmented Treap MinGap Functionality");
            MinGapTreap minGap = new MinGapTreap();
            // Generate/Display sample tree
            int[] values = new int[] { 50, 20, 70, 8, 33, 22, 48, 61, 57, 12, 30, 18, 89, 105, 76, 27 };
            foreach (int val in values)
                minGap.Add(val);
            minGap.Print();
            Console.WriteLine("Minimum Gap: " + minGap.MinGap());
            Console.WriteLine(new string('-', 100));
            // Remove no change MinGap
            minGap.Remove(50);
            Console.WriteLine("Removing 50");
            minGap.Print();
            Console.WriteLine("Minimum Gap: " + minGap.MinGap());
            Console.WriteLine(new string('-', 100));
            // Remove increase MinGap
            minGap.Remove(20);
            Console.WriteLine("Removing 20");
            minGap.Print();
            Console.WriteLine("Minimum Gap: " + minGap.MinGap());
            Console.WriteLine(new string('-', 100));
            // Remove increase MinGap
            minGap.Remove(30);
            Console.WriteLine("Removing 30");
            minGap.Print();
            Console.WriteLine("Minimum Gap: " + minGap.MinGap());
            Console.WriteLine(new string('-', 100));

            // Add no change MinGap
            minGap.Add(99);
            Console.WriteLine("Adding 99");
            minGap.Print();
            Console.WriteLine("Minimum Gap: " + minGap.MinGap());
            Console.WriteLine(new string('-', 100));
            // Add reduce MinGap
            minGap.Add(68);
            Console.WriteLine("Adding 68");
            minGap.Print();
            Console.WriteLine("Minimum Gap: " + minGap.MinGap());
            Console.WriteLine(new string('-', 100));
            // Add reduce MinGap
            minGap.Add(28);
            Console.WriteLine("Adding 28");
            minGap.Print();
            Console.WriteLine("Minimum Gap: " + minGap.MinGap());
            Console.WriteLine(new string('-', 100));

            MinGapTreap minGap2 = new MinGapTreap();
            // Only a single element
            Console.WriteLine("Single element treap:");
            minGap2.Add(50);
            minGap2.Print();
            Console.WriteLine(new string('-', 100));
            // Only 2 elements
            Console.WriteLine("Two element treap:");
            minGap2.Add(10);
            minGap2.Print();
            Console.WriteLine(new string('-', 100));

        }

        // Tests various functionality of ropes
        static void RopeTests()
        {
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Testing Rope Functionality");

            // Constructor for long string
            Rope rope = new Rope("aaaaabbbbbcccccdddddeeeeefffffggggghhhhhiiiiijjjjjkkkkklllllmmmmmnnnnnooooopppppqqqqqrrrrrssssstttttuuuuuvvvvvwwwwwxxxxxyyyyyzzzzz");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine(new string('-', 100));

            // Concatenation normal case
            Rope rope2 = Rope.Concatenate(new Rope("abcdefghijklmnopqrstuvwxyz"), new Rope("zyxwvutsrqponmlkjihgfedcba"));
            Console.Write("Rope Value: ");
            rope2.Print();
            Console.WriteLine("Rope Structure: ");
            rope2.PrintStructure();
            Console.WriteLine(new string('-', 100));

            // Concatenation 2 small ropes
            Rope rope3 = Rope.Concatenate(new Rope("12345"), new Rope("54321"));
            Console.Write("Rope Value: ");
            rope3.Print();
            Console.WriteLine("Rope Structure: ");
            rope3.PrintStructure();
            Console.WriteLine(new string('-', 100));

            // Concatenation, rope with small right child + small rope
            Rope rope4 = Rope.Concatenate(new Rope("123456789012345"), new Rope("abcd"));
            Console.Write("Rope Value: ");
            rope4.Print();
            Console.WriteLine("Rope Structure: ");
            rope4.PrintStructure();
            Console.WriteLine(new string('-', 100));

            Console.Write("Rope Value: ");
            rope.Print();
            // Substring overlapping nodes
            Console.WriteLine("\nSubstring [23,47]: " + rope.Substring(23, 47));
            // substring single node
            Console.WriteLine("Substring [23,25]: " + rope.Substring(23, 25));
            // substring single char
            Console.WriteLine("Substring [99,99]: " + rope.Substring(99, 99));
            // substring index 0 bound
            Console.WriteLine("Substring [0,32]: " + rope.Substring(0, 32));
            // substring max index bound
            Console.WriteLine("Substring [0,32]: " + rope.Substring(102, 129));
            // substring index < 0
            Console.WriteLine("Substring [-1,10]: " + (rope.Substring(-1, 10) == null ? "No substring found" : "Substring found"));
            // substring > max
            Console.WriteLine("Substring [50,150]: " + (rope.Substring(50, 150) == null ? "No substring found" : "Substring found"));
            // start index > end index
            Console.WriteLine("Substring [20,10]: " + (rope.Substring(20, 10) == null ? "No substring found" : "Substring found"));

            // ChatAt index within range
            Console.WriteLine("\nChar at index 25: " + rope.CharAt(25));
            // ChatAt index 0
            Console.WriteLine("Char at index 25: " + rope.CharAt(0));
            // ChatAt max index
            Console.WriteLine("Char at index 25: " + rope.CharAt(129));
            // ChatAt index < 0
            Console.WriteLine("Char at index 25: " + (rope.CharAt(130) == '\0' ? "No char found" : "char found"));
            // ChatAt index > max
            Console.WriteLine("Char at index 25: " + (rope.CharAt(-1) == '\0' ? "No char found" : "char found"));

            // Find string accross multiple nodes
            Console.WriteLine("\nFind string \"dddee\" index: " + rope.Find("dddee"));
            // Find string in single node
            Console.WriteLine("Find string \"dddee\" index: " + rope.Find("ggghh"));
            // find string not in rope
            Console.WriteLine("Find string \"dddee\" index: " + rope.Find("xxxxxx"));
            // find empty string
            Console.WriteLine("Find string \"dddee\" index: " + rope.Find(""));
            // find string exceeding rope capacity
            Console.WriteLine("Find string \"dddee\" index: " + rope.Find(new string('a', 200)));

            // Splitting rope at end node 
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Splitting rope at index 69");
            Rope rope5 = rope.Split(69);
            Console.Write("Left Split Rope Value: ");
            rope.Print();
            Console.WriteLine("Left Split Rope Structure: ");
            rope.PrintStructure();
            Console.Write("Right Split Rope Value: ");
            rope5.Print();
            Console.WriteLine("Right Split Rope Structure: ");
            rope5.PrintStructure();

            // splitting rope mid node
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Splitting rope at index 48");
            rope5 = rope.Split(48);
            Console.Write("Left Split Rope Value: ");
            rope.Print();
            Console.WriteLine("Left Split Rope Structure: ");
            rope.PrintStructure();
            Console.Write("Right Split Rope Value: ");
            rope5.Print();
            Console.WriteLine("Right Split Rope Structure: ");
            rope5.PrintStructure();

            // splitting rope at end of rope
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Splitting rope at index 48");
            rope5 = rope.Split(48);
            Console.Write("Left Split Rope Value: ");
            rope.Print();
            Console.WriteLine("Left Split Rope Structure: ");
            rope.PrintStructure();
            Console.Write("Right Split Rope Value: ");
            rope5.Print();
            Console.WriteLine("Right Split Rope Structure: ");
            rope5.PrintStructure();

            // splitting rope at start of rope (index - 1)
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Splitting rope at index -1");
            rope5 = rope.Split(-1);
            Console.Write("Left Split Rope Value: ");
            rope.Print();
            Console.WriteLine("Left Split Rope Structure: ");
            rope.PrintStructure();
            Console.Write("Right Split Rope Value: ");
            rope5.Print();
            Console.WriteLine("Right Split Rope Structure: ");
            rope5.PrintStructure();

            rope = rope5;
            // splitting rope at index > rope length
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Splitting rope at index 100");
            rope5 = rope.Split(100);
            Console.Write("Left Split Rope Value: ");
            rope.Print();
            Console.WriteLine("Left Split Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine("Right Split Rope Value: " + (rope5 == null ? "null" : " not null"));

            // splitting rope at index < start
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Splitting rope at index -10");
            rope5 = rope.Split(-10);
            Console.Write("Left Split Rope Value: ");
            rope.Print();
            Console.WriteLine("Left Split Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine("Right Split Rope Value: " + (rope5 == null ? "null" : " not null"));



            
            Console.WriteLine(new string('-', 100));
            rope = new Rope("aaaaabbbbbcccccdddddeeeeefffffggggghhhhhiiiiijjjjjkkkkklllllmmmmmnnnnnooooopppppqqqqqrrrrrssssstttttuuuuuvvvvvwwwwwxxxxxyyyyyzzzzz");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();

            // deleting from center of rope crossing multiple nodes
            Console.WriteLine(new string('-', 100));
            if (rope.Delete(30, 65))
                Console.WriteLine("Deleted substring [30,65]");
            else
                Console.WriteLine("Failed to delete substring [30,65]");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine(new string('-', 100));

            // deleting single character
            if (rope.Delete(90, 90))
                Console.WriteLine("Deleted substring [90,90]");
            else
                Console.WriteLine("Failed to delete substring [90,90]");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine(new string('-', 100));

            // deleting from middle of string
            if (rope.Delete(50, 68))
                Console.WriteLine("Deleted substring [50,68]");
            else
                Console.WriteLine("Failed to delete substring [50,68]");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine(new string('-', 100));
            
            // deleting from end of string
            if (rope.Delete(48, 73))
                Console.WriteLine("Deleted substring [48,73]");
            else
                Console.WriteLine("Failed to delete substring [48,73]");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine(new string('-', 100));
            
            // deleting from start of string
            if (rope.Delete(0, 15))
                Console.WriteLine("Deleted substring [0,15]");
            else
                Console.WriteLine("Failed to delete substring [0,15]");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine(new string('-', 100));

            // deleting with second bound exceding string size
            if (rope.Delete(25, 100))
                Console.WriteLine("Deleted substring [25,100]");
            else
                Console.WriteLine("Failed to delete substring [25,100]");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine(new string('-', 100));

            // deleting with first bound exceding string size
            if (rope.Delete(-10, 10))
                Console.WriteLine("Deleted substring [-10,10]");
            else
                Console.WriteLine("Failed to delete substring [-10,10]");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine(new string('-', 100));

            // inserting at center of rope
            if (rope.Insert("ABCDEFGHIJKLMNO", 11))
                Console.WriteLine("Inserted \"ABCDEFGHIJKLMNO\" at index 11");
            else
                Console.WriteLine("Failed to insert at index 11");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine(new string('-', 100));


            // inserting samll string inside other string
            if (rope.Insert("QRS", 28))
                Console.WriteLine("Inserted \"QRS\" at index 28");
            else
                Console.WriteLine("Failed to insert at index 28");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine(new string('-', 100));

            // inserting at start of rope
            if (rope.Insert("TUVW", 0))
                Console.WriteLine("Inserted \"TUVM\" at index 0");
            else
                Console.WriteLine("Failed to insert at index 0");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine(new string('-', 100));

            // inserting at end of rope
            if (rope.Insert("XYZ", 54))
                Console.WriteLine("Inserted \"XYZ\" at index 54");
            else
                Console.WriteLine("Failed to insert at index 54");
            Console.Write("Rope Value: ");
            rope.Print();
            Console.WriteLine("Rope Structure: ");
            rope.PrintStructure();
            Console.WriteLine(new string('-', 100));

            // inserting before start of rope
            if (rope.Insert("AAAAA", -10))
                Console.WriteLine("Inserted \"AAAA\" at index -10");
            else
                Console.WriteLine("Failed to insert at index -10");
            Console.Write("Rope Value: ");
            rope.Print();


            // inserting after end of rope
            if (rope.Insert("ZZZZZ", 100))
                Console.WriteLine("Inserted \"ABCDEFGHIJKLMNO\" at index 100");
            else
                Console.WriteLine("Failed to insert at index 100");
            Console.Write("Rope Value: ");
            rope.Print();


            /*
            Console.WriteLine(rope.CharAt(89));
            Console.WriteLine(rope.Substring(20, 55));
            Console.WriteLine(rope.Find("aaaaabbbbbbb"));
            //Rope split = rope.Split(25);
            rope.Insert("123456789012345", 0);
            rope.PrintStructure();
            rope.Delete(0, 170);
            rope.PrintStructure();
            rope.Print();
            rope.Insert("123456789012345", 0);
            rope.PrintStructure();
            //split.PrintStructure();
            */
        }
    }
}
