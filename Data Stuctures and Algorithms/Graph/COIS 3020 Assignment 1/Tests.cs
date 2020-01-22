using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_3020_Assignment_1
{
    // Tests for each public method of SubwayMap class
    // Each test takes subwaymap and parameters for associated method and outputs formatted result of function
    static class Tests
    {
        // Attempts to add a station to subwaymap, fails if duplicate station already exists
        public static void AddStation(SubwayMap subway, string name)
        {
            Console.WriteLine($"Adding Station \"{name}\": {(subway.InsertStation(name) ? "Success" : "Failure")}");
        }

        // Attempts to link two station in subwaymap, fails if either stations does not exist or duplicate link exists
        public static void AddLink(SubwayMap subway, string station1, string station2, string colour)
        {
            Console.WriteLine($"Adding Link Between Stations \"{station1}\" and \"{station2}\" with colour \"{colour}\": {(subway.InsertLink(station1, station2, colour) ? "Success" : "Failure")}");
        }

        // Attempts to delete link between two station in subwaymap, fails if either stations does not exist or if link does not exist
        public static void DeleteLink(SubwayMap subway, string station1, string station2, string colour)
        {
            Console.WriteLine($"Deleting Link Between Stations \"{station1}\" and \"{station2}\" with colour \"{colour}\": {(subway.DeleteLink(station1, station2, colour) ? "Success" : "Failure")}");
        }

        // Attemps to find and output shortest route between two stations in subwaymap, fails if either station does not exist
        public static void FastestRoute(SubwayMap subway, string start, string end)
        {
            Console.WriteLine($"Finding Shortest Route from \"{start}\" to \"{end}\":");
            if (!subway.FastestRoute(start, end))
                Console.WriteLine("Invalid Input");
            Console.WriteLine();
        }

        // Finds and outputs all critical stations in subwaymap
        public static void CriticalStations(SubwayMap subway)
        {
            Console.Write("Finding Critical Stations: ");
            LinkedList<Station> crit = subway.CriticalStations();
            if (crit.Count > 0)
            {
                foreach (Station station in crit)
                    Console.Write(station.Name + " ");
            }
            else
                Console.Write("None Found");
            Console.WriteLine();
        }

    }
}
