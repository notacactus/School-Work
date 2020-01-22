package com.company;

import java.util.*;

// Subject abstract class for observer pattern
public abstract class Subject {
    protected ArrayList<Observer> observers;    // list of observers

    // constructor instatiates list of observers, should be called with super in inheriting classes
    protected Subject(){
        observers = new ArrayList<Observer>();
    }

    // attaches an abserver by adding it to the list if not already in list
    public void attach(Observer obs) {
        if (!observers.contains(obs))
            observers.add(obs);
    }

    // detaches observer by removing from list
    public void detach(Observer obs) {
        observers.remove(obs);
    }

    // calls update on each observer in list
    protected void notifyObs() {
        for (Observer obs : observers) {
            obs.update();
        }
    }
}
