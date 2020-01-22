package com.company;

import java.util.ArrayList;

// Class adaptor from ArrayList to ListInterface(GoF List)
public class ListClassAdaptor extends ArrayList implements ListInterface {

    // Constructor calls constructor of parent(legacy) class
    public ListClassAdaptor() {
        super();
    }

    // All methods perform as described in ListInterface
    // All methods function by calling equivalent methods inherited from Arraylist, inherited get(int index) method already functions as expected so is not overridden here
    @Override
    public int count() {
        return size();
    }

    @Override
    public Object first() {
        return get(0);
    }

    @Override
    public Object last() {
        return get(size() - 1);
    }

    @Override
    public boolean include(Object obj) {
        return contains(obj);
    }

    @Override
    public void append(Object obj) {
        add(obj);
    }

    @Override
    public void prepend(Object obj) {
        add(0, obj);
    }

    @Override
    public void delete(Object obj) {
        remove(obj);
    }

    @Override
    public void deleteLast() {
        remove(size() - 1);
    }

    @Override
    public void deleteFirst() {
        remove(0);
    }

    @Override
    public void deleteAll() {
        clear();
    }
}
