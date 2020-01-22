/*  Program:    COIS 3040 Assignment 2 Question 3
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-03-03
 *  Purpose:   Implements a ArrayList as a subject using the observer pattern, notifies observers when delete is called
 */

package com.company;

public class Main {
    // Tests functionality of subject/observer
    public static void main(String[] args) {
        ArrayListSubject listSubj = new ArrayListSubject();                   // list subject
        ListObserver obs1 = new ListObserver("Observer 1", listSubj);   // list observer
        ListObserver obs2 = new ListObserver("Observer 2", listSubj);   // second list observer
        Integer int1 = 1, int2 = 2, int3 = 3;                                 // objects to add to list
        // Appends objects to list
        System.out.println("Appending Objects to list");
        listSubj.append(int1);
        listSubj.append(int2);
        listSubj.append(int3);

        // Deletes object from list, should display message from each observers
        System.out.println("Deleting object from list");
        listSubj.delete(int2);

        // Detaches observer and deletes object from list, should display message from remaining observer
        System.out.println("\nDetaching Observer from list");
        listSubj.detach(obs1);
        System.out.println("Deleting object from list");
        listSubj.delete(int1);

        // Reattaches observer and deletes object from list, should display message from each observer
        System.out.println("\nAttaching Observer from list");
        listSubj.attach(obs1);
        System.out.println("Deleting object from list");
        listSubj.delete(int3);
    }
}
