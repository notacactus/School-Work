using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_2
{
    public class Node
    {
        public Node next;
        public Node previous;
        public Object data;
    }

    // Generic doubly linked list with head and tail
    class DoublyLinkedList<T>
    {
        private Node head;
        private Node tail;
        private int length;

        public DoublyLinkedList()
        {
            head = null;
            tail = null;
            length = 0;
        }

        public int Length()
        {
            return length;
        }

        // Adds object to beginning of list
        public void AddFirst(T data)
        {
            // If head null, sets head and tail to object
            if (head == null)
            {
                head = new Node();
                head.data = data;
                tail = head;
            }
            // Otherwise links object to head then changes head to object
            else
            {
                Node toAdd = new Node();
                toAdd.data = data;
                toAdd.next = head;
                head.previous = toAdd;
                head = toAdd;
            }
            length++;
        }

        // adds object to end of list
        public void AddLast(T data)
        {
            // If head null, sets head and tail to object
            if (head == null)
            {
                head = new Node();
                head.data = data;
                tail = head;
            }
            // Otherwise links object to tail then changes tail to object
            else
            {
                Node toAdd = new Node();
                toAdd.data = data;
                toAdd.previous = tail;
                tail.next = toAdd;
                tail = toAdd;
            }
            length++;
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
                length--;
            }
            // if list has more than one object, moves head to second element of list then removes link to first object
            else
            {
                head = head.next;
                head.previous = null;
                length--;
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
                length--;
            }
            // if list has more than one object, moves tail to second-last element of list then removes link to last object
            else
            {
                tail.previous.next = null;
                tail = tail.previous;
                length--;
            }
        }

        // returns a sting containing all of the elements in the list in order
        public string PrintAllForward()
        {
            string output = "";
            int count = 0;
            Node current = head;
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
            int count = length-1;
            Node current = tail;
            while (current != null)
            {
                output += "Element " + count + ": " + current.data + "\n";
                current = current.previous;
                count--;
            }
            return output;
        }

        // deletes all elements of list by setting head and tail to null
        public void DeleteAll()
        {
            if (head != null)
            {
                Node current = head;
                while (current.next != null)
                {
                    current.data = null;
                    current = current.next;
                    current.previous.next = null;
                    current.previous = null;
                }
                current.data = null;
                current.previous = null;

                head = null;
                tail = null;
                length = 0;
            }
        }
    }
}
