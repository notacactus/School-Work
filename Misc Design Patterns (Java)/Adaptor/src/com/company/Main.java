/*  Program:    COIS 3040 Assignment 1 Question 1
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-02-03
 *  Purpose:    Implements and tests a Class Adaptor and Object Adaptor to adapt the Java ArrayList class to GoF List interface
 */

package com.company;

public class Main {

    // Tests the ListClassAdaptor and ListObjectAdaptor classes
    public static void main(String[] args) {
        // Instantiates/initializes object of each class
        ListInterface listClass = new ListClassAdaptor();
        ListInterface listObject = new ListObjectAdaptor();
        // Calls testing helper method on each object to test all methods
        System.out.println("Testing Functionality of ListClassAdaptor\n");
        listInterfaceTests(listClass);

        System.out.println("-".repeat(50));

        System.out.println("Testing Functionality of ListObjectAdaptor\n");
        listInterfaceTests(listObject);
    }

    // Test function for List adaptor classes, Takes a class implementing ListInterface and tests all interface methods
    // Should produce identical results for any Adaptor
    public static void listInterfaceTests(ListInterface list) {
        // Testing count with 0 elements
        System.out.println("Empty List");
        printList(list);

        // Testing append and count with elements
        System.out.println("\nAdding elements \"ABC\", 1, \"CD\", 'E'");
        list.append("ABC");
        list.append(1);
        list.append("CD");
        list.append('E');
        printList(list);

        // Testing first and last functions
        System.out.println("\nFirst element of list: " + list.first());

        System.out.println("Last element of list: " + list.last());

        // Testing prepend
        System.out.println("\nAdding element to front of list: \"Q\"");
        list.prepend("Q");
        printList(list);

        // Testing include for item in and not in list
        System.out.println("\nSearching for element \"CD\" in list: " + (list.include("CD") ? "Found" : "Not Found"));

        System.out.println("Searching for element \"ZY\" in list: " + (list.include("ZY") ? "Found" : "Not Found"));

        // Testing delete and delete for non-existent element
        System.out.println("\nDeleting element from list: 1");
        list.delete(1);
        printList(list);

        System.out.println("Attempting to delete element from list: \"CBA\"");
        list.delete("CBA");
        printList(list);

        // Testing deleteFirst, deleteLast, and deleteAll
        System.out.println("\nDeleting first element from list");
        list.deleteFirst();
        printList(list);

        System.out.println("Deleting last element from list");
        list.deleteLast();
        printList(list);

        System.out.println("Deleting all elements from list");
        list.deleteAll();
        printList(list);
    }


    // testing helper function to display number of objects in list and output each object
    // tests count and get(int index) methods
    public static void printList(ListInterface list) {
        System.out.println("Number of elements in list: " + list.count());
        System.out.print("Elements in list: ");
        for (int i = 0; i < list.count(); i++) {
            System.out.print(list.get(i) + "  ");
        }
        System.out.println();
    }
}
