using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_3
{
    // Generic doubly linked list with head and tail
    class DoublyLinkedList<T>
    {
        private Node<T> head;
        private Node<T> tail;
        private int count;

        public DoublyLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public int Count()
        {
            return count;
        }

        // Adds object to beginning of list
        public void AddFirst(T data)
        {
            // If head null, sets head and tail to object
            if (head == null)
            {
                head = new Node<T>();
                head.data = data;
                tail = head;
            }
            // Otherwise links object to head then changes head to object
            else
            {
                Node<T> toAdd = new Node<T>();
                toAdd.data = data;
                toAdd.next = head;
                head.previous = toAdd;
                head = toAdd;
            }
            count++;
        }

        // adds object to end of list
        public void AddLast(T data)
        {
            // If head null, sets head and tail to object
            if (head == null)
            {
                head = new Node<T>();
                head.data = data;
                tail = head;
            }
            // Otherwise links object to tail then changes tail to object
            else
            {
                Node<T> toAdd = new Node<T>();
                toAdd.data = data;
                toAdd.previous = tail;
                tail.next = toAdd;
                tail = toAdd;
            }
            count++;
        }

        // deletes first element of list
        public void DeleteFirst()
        {
            // if list empty does nothing
            if (head == null)
            {
            }
            // if list has one object sets head and tail to null
            else if (head.next == null)
            {
                head = null;
                tail = null;
                count--;
            }
            // if list has more than one object, moves head to second element of list then removes link to first object
            else
            {
                head = head.next;
                head.previous = null;
                count--;
            }
        }

        // deletes last element of list
        public void DeleteLast()
        {
            // if list empty does nothing
            if (head == null)
            {
            }
            // if list has one object sets head and tail to null
            else if (head.next == null)
            {
                head = null;
                tail = null;
                count--;
            }
            // if list has more than one object, moves tail to second-last element of list then removes link to last object
            else
            {

                tail = tail.previous;
                tail.next = null;
                count--;
            }
        }

        // Basic push/pop/enqueue/dequeue functions, mostly reusing other functions and returning values for removed objects
        public void Push(T data)
        {
            AddFirst(data);
        }
        public T Pop()
        {
            if (head != null)
            {
                T temp = head.data;
                DeleteFirst();
                return temp; 
            }
            return default(T);
        }
        public void Enqueue(T data)
        {
            AddFirst(data);
        }
        public T Dequeue(T data)
        {
            if (head != null)
            {
                T temp = tail.data;
                DeleteLast();
                return temp; 
            }
            return default(T);
        }

        // returns a sting containing all of the elements in the list in order
        public string PrintAllForward()
        {
            string output = "";
            int count = 0;
            Node<T> current = head;
            while (current != null)
            {
                output += "Element " + count + ": " + current.data + "\n";
                current = current.next;
                count++;
            }
            return output;
        }

        // returns a sting containing all of the elements in the list in reverse order
        public string PrintAllReverse()
        {
            string output = "";
            int counter = count - 1;
            Node<T> current = tail;
            while (current != null)
            {
                output += "Element " + counter + ": " + current.data + "\n";
                current = current.previous;
                counter--;
            }
            return output;
        }

        // deletes all elements of list by calling deletelast until list is empty
        public void DeleteAll()
        {
            while (head != null)
            {
                DeleteLast();
            }
        }
    }
}
