package com.company;

// Interface for Composite pattern list structure
public interface ListComponent {
    public void printValue();                               // Prints values
    public void addChild(int index, ListComponent child);   // adds child element to list
    public void removeChild(int index);                     // removes child element from list
    public ListComponent getChild(int index);               // returns child item at index

}
