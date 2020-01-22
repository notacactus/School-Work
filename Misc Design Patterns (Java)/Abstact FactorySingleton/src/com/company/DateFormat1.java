package com.company;

import java.text.SimpleDateFormat;
import java.util.Date;

// implementation of DateObject for format 1
public class DateFormat1 implements DateObject {
    // returns a string representing the current date in format MM/DD/YYYY
    @Override
    public String getDate() {
        return (new SimpleDateFormat("MM/dd/yyyy")).format(new Date());
    }
}
