using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_3
{
    class CircularLinkedList<T>
    {
        private Node<T> head;
        private Node<T> tail;
        private int count;

        public CircularLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        // Returns number of items in list
        public int Count
        {
            get { return count; }
        }

        // Adds object to beginning of list
        public void AddFirst(T data)
        {
            // If head null, sets head and tail to object
            if (head == null)
            {
                AddFirstNode(data);
            }
            // Otherwise links object to head then changes head to object
            else
            {
                InsertAfter(data, tail);
                head = tail.next;
            }
            count++;
        }

        // adds object to end of list
        public void AddLast(T data)
        {
            // If head null adds object as first node
            if (head == null)
            {
                AddFirstNode(data);
            }
            // Otherwise inserts object after tail then changes tail to object
            else
            {
                InsertAfter(data, tail);
                tail = tail.next;
            }
            count++;
        }

        // Adds the first node to the list, sets head and tail to object and links its next and previous to itself
        private void AddFirstNode(T data)
        {
            head = new Node<T>();
            head.data = data;
            head.next = head;
            head.previous = head;
            tail = head;
        }

        // inserts a given object as a node after a given node
        private void InsertAfter(T data, Node<T> previous)
        {
            Node<T> toAdd = new Node<T>();
            toAdd.data = data;
            toAdd.next = previous.next;
            toAdd.previous = previous;
            previous.next = toAdd;
            toAdd.next.previous = toAdd;
        }

        // deletes first element of list
        public void DeleteFirst()
        {
            // if list empty does nothing
            if (head == null)
            {
            }
            // if list has one object sets head and tail to null
            else if (head.next == head)
            {
                head = null;
                tail = null;
                count--;
            }
            // if list has more than one object, moves head to second element of list then removes link to first object
            else
            {
                head = head.next;
                head.previous = tail;
                tail.next = head;
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
            else if (head.next == head)
            {
                head = null;
                tail = null;
                count--;
            }
            // if list has more than one object, moves tail to second-last element of list then removes link to last object
            else
            {
                tail = tail.previous;
                tail.next = head;
                head.previous = tail;
                count--;
            }
        }

        // returns a sting containing all of the elements in the list in order
        public string PrintAllForward()
        {
            string output = "";
            Node<T> current = head;
            for (int i = 0; i < count; i++)
            {
                output += "Element " + i + ": " + current.data + "\n";
                current = current.next;
            }
            return output;
        }

        // returns a sting containing all of the elements in the list in reverse order
        public string PrintAllReverse()
        {
            string output = "";
            Node<T> current = tail;
            for (int i = count - 1; i >= 0; i--)
            {
                output += "Element " + i + ": " + current.data + "\n";
                current = current.previous;
            }
            return output;
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

        // deletes all elements of list by deleting last object until list is empty
        public void DeleteAll()
        {
            while (head != null)
            {
                DeleteLast();
            }
        }
    }
}
