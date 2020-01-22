package com.company;

import java.text.SimpleDateFormat;
import java.util.Date;


// implementation of DateObject for format 2
public class DateFormat2 implements DateObject {
    // returns a string representing the current date in format DD-MM-YYYY
    @Override
    public String getDate() {
        return (new SimpleDateFormat("dd-MM-yyyy")).format(new Date());
    }
}
