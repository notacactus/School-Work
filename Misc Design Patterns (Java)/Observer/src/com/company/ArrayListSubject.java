package com.company;
import java.util.*;

// concrete subject arraylist, supports append and delete, notify on delete
public class ArrayListSubject extends Subject{
    ArrayList list;     // backing list

    // constructor calls super constructor and instantiates list
    public ArrayListSubject(){
        super();
        list = new ArrayList();
    }

    // appends item to list
    void append(Object obj){
        list.add(obj);
    }

    // removes item from list and updates observers
    void delete(Object obj){
        list.remove(obj);
        notifyObs();
    }
}
