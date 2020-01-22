// Algoritm for critical points method inspired by https://www.geeksforgeeks.org/articulation-points-or-cut-vertices-in-a-graph/

using System;
using System.Collections.Generic;

namespace COIS_3020_Assignment_1
{
    // Station class, vertex of an undirected graph (SubwayMap) connected by Links
    class Station
    {
        private string name;            // Station Name
        private LinkedList<Link> links; // Links (edges) to other Stations
        private int visited;            // indicates whether the station was visited during a search and at what point (-1 for unvisited)
        private bool isCritical;        // flag indicating if found to be a critical node in a search
        private Station previous;       // previous station for retracing path in breadth first search
        private string previousColour;  // previous link colour for retracing path in breadth first search

        // Getters/Setters for fields
        public string Name { get { return name; } }
        public IEnumerable<Link> Links { get { return links; } }
        public int Visited { get { return visited; } set { visited = value; } }
        public bool IsCritical { get { return isCritical; } set { isCritical = value; } }
        public Station Previous { get { return previous; } set { previous = value; } }
        public string PreviousColour { get { return previousColour; } set { previousColour = value; } }

        // Constructor for Station
        // Parameters:
        //      string name - name of Station
        public Station(string name)
        {
            // sets name to given value and sets other fields to default values
            this.name = name;
            links = new LinkedList<Link>();
            visited = -1;
            isCritical = false;
            previous = null;
            previousColour = null;
        }

        // Checks if station is linked to another station with a certain colour link, returns link if found, otherwise returns null
        // Parameters:
        //      Station station - station to check if connected with
        //      string colour   - colour of link to check
        public Link LinkedTo(Station station, string colour)
        {
            // iterates through list of links and check if the link is the given colour and connected to the given station
            foreach (Link link in links)
            {
                if (link.Colour == colour && (link.Station1 == station || link.Station2 == station))
                    return link;
            }
            return null;
        }

        // Adds a given link to list of links
        // Parameters:
        //      Link link - Link to add
        public void AddLink(Link link)
        {
            links.AddLast(link);
        }

        // removes a given link from list of links
        // Parameters:
        //      Link link - Link to remove
        public void DeleteLink(Link link)
        {
            links.Remove(link);
        }

    }

    // Link class, edge of an undirected graph (SubwayMap) connecting Stations, distinguished by colour
    class Link
    {
        private Station station1;   // station on one end of link
        private Station station2;   // station on other end of link
        private string colour;      // colour of link

        // Getters for fields
        public Station Station1 { get { return station1; } }
        public Station Station2 { get { return station2; } }
        public string Colour { get { return colour; } }

        // Constructor for Link
        // Parameters:
        //      Station station1 - station on one end of link
        //      Station station2 - station on other end of link
        //      string colour    - colour of link
        public Link(Station station1, Station station2, string colour)
        {
            // sets fields to given values
            this.station1 = station1;
            this.station2 = station2;
            this.colour = colour;
        }
    }

    // SubwayMap class, an undirected graph composed of Stations (verteces) and Links (edges), represents a subway system and allows for parallel links of different colours
    // Search methods assume all stations are linked
    class SubwayMap
    {
        private LinkedList<Station> stations;   // List of stations in subway

        // constructor for SubwayMap
        public SubwayMap()
        {
            stations = new LinkedList<Station>(); // instatiates list of stations
        }

        // Adds a new station with given name to SubwayMap, returns true if successful, false if duplicate station exists
        // Paratemeters:
        //      string name - name of station to add
        public bool InsertStation(string name)
        {
            // checks if station with given name already exists and returns false if so
            foreach (Station station in stations)
            {
                if (station.Name == name)
                    return false;
            }
            // otherwise adds new station and returns true
            stations.AddLast(new Station(name));
            return true;
        }

        // Creates a new link between two given stations, returns true if successful, false if duplicate link exists or either station given does not exist
        // Paratemeters:
        //      string from     - name of one station to link
        //      string to       - name of other station to link
        //      string colour   - colour of link
        public bool InsertLink(string from, string to, string colour)
        {
            Station station1 = null;    // reference to one station to link 
            Station station2 = null;    // reference to other station to link
            Link newLink;               // new link between given stations
            // searches for stations with given names in list of stations and stores references in station1, station2 if found
            foreach (Station station in stations)
            {
                if (station.Name == from)
                    station1 = station;
                else if (station.Name == to)
                    station2 = station;
            }
            // if both stations found and not linked with given colour, creates new link and adds to both stations, then returns true
            if (station1 != null && station2 != null && station1.LinkedTo(station2, colour) == null)
            {
                newLink = new Link(station1, station2, colour);
                station1.AddLink(newLink);
                station2.AddLink(newLink);
                return true;
            }
            // otherwise returns false
            return false;
        }

        // deletes a link of a givne colour between two stations, returns true if successful, false if link does not exists
        // Paratemeters:
        //      string from     - name of one station in link
        //      string to       - name of other station in link
        //      string colour   - colour of link
        public bool DeleteLink(string from, string to, string colour)
        {
            Station station1 = null;    // reference to one station in link
            Station station2 = null;    // reference to one station in link
            Link link;                  // reference to link to remove
            // searches for stations with given names in list of stations and stores references in station1, station2 if found
            foreach (Station station in stations)
            {
                if (station.Name == from)
                    station1 = station;
                else if (station.Name == to)
                    station2 = station;
            }
            // if both stations found and linked with given colour, removes link from both stations, then returns true
            if (station1 != null && station2 != null)
            {
                if ((link = station1.LinkedTo(station2, colour)) != null)
                {
                    station1.DeleteLink(link);
                    station2.DeleteLink(link);
                    return true;
                }
            }
            //otherwise returns false
            return false;
        }

