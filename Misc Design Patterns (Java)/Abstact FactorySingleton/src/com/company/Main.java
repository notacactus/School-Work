/*  Program:    COIS 3040 Assignment 1 Question 2
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-02-03
 *  Purpose:    Allows the user to select a format pattern for date and time and choose between displaying the current date or time in the chosen format until they wish to quit
 *              Uses abstract instance pattern for format choice and implements concrete factories as singletons
 */

package com.company;

import java.util.Scanner;

public class Main {

    public static void main(String[] args) {
        String input;                               // String to hold user input
        DateTimeFactory factory;                    // Factory to produce DateObjects and TimeObjects with chosen format
        Scanner scanner = new Scanner(System.in);   // Scanner to accept input from uset

        // Introduces program and displays format options
        System.out.println("This program will display the current date and time using one of 2 formats\n\n" +
                "Format 1:\n" +
                "Date - MM/DD/YYYY\n" +
                "Time - HH:MM:SS\n" +
                "Format 2:\n" +
                "Date - DD-MM-YYYY\n" +
                "Time - SS,MM,HH\n");

        // Prompts user to input desired format until valid format selected
        System.out.print("Enter \"1\" to use Format 1, or \"2\" to use Format 2: ");
        input = scanner.next();
        while (!input.equals("1") && !input.equals("2")) {
            System.out.print("\nInvalid Input \n" +
                    "Enter \"1\" to use Format 1, or \"2\" to use Format 2: ");
            input = scanner.next();
        }

        // Initializes instance for format 1 or 2 based on user input
        factory = (input.equals("1") ? Format1Factory.getInstance() : Format2Factory.getInstance());
        System.out.println("\nUsing Format " + input);

        // loop until user decides to exit
        do {
            // Prompts user to input action: display date, display time, quit. Repeats until valid input provided
            System.out.print("\nEnter \"d\" to display date, \"t\" to display time or \"q\" to quit: ");
            input = scanner.next();
            while (!input.equals("d") && !input.equals("D") && !input.equals("t") && !input.equals("T") && !input.equals("q") && !input.equals("Q")) {
                System.out.print("\nInvalid Input \n" +
                        "Enter \"d\" to display date, \"t\" to display time or \"q\" to quit: ");
                input = scanner.next();
            }
            // if date selected, output date formatted based on chosen instance
            if (input.equals("d") || input.equals("D")){
                System.out.println("\nDate: " + factory.getDateObject().getDate());
            }
            // if time selected, output time formatted based on chosen instance
            else if (input.equals("t") || input.equals("T")){
                System.out.println("\nTime: " + factory.getTimeObject().getTime());
            }
        } while (!input.equals("q") && !input.equals("Q")); // exit loop if quit selected
    }
}
