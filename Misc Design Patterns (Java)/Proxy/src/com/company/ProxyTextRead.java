package com.company;

// proxy text object with read only permission
public class ProxyTextRead implements Text {
    RealText text;  // instance of real text object

    // Constructor
    // Parameters:
    //  String content - text to store in real text object
    public ProxyTextRead(String content){
        text = new RealText(content);
    }

    // No write permission
    // Displays error message
    @Override
    public void setContent(String newContent) {
        System.out.println("You do not have permission to write.");
    }

    // returns contents of real test
    @Override
    public String getContent() {
        return text.getContent();
    }
}
