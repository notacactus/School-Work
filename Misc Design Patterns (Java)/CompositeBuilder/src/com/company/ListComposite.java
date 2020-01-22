package com.company;

import java.util.*;

// List that can contain items and other lists, implemented using Composite Pattern
public class ListComposite implements ListComponent {
    private ArrayList<ListComponent> list;  // list acting as base for Composite list structure

    // Constructor
    public ListComposite() {
        list = new ArrayList<ListComponent>();
    }

    // Prints brackets and in them between calls printValue recursively on each element of the list
    @Override
    public void printValue() {
        System.out.print("( ");
        for (ListComponent component : list) {
            if (component != null)
                component.printValue();
        }
        System.out.print(") ");
    }

    // Adds a given ListComponent at specified index, shifts other elements forward if needed
    // Parameters:
    //  int index           - index to add at
    //  ListComponent child - element to add to list
    @Override
    public void addChild(int index, ListComponent child) {
        if (child != null)
            list.add(index, child);
    }

    // Adds the element at specified index, shifts other elements back if needed
    // Parameters:
    //  int index           - index to remove at
    @Override
    public void removeChild(int index) {
        list.remove(index);
    }

    // Returns the ListComponent at specified index
    // Parameters:
    //  int index           - index of element to retrieve
    @Override
    public ListComponent getChild(int index) {
        return list.get(index);
    }
}
