package com.company;

// Concrete instance for format 1
// implemented as a lazy singleton class
public final class Format1Factory implements DateTimeFactory {
    // singleton object instance
    static Format1Factory instance;

    // private constructor
    private Format1Factory() {
    }

    // method to get singleton instance, lazy implementation initializes singleton instance if null
    public static DateTimeFactory getInstance() {
        if (instance == null)
            instance = new Format1Factory();
        return instance;
    }


    // methods to produce DateObject and TimeObject of format 1
    @Override
    public DateObject getDateObject() {
        return new DateFormat1();
    }

    @Override
    public TimeObject getTimeObject() {
        return new TimeFormat1();
    }
}
