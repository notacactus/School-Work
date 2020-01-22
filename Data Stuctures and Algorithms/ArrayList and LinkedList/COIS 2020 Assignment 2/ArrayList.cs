using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_2
{
    // Generic list with backend array 
    class ArrayList<T>
    {
        private T[] array;
        private int count;  // number of items in the array

        // no argument constructor, sets array size to 0
        public ArrayList()
        {
            array = new T[0];
            count = 0;
        }
        // single argument constructor, takes int that determines size of array, throws exception if size is negative
        public ArrayList(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size");
            array = new T[size];
            count = 0;
        }

        // Returns number of items in array and capacity respectively
        public int Count()
        {
            return count;
        }
        public int Capacity()
        {
            return array.Length;
        }

        // Takes int and returns item at that index, throws exception if index < 0 or >= count
        public T GetItem(int index)
        {
            if (index >= count || index < 0)
                throw new IndexOutOfRangeException("index");
            return array[index];
        }

        // Grows array
        public void Grow()
        {
            // if size = 0 creates new array of size 1
            if (array.Length == 0)
                array = new T[1];
            // otherwise creates new array double previous size and copies current array over
            else
            {
                T[] tempArray = new T[array.Length * 2];
                for (int i = 0; i < array.Length; i++)
                    tempArray[i] = array[i];
                array = tempArray;
            }
        }

        // appends item to end of array, if array is too small calls grow first
        public void Append(T value)
        {
            if (count == array.Length)
                Grow();
            array[count] = value;
            count++;
        }

        // deletes last element of array, if empty does nothing
        public void DeleteLast()
        {
            if (count > 0)
            {
                array[count - 1] = default(T);
                count--;
            }
        }

        // returns a sting containing all of the elements in the array in order
        public string PrintAllForward()
        {
            string output = "";
            for (int i = 0; i < count; i++)
            {
                output += "Element " + i + ": " + array[i] + "\n";
            }
            return output;
        }

        // returns a sting containing all of the elements in the array in reverse order
        public string PrintAllReverse()
        {
            string output = "";
            for (int i = count - 1; i >= 0; i--)
            {
                output += "Element " + i + ": " + array[i] + "\n";
            }
            return output;
        }

        // deletes all elements of array by setting all values to default and resetting count.
        public void DeleteAll()
        {
            for (int i = 0; i < count; i++)
            {
                array[i] = default(T);
            }
            count = 0;
        }
    }
}
