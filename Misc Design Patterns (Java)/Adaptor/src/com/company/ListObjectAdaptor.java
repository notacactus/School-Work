package com.company;

import java.util.ArrayList;

// Object adaptor from ArrayList to ListInterface(GoF List)
public class ListObjectAdaptor implements ListInterface {
    //Object instance of ArrayList, used to call legacy methods
    ArrayList list;

    // Constructor initializes list
    public ListObjectAdaptor() {
        list = new ArrayList();
    }

    // All methods perform as described in ListInterface
    // All methods function by calling equivalent method on legacy ArrayList object
    @Override
    public int count() {
        return list.size();
    }

    @Override
    public Object get(int index) {
        return list.get(index);
    }

    @Override
    public Object first() {
        return list.get(0);
    }

    @Override
    public Object last() {
        return list.get(list.size() - 1);
    }

    @Override
    public boolean include(Object obj) {
        return list.contains(obj);
    }

    @Override
    public void append(Object obj) {
        list.add(obj);
    }

    @Override
    public void prepend(Object obj) {
        list.add(0, obj);
    }

    @Override
    public void delete(Object obj) {
        list.remove(obj);
    }

    @Override
    public void deleteLast() {
        list.remove(list.size() - 1);
    }

    @Override
    public void deleteFirst() {
        list.remove(0);
    }

    @Override
    public void deleteAll() {
        list.clear();
    }
}