package com.company;

// concrete instance for format 2
// implemented as a lazy singleton class
public final class Format2Factory implements DateTimeFactory {
    // singleton object instance
    static Format2Factory instance;

    // private constructor
    private Format2Factory() {
    }

    // method to get singleton instance, lazy implementation initializes singleton instance if null
    public static DateTimeFactory getInstance() {
        if (instance == null)
            instance = new Format2Factory();
        return instance;
    }

    // methods to produce DateObject and TimeObject of format 2
    @Override
    public DateObject getDateObject() {
        return new DateFormat2();
    }

    @Override
    public TimeObject getTimeObject() {
        return new TimeFormat2();
    }
}
