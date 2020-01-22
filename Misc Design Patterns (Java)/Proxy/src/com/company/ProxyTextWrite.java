package com.company;

// proxy text object with write only permission
public class ProxyTextWrite implements Text {
    RealText text;

    // Constructor
    // Parameters:
    //  String content - text to store in real text object
    public ProxyTextWrite(String content){
        text = new RealText(content);
    }

    // sets contents of real test
    @Override
    public void setContent(String newContent) {
        text.setContent(newContent);
    }

    // No read permission
    // Displays error message and returns null
    @Override
    public String getContent() {
        System.out.println("You do not have permission to read.");
        return null;
    }
}
