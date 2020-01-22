package com.company;

import java.text.SimpleDateFormat;
import java.util.Date;

// implementation of TimeObject for format 2
public class TimeFormat2 implements TimeObject {

    // returns a string representing the current time in format SS,MM,HH , uses 24 hr clock for hours
    @Override
    public String getTime() {
        return (new SimpleDateFormat("ss,mm,HH")).format(new Date());
    }
}
