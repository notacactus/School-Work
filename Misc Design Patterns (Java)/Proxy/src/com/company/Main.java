/*  Program:    COIS 3040 Assignment 2 Question 2
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-03-03
 *  Purpose:   Implements a Text class using the proxy pattern to allow for 3 levels of access, read only, write only, and read/write
 */

package com.company;

public class Main {
    // Tests functionality of proxies
    public static void main(String[] args) {
        Text readText = new ProxyTextRead("Toast");             // read only Text
        Text writeText = new ProxyTextWrite("Toast2");          // write only Text
        Text readWriteText = new ProxyTextReadWrite("Toast3");  // read/write Text
        String readContent;                                                // content read from Text object

        // Testing read only Text proxy
        // Attempts to read, then write, then reads again to check if write succeeded
        System.out.println("Testing read only Text proxy");
        System.out.println("Reading from Text proxy: " + ((readContent = readText.getContent()) != null ? readContent : ""));
        System.out.println("Writing to Text proxy");
        readText.setContent("abcde");
        System.out.println("Reading from Text proxy: " + ((readContent = readText.getContent()) != null ? readContent : ""));

        // Testing write only Text proxy
        // Attempts to read, then write
        System.out.println("\n\nTesting write only Text proxy");
        System.out.print("Reading from Text proxy: ");
        System.out.print(((readContent = writeText.getContent()) != null ? readContent : ""));
        System.out.println("Writing to Text proxy");
        writeText.setContent("fghij");

        // Testing read/write Text proxy
        // Attempts to read, then write, then reads again to check if write succeeded
        System.out.println("\n\nTesting read/write Text proxy");
        System.out.println("Reading from Text proxy: " + ((readContent = readWriteText.getContent()) != null ? readContent : ""));
        System.out.println("Writing to Text proxy");
        readWriteText.setContent("klmno");
        System.out.println("Reading from Text proxy: " + ((readContent = readWriteText.getContent()) != null ? readContent : ""));

    }
}
