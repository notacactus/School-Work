package com.company;

// Real text object, accessible through proxy objects
public class RealText implements Text {
    private String content;     // text content

    // Constructor
    // Parameters:
    //  String content - text to store
    public RealText(String content){
        this.content = content;
    }

    // Sets content to given text
    // Parameters:
    //  String newContent - new text to store
    public void setContent(String newContent) {
        this.content = newContent;
    }

    // returns text stored in content
    public String getContent() {
        return content;
    }
}
