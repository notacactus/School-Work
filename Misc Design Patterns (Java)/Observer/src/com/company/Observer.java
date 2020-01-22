package com.company;

// Observer abstract class for observer pattern
public abstract class Observer {
    // Constructor attaches self to subject
    public Observer(Subject subj){
        subj.attach(this);
    }
    public abstract void update();  // update to be called by observation subjects
}
