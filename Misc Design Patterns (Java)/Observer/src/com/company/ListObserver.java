package com.company;

// Concrete observer to observe List Subject
public class ListObserver extends Observer {
    String name;        // name to identify Observer

    // Constructor calls super constructor to attach subject and assigns given name
    public ListObserver(String name, Subject subj){
        super(subj);
        this.name = name;
    }

    // update identifies observer by name and prints "an item being deleted"
    @Override
    public void update() {
        System.out.println("Observer \"" + name + "\": an item is being deleted");
    }
}
