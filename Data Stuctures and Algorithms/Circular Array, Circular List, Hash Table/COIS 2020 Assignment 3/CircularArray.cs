using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_3
{
    class CircularArray<T>
    {
        private T[] array;
        private int queueFront;
        private int queueRear;
        private int count;

        // Default Constructor
        public CircularArray()
        {
            array = new T[0];
            queueFront = 0;
            queueRear = 0;
            count = 0;
        }

        // Constructor that takes starting size of array
        public CircularArray(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException("size");
            array = new T[size];
            queueFront = 0;
            queueRear = 0;
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
        
        // Adds item after last current item, grows the array beforehand if array is full, resets rear position tracker to 0 if passes max
        public void AddBack(T value)
        {
            if (count == array.Length)
                Grow();
            array[queueRear] = value;
            queueRear++;
            queueRear %= array.Length;
            count++;
        }

        // Removes last item in list, sets rear postion tracker to max if passes 0
        public T RemoveBack()
        {
            if (count > 0)
            {
                queueRear--;
                if (queueRear < 0)
                    queueRear += array.Length;
                T sample = array[queueRear];
                array[queueRear] = default(T);
                count--;
                return sample;
            }
            return default(T);
        }

        // Removes first item in list, sets front postion tracker to 0 if passes max
        public T RemoveFront() 
        {
            if (count > 0)
            {
                T sample = array[queueFront];
                array[queueFront] = default(T);
                queueFront++;  
                if (queueFront >= array.Length)
                    queueFront %= array.Length;
                count--;
                return sample;
            }
            return default(T);
        }
        
        // Creates a new array twice previous size and moves elements of new array into it, maintains ordering with element 0 being previous front and last element being previous rear
        private void Grow()
        {
            T[] temp = new T[array.Length * 2];

            for (int i = 0; i < count; i++)
                temp[i] = array[(queueFront + i) % array.Length];
            queueFront = 0;
            queueRear = count;
            array = temp;
        }

        // Deletes all elements by deleting first element until array is empty, then resets front and rear to 0
        public void DeleteAll()
        {
            while (count > 0)
            {
                array[queueFront] = default(T);
                queueFront++;
                if (queueFront >= array.Length)
                    queueFront %= array.Length;
                count--;
            }
            queueFront = 0;
            queueRear = 0;
        }

        // Basic push/pop/enqueue/dequeue functions, mostly reusing other functions and returning values for removed objects
        public void Push(T data)
        {
            AddBack(data);
        }
        public T Pop()
        {
            return RemoveBack();
        }
        public void Enqueue(T data)
        {
            AddBack(data);
        }
        public T Dequeue(T data)
        {
            return RemoveFront();
        }

        // Prints all elements in order of array index, outputs null for any empty element
        public void Print()
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (queueRear < queueFront? i < queueRear || i >= queueFront : i < queueRear && i >= queueFront)
                    Console.Write(array[i] + " ");
                else
                    Console.Write("null ");

            }
        }

        // returns a string containg all elements ordered from front to rear
        public override string ToString()
        {
            string output = "";
            for (int i = 0; i < count; i++)
                output += array[(queueFront + i) % array.Length] + " ";
            return output;
        }
    }
}
