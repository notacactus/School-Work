package com.company;

// Abstract instance for production of DateObjects, and TimeObjects
public interface DateTimeFactory {
    DateObject getDateObject();
    TimeObject getTimeObject();
}
