using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_3
{
    class HashTable
    {
        string[] array;

        // Creates new hash table with fixed size, maximum size 50
        public HashTable(int size)
        {
            if (size < 0 || size > 50)
                throw new ArgumentOutOfRangeException("size");
            array = new string[size];
        }

        // Hashes a given item and adds it to the hash table
        public void AddItem(string element)
        {
            // Hashes item to find key then looks if position is free. If position is free or contains a duplicate then inserts at position, otherwise increases position counter quadratically and repeats until either a free space has been found or all posible positions have been checked
            int index = Hash(element);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[(index + i * i) % array.Length] == null || array[(index + i * i) % array.Length] == element)
                {
                    array[(index + i * i) % array.Length] = element;
                    return;
                }
            }
            // If no empty position was found throws exception
            throw new Exception("Insufficient Space in Hashtable");
        }

        // Uses quadratic probing to see if item is in table
        public bool FindItem(string element)
        {
            // hashes item to find key and looks at position. If the item is there returns true, if position is null returns false, otherwise increases position counter quadratically and repeats until either the item is found, a null space is found, or all posible positions have been checked
            int index = Hash(element);
            for (int i = 0; i < array.Length && array[(index + i * i) % array.Length] != null; i++)
            {
                if (array[(index + i * i) % array.Length] == element)
                {
                    return true;
                }
            }
            // If null value found or all positions have been found returns false
            return false;
        }

        // Hashes a given string by returning its length
        private int Hash(string element)
        {
            return element.Length;
        }
    }
}
