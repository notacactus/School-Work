package com.company;

import java.text.SimpleDateFormat;
import java.util.Date;

// implementation of TimeObject for format 1
public class TimeFormat1 implements TimeObject {

    // returns a string representing the current time in format HH:MM:SS , uses 24 hr clock for hours
    @Override
    public String getTime() {
        return (new SimpleDateFormat("HH:mm:ss")).format(new Date());
    }
}
