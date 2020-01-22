using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_4
{
    class BinaryHeap<T> : IEnumerable where T : IComparable<T>
    {
        private T[] array;
        private int count; 
        public BinaryHeap(int size)
        {
            array = new T[size];
            count = 0;
        }
        public int Count
        {
            get { return count; }
        }
        // Get Item should really be private (needs to be public in the lab for demo purposes)
        public T GetItem(int index)
        {
            return array[index];
        }
        private void SetItem(int index, T value)
        {
            if (index >= array.Length)
                Grow(array.Length * 2);
            array[index] = value;
        }
        private void Grow(int newsize)
        {
            Array.Resize(ref array, newsize);
        }
        
        //returns left/right child of given position
        private int LeftChild(int pos)
        { return 2 * pos + 1; }
        private int RightChild(int pos)
        { return 2 * pos + 2; }
        //iterator so you can see how they work (sort of? It works but it's not really obvious how)
        public IEnumerator GetEnumerator()
        {
            for (int index = 0; index < count; index++) 
            {
                // Yield each element 
                yield return array[index];
            }
        }
        // Adds an item to the heap
        public void AddItem(T value)
        {
            // adds item at last position then sorts it up to correct poistion and increments count
            SetItem(count, value);
            MoveUp(count);
            count++;
        }
        // adds an array of items to the heap
        public void AddItems(T[] array)
        {
            // iterates through given array and adds each item to heap
            foreach (T item in array)
                AddItem(item);
        }
        // moves an item up to it's correct position
        public void MoveUp(int position)
        {
            // finds the parent position  
            int parent = ((position - 1) / 2);
            // if the position is not the first eleemnt and is smaller than its parent, then swaps them and calls MoveUp on the parent position 
            if (position > 0 && array[position].CompareTo(array[parent]) < 0)
            {
                Swap(position, parent);
                MoveUp(parent);
            }
        }

        //ExtractRoot (which is the same as extract min in our case)
        public T ExtractHead()
        {
            // check to make sure the heap isn't empty, if it is, return a 'null' or at least, default object
            if (count <= 0)
                return default(T);
            // stores the value in the head then moves the last element to replace it and decrements count
            T head = array[0];
            array[0] = array[count - 1];
            array[count - 1] = default(T);
            count--;
            // Sorts the new first element down to correct position
            MoveDown(0);
            // returns the saved head
            return head;
        }
        // swaps the items at given positions
        public void Swap(int position1, int position2)
        {
            T first = array[position1];
            array[position1] = array[position2];
            array[position2] = first;
        }
        //heapify should heapify the subtree for the element i that is the root of a subtree
       public void MoveDown(int root)
        {
            // while the root has at least one child
            while (LeftChild(root) < count)
            {
                // root*2+1 points to the left child
                int child = LeftChild(root);
                // take the highest of the left or right child
                if ((child + 1) < count && (array[child].CompareTo(array[child + 1]))>0)
                {
                    // then point to the right child instead
                    child = child + 1;
                }

                // out of max-heap order
                // swap the child with root if child is greater
                if ((array[root].CompareTo(array[child]))>0)
                {
                    T tmp = array[root];
                    array[root] =array[child];
                    array[child] = tmp;

                    // return the swapped root to test against
                    //  it's new children
                    root = child;
                }
                else {
                    return;
                }
            }
        }

        // builds a min heap out of the contents of array by calling minheapify on every eleemnt in the first half of array starting from the middle
        public void buildMinHeap()
        {
            int midPoint = array.Length / 2;
            for (int indexsOfLeaves = midPoint; indexsOfLeaves >= 0; indexsOfLeaves--)
            {
                MinHeapify(indexsOfLeaves);
            }
        }

        // Sorts the item at a given index down to the corect poistion in the heap
        public void MinHeapify(int position)
        {
            int lchild = LeftChild(position);
            int rchild = RightChild(position);
            int smallest = position; 
            // https://www.geeksforgeeks.org/binary-heap/  has an alternative implmentation using "smallest" rather than "largest"
            // https://stackoverflow.com/questions/15493056/min-heapify-method-min-heap-algorithm?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa  has the same sort of question in Java

            // if the left/right children exist finds which of the parent and children is the smallest
            if (count > lchild && (array[lchild].CompareTo(array[smallest])) < 0) 
            {
                smallest = lchild;
            }
            if (count > rchild && (array[rchild].CompareTo(array[smallest])) < 0) 
            {
                smallest = rchild;
            }
            // if the parent is not the smallest then swaps with the smallest and calls minheapify on the child that was swapped
            if (smallest != position)
            {
                Swap(position, smallest);
                MinHeapify(smallest);
            }

        }

        // Heapsort for already constructed heap
        // Returns array with contents of heap sorted from largest to smallest
        public T[] HeapSort()
        {
            // saves count and creates array to store sorted heap
            T[] temp = new T[count];
            int size = count;
            // repeatedely extracts the head and places it at end of the heap until there are no more items in the heap
            while (count > 0)
                array[count-1] = ExtractHead();
            // restores count then iterates through the sorted array and copies it to the new array
            count = size;
            for (int i = 0; i < count; i++)
                temp[i] = array[i];
            // rebuilds the heap from the sorted array then returns the new array
            buildMinHeap();
            return temp;
        }

        // Heapsort for array
        // takes an array, converts it to a heap, then heapsorts it and returns the result
        public static T[] HeapSort(T[] array)
        {
            BinaryHeap<T> heap = new BinaryHeap<T>(array.Length);
            heap.AddItems(array);
            return heap.HeapSort();
        }
    }
}
