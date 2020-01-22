package com.company;


// Builder class for Composite List (ListComponent), recursively builds nested lists
public class ListBuilder {
    private int brackets, position;     // counters for bracket depth and current position in list
    private ListComposite list;         // Composite list being built
    private ListBuilder nestedBuilder;  // Nested builder for recursively building nested lists

    // Constructor sets counters for brackets and position to 0
    public ListBuilder() {
        brackets = 0;
        position = 0;
    }
    // Increases bracket level by one recursively through nested ListBuilders
    // bracket level must be at least 1 to add items and only one base-level bracket is allowed
    public void buildOpenBracket() throws Exception {
        // Checks if first bracket has been opened
        if (brackets == 0) {
            // If first bracket not yet opened instantiate list
            if (list == null)
                list = new ListComposite();
            // If first bracket already closed once throws exception
            else {
                throw new Exception("Error: multiple starting brackets");
            }
        } else {
            // if first bracket opened already, instantiate nested builder if needed and recursively open brackets of nested builders
            if (brackets == 1)
                nestedBuilder = new ListBuilder();
            nestedBuilder.buildOpenBracket();
        }
        // increase bracket count by 1
        brackets++;
    }

    // Recursively closes brackets through all nested builders
    // exits if no brackets to close
    public void buildCloseBracket() throws Exception {
        // if all brakets closedthrows exception
        if (brackets == 0) {
            throw new Exception("Error: too many closing brackets detected");
        } else if (brackets >= 2) {
            // if there are multiple brackets open, recursively close brackets in nested builders
            nestedBuilder.buildCloseBracket();
            // if closing last nested builder, build list from builder and add to Composite List, then close nested builder and increase position counter
            if (brackets == 2) {
                list.addChild(position, nestedBuilder.getList());
                nestedBuilder = null;
                position++;
            }
        }
        // decrease bracket count by 1
        brackets--;
    }

    // Recursively adds element to deepest nested builder
    // Parameters:
    //  int element - element to add to list
    public void buildElement(int element) throws Exception {
        // if no open brackets (list not started or already closed) throws exception
        if (brackets == 0) {
            throw new Exception("Error: no opening bracket detected");
        } else if (brackets == 1) {
            // if at deepest nested builder, add item to that builders list and increment position
            list.addChild(position, new Item(element));
            position++;
        } else {
            // if not at deepest level, recursively add element to nested list
            nestedBuilder.buildElement(element);
        }
    }

    // returns finished list, must have opened brackets at least once and closed all open brackets
    public ListComponent getList() throws Exception {
        // if brackets never opened throws exception
        if (list == null) {
            throw new Exception("Error: no list provided");
        }
        // if brackets left unclosed, throws exception
        if (brackets > 0) {
            throw new Exception("Error: unclosed brackets");
        }
        // otherwise returns list
        return list;
    }
}
