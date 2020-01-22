package com.company;

// Item stored in Composite List, contains an integer
public class Item implements ListComponent {
    private int value;  // value stored in Item

    // Constructor
    // Parameters:
    //  int value - value to store in list
    public Item(int value){
        this.value = value;
    }

    // prints the value of the item, used in recursive printValue for ListComposite
    @Override
    public void printValue() {
        System.out.print(value + " ");
    }

    // Not implemented for Item
    @Override
    public void addChild(int index, ListComponent child) {

    }

    // Not implemented for Item
    @Override
    public void removeChild(int index) {

    }

    // Not implemented for Item, returns null
    @Override
    public ListComponent getChild(int index) {
        return null;
    }
}
