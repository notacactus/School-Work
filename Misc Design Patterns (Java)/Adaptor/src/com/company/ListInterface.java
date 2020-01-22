package com.company;

// GoF list interface, lists methods adaptor class must implement
public interface ListInterface {
    // returns number of elements in list
    int count();

    // returns element at given index
    Object get(int index);

    // returns first element
    Object first();

    // returns last element
    Object last();

    // return true if element is in list
    boolean include(Object obj);

    // adds element to end of list
    void append(Object obj);

    // adds element to front of list
    void prepend(Object obj);

    //removes element form list
    void delete(Object obj);

    //removes last element
    void deleteLast();

    //removes first element
    void deleteFirst();

    //removes all elements
    void deleteAll();
}
