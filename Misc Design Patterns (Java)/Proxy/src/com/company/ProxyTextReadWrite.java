package com.company;

// proxy text object with read and write permission
public class ProxyTextReadWrite implements Text {
    RealText text;

    // Constructor
    // Parameters:
    //  String content - text to store in real text object
    public ProxyTextReadWrite(String content) {
        text = new RealText(content);
    }

    // sets contents of real test
    @Override
    public void setContent(String newContent) {
        text.setContent(newContent);
    }

    // returns contents of real test
    @Override
    public String getContent() {
        return text.getContent();
    }
}
