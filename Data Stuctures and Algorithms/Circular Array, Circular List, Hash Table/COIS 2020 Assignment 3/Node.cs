using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_2020_Assignment_3
{
    // Node used in DoublyLinkedList and CircularLinkedList
    public class Node<T>
    {
        public Node<T> next;
        public Node<T> previous;
        public T data;
    }
}
