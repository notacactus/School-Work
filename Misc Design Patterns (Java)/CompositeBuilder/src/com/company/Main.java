/*  Program:    COIS 3040 Assignment 2 Question 1
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-03-03
 *  Purpose:   Implements a list allowing nested lists using the composite pattern and a builder class for the list
 *              Main program takes a list as input from the user, tokenizes it, and uses the builder class to construct the list
 */

package com.company;

import java.util.Scanner;

public class Main {
    // parses input to build list and tests functionality of list builder / composite list
    public static void main(String[] args) {
        ListBuilder builder = new ListBuilder();    // Builder to construct list
        ListComponent list;                         // Composite list
        Scanner scanner = new Scanner(System.in);   // Scanner to accept input from user
        String input;                               // string to contain user input
        // Gets list input from user
        System.out.print("Enter a list to construct: ");
        input = scanner.nextLine();
        // Calls function to tokenize input and pass values to builder
        ParseStringToBuild(input, builder);
        // builds and prints values of list
        try {
            list = builder.getList();
            list.printValue();
        } catch (Exception e) {
            System.out.println(e.getMessage());
        }

        // Functionallity tests for ListComponent, uncomment to run tests
        // ListComponentTests();
    }

    // Tokenizes a given string and feeds the tokens into a ListBuilder to construct a list
    // Parameters:
    //  String input        - list inputted by user
    //  ListBuilder builder - builder to pass tokens to
    public static void ParseStringToBuild(String input, ListBuilder builder) {
        // Splits the string into tokens
        //  valid tokens include "(", ")", and any number of uninterupted digits
        //  any other characters are ignored
        String[] inputTokens = input.split("(?:[^\\d()]+)|(?<=[\\d+])(?=[^\\d])|(?<=[(]|[)])");
        try {
            // iterates through tokens
            for (String token : inputTokens) {
                // if token is "" ignore it
                if (token.equals(""))
                    continue;
                // if "(" open bracket
                if (token.equals("("))
                    builder.buildOpenBracket();
                    // if ")" close bracket
                else if (token.equals(")"))
                    builder.buildCloseBracket();
                    // otherwise, token must be number, add it as element to builder
                else
                    builder.buildElement(Integer.parseInt(token));
            }
        } catch (Exception e) {
            System.out.println(e.getMessage());
            System.exit(1);
        }
    }

    // Tests various functions of ListComponent
    public static void ListCompositeTests() {
        ListComponent list1 = null, list2 = null;                           // Lists for testing
        ListBuilder builder1 = new ListBuilder();                           // builder for testing
        ListBuilder builder2 = new ListBuilder();                           // second builder for testing
        String input1 = "(1 23 (4 6) 3 ((10) 4) 2)", input2 = "(3 (2))";    // lists to build
        System.out.println("\nTesting functionality of ListComponent");
        try {
            // Creates an prints list based on input1
            System.out.println("\nBuilding list: " + input1);
            ParseStringToBuild(input1, builder1);
            list1 = builder1.getList();
            list1.printValue();
            // Creates and prints list based on input2
            System.out.println("\n\nBuilding list: " + input2);
            ParseStringToBuild(input2, builder2);
            list2 = builder2.getList();
            list2.printValue();
        } catch (Exception e) {
            System.out.println(e.getMessage());
            System.exit(1);
        }

        // Adds second list to first and prints result
        System.out.println("\n\nAdding second list to index 2 of first list");
        list1.addChild(2, list2);
        list1.printValue();

        // removes element from list and displays result
        System.out.println("\n\nRemoving fourth element of list");
        list1.removeChild(3);
        list1.printValue();

        // gets element from list and displays result
        System.out.println("\n\nRetrieving fifth element of list");
        list1.getChild(4).printValue();

    }
}
