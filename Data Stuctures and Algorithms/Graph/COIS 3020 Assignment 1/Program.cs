/*  Program:    COIS 3020 Assignment 1
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-01-28
 *  Purpose:    Defines a SubwapMap class based on an adjacency list to represent a subway system and tests the functionality of each of its methods
 *  Dependancies:
 *      See include statements
 *  Software/Language:
 *      Microsoft Visual Studio 2017 C#
 *  Assumptions:    The SubwayMap is assumed to be fully connected for the FastestRoute and CriticalStations methods
 */

using System;

namespace COIS_3020_Assignment_1
{
    class Program
    {
        // Generates a SubwayMap and runs various tests on each public method
        static void Main(string[] args)
        {
            string[] stationNames = { "A", "B", "C", "D", "E", "F", "G", "H", "I"};                                                     // Station to add to subway map (A-I)
            string[,] stationLinks = { { "A", "D", "Blue" }, { "D", "E", "Blue" }, { "E", "F", "Blue" },                                // Links for blue subway line to map (A->D->E->F)
                { "A", "B", "Red" }, { "B", "C", "Red" }, { "C", "D", "Red" }, { "D", "E", "Red" },                                     // Links for red subway line to map (A->B->C->D->E)
                { "C", "D", "Green" }, { "D", "G", "Green" }, { "G", "F", "Green" }, { "F", "H", "Green" }, { "H", "I", "Green" }, };   // Links for green subway line to map (C->D->G->F->H->I)
            SubwayMap subway = new SubwayMap();
            // Constructs subway map using stations from stationsNames and links from stationLinks
            // Tests valid input for adding new stations, adding new links, and adding parallel links of different colours
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Constructing SubwayMap\n");
            foreach (string name in stationNames)
                Tests.AddStation(subway, name);
            for (int i = 0; i < stationLinks.GetLength(0); i++)
                Tests.AddLink(subway, stationLinks[i, 0], stationLinks[i, 1], stationLinks[i, 2]);

            // Tests input for adding new stations and links and for deleting links
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Tests for adding Stations\n");
            // Tests adding new station
            Tests.AddStation(subway, "J");
            // Tests adding duplicate station
            Tests.AddStation(subway, stationNames[3]);

            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Tests for adding Links\n");
            // Tests adding new link
            Tests.AddLink(subway, "I", "J", "Red");
            // Tests adding parallel link
            Tests.AddLink(subway, "I", "J", "Orange");
            // Tests adding duplicate link
            Tests.AddLink(subway, stationLinks[2, 0], stationLinks[2, 1], stationLinks[2, 2]);
            // Tests adding duplicate link with station order swapped
            Tests.AddLink(subway, stationLinks[2, 1], stationLinks[2, 0], stationLinks[2, 2]);
            // Tests adding link with non-existant station in first postion
            Tests.AddLink(subway, "Z", stationLinks[2, 0], stationLinks[2, 2]);
            // Tests adding link with non-existant station in second postion
            Tests.AddLink(subway, stationLinks[2, 1], "Z", stationLinks[2, 2]);

            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Tests for deleting Links\n");
            // Tests deleting part of parallel link
            Tests.DeleteLink(subway, "I", "J", "Red");
            // Tests deleting solitary link
            Tests.DeleteLink(subway, "I", "J", "Orange");
            // Tests removing link between links stations with unused colour
            Tests.DeleteLink(subway, stationLinks[2, 0], stationLinks[2, 1], "Orange");
            // Tests removing link between stations with non-existant station in first position
            Tests.DeleteLink(subway, "Z", stationLinks[2, 1], stationLinks[2, 2]);
            // Tests removing link between stations with non-existant station in second position
            Tests.DeleteLink(subway, stationLinks[2, 0], "Z", stationLinks[2, 2]);
            
            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Tests for shortest route method\n");
            // Testing basic usage - expected: green(I->H->F->G)
            Tests.FastestRoute(subway, "I", "G");
            // Testing staying on single line if possible - expected: blue(A->D->E)
            Tests.FastestRoute(subway, "A", "E");
            // Testing switching lines - expected: red(B->A)->blue(A->D)->green(D->G)
            Tests.FastestRoute(subway, "B", "G");

            Console.WriteLine(new string('-', 100));
            Console.WriteLine("Tests for critical stations method\n");
            // Tests critical stations at intersection of loops (D), intersection of loop and non-loop (F), and middle of branch (H)
            // Start point (A) is not a critical station
            // SubwayMap as shown in diagram (J not connected) - expected results: D, F, H
            Tests.CriticalStations(subway);
            // Tests start point as critical station
            // SubwayMap as shown with station J connected to A - expected results: A, D, F, H
            Tests.AddLink(subway, "A", "J", "Red");
            Tests.CriticalStations(subway);
            // Tests no critical points
            // SubwayMap as shown with station J connected to A and I - expected results: none
            Tests.AddLink(subway, "I", "J", "Red");
            Tests.CriticalStations(subway);

            Console.ReadLine();
        }

        
    }
}