        // determines the shortest route between two stations with given names, returns false if either station does not exist
        // Paratemeters:
        //      string from     - name of starting station
        //      string to       - name of ending station
        public bool FastestRoute(string from, string to)
        {
            Station start = null; // reference to starting station
            Station end = null;   // reference to ending station
            // searches for stations with given names in list of stations and stores references in start, end if found
            foreach (Station station in stations)
            {
                if (station.Name == from)
                    start = station;
                else if (station.Name == to)
                    end = station;
            }
            // if both stations found call private FastestRoute with start and end Stations
            if (start != null && end != null)
            {
                FastestRoute(start, end);
                // after search reset values used in search
                foreach (Station station in stations)
                {
                    station.Visited = -1;
                    station.Previous = null;
                    station.PreviousColour = null;
                }
                return true;
            }
            else
                return false;
        }
        // determines the shortest route between two stations using a breadth first search
        // Paratemeters:
        //      Station from    - starting station
        //      Station to      - ending station
        private void FastestRoute(Station from, Station to)
        {
            Station current = from;                                 // current station, starts at given startign position
            Station next = null;                                    // next station
            Queue<Station> queue = new Queue<Station>();            // queue of stations
            LinkedList<Station> route = new LinkedList<Station>();  // list of stations in shortest route
            // visit first station and enqueue it
            current.Visited = 0;
            queue.Enqueue(current);
            // while there are stations in the queue, dequeue the first and set as current, if it is the ending station stop looping
            while (queue.Count != 0 && (current = queue.Dequeue()) != to)
            {
                // visit every unvisited station linked to current, set their previous station/colour based on link to current and add them to queue
                foreach (Link link in current.Links)
                {
                    if ((next = (link.Station1 != current ? link.Station1 : link.Station2)).Visited < 0)
                    {
                        next.Visited = 0;
                        next.Previous = current;
                        // Checks if transfer between lines needed and sets previous link colour to minimize unnecessary transfers
                        if (current.PreviousColour != null && next.LinkedTo(current, current.PreviousColour) != null)
                            next.PreviousColour = current.PreviousColour;
                        else
                            next.PreviousColour = link.Colour;
                        queue.Enqueue(next);
                    }
                }
            }
            // add ending station to front of route
            route.AddFirst(current);
            // traverse through previous stations adding each station to front of route to reconstruct path taken
            while (current.Previous != null)
            {
                route.AddFirst(current.Previous);
                current = current.Previous;
            }
            // Prints out route with line changes
            foreach (Station station in route)
            {
                // Checks if line changed
                if (station.Previous == null)
                {
                    Console.WriteLine($"Start at {station.Name}");
                }
                else
                {
                    if (station.Previous.PreviousColour == null)
                        Console.WriteLine($"Get on {station.PreviousColour} Line");
                    else if (station.Previous.PreviousColour != station.PreviousColour)
                        Console.WriteLine($"Switch to {station.PreviousColour} Line");
                    Console.WriteLine($"Ride to {station.Name} ");
                }
            }


        }

        // uses a depth first search to find critical stations (articulation points) and returns them as a list, assumes all stations are connected
        public LinkedList<Station> CriticalStations()
        {
            LinkedList<Station> criticalList = new LinkedList<Station>();   // list of critical stations to return
            Station first = stations.First.Value;                           // first station to visit (first in list)
            Station next;                                                   // next station to visit
            int childCount = 0;                                             // counter for number of Stations visited from first station
            // visits first station
            first.Visited = 0;
            // visit each station linked to first station
            foreach (Link link in first.Links)
            {
                // determines next station and visits it if not already visited
                next = (link.Station1 != first ? link.Station1 : link.Station2);
                if (next.Visited < 0)
                {
                    CriticalStations(next, 1);
                    childCount++;
                }
            }
            // if multiple stations visited from first station, set first station as critical
            if (childCount > 1)
                first.IsCritical = true;
            // adds critical stations to list and resets visited and critical flags
            foreach (Station station in stations)
            {
                if (station.IsCritical)
                {
                    criticalList.AddLast(station);
                    station.IsCritical = false;
                }
                station.Visited = -1;
            }
            // return list of critical stations
            return criticalList;
        }

        // recursive part of critical stations depth first search, returns furthest back point accessable without revisiting nodes
        // Parameters:
        //      Station current - current station being visited
        //      int distance    - current distance traveled
        private int CriticalStations(Station current, int distance)
        {
            Station next;               // next station to visit
            int backEdge;               // furthest back station branch connects to
            int minEdge = distance - 1; // furthest back station this station can connect to
            // visits current station at current depth
            current.Visited = distance;
            // attempt to visit each linked station, finds furthest back connected station as sets visited distance to that distance
            foreach (Link link in current.Links)
            {
                // determine which station link connects to
                next = (link.Station1 != current ? link.Station1 : link.Station2);
                // if station isn't visited, recursively visit it
                if (next.Visited < 0)
                {
                    backEdge = CriticalStations(next, distance + 1);
                    // if no back edges set node as critical
                    if (backEdge == distance)
                        current.IsCritical = true;
                    // if back edge less than current min, set as min edge
                    else if (backEdge < minEdge)
                        minEdge = backEdge;
                }
                // otherwise if next station visited less than min, set as min edge
                else if (next.Visited < minEdge)
                    minEdge = next.Visited;
            }
            // return earliest back edge found
            return minEdge;
        }
    }
}
