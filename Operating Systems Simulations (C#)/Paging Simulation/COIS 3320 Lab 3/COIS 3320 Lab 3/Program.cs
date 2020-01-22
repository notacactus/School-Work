/*  Program:    COIS 3320 Assignment 3
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2018-11-30
 *  Purpose:    Loads job/page data from cvs files and loads pages into memory using LRU and random replacement policies
 *  Dependancies:
 *      See include statements
 *  Software/Language:
 *      Microsoft Visual Studio 2017 C#
 *  Assumptions:    Physical memory size is 10 pages
 *                  Swap space is 15 pages 
 */
using System;
using System.Collections.Generic;

namespace COIS_3320_Lab_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Memory memory = new Memory(10, 15);     // Object containing memory to load pages into and run replacement algortithmon
            LinkedList<Page> pipeline;              // List of pages in pipeline
            string[] fileNames = { "job_data_1.csv", "job_data_2.csv", "job_data_3.csv", "job_data_4.csv", "job_data_5.csv", "job_data_6.csv" };    // file names to load job/page data from
            // Loop to run algorithm on each file
            foreach (string fileName in fileNames)
            {
                // Loads jobs/pages in file into pipeline then clears memory from previous simulation and sets algorithm to LRU
                pipeline = Page.LoadPipeline(fileName);
                memory.Clear();
                memory.LRU();
                // Runs simulation on pipeline and displays results
                memory.Pipeline(pipeline);
                memory.DisplayStatistics(fileName);
                // clears memory from previous simulation and sets algorithm to random
                memory.Clear();
                memory.Random();
                // Runs simulation on pipeline and displays results
                memory.Pipeline(pipeline);
                memory.DisplayStatistics(fileName);
            }

            // Calculates average page hits for running random algorithm 100000x on each pipeline 
            //foreach (string fileName in fileNames)
            //    Console.WriteLine($"{fileName} Avg Hits = {RandomAvgHits(Page.LoadPipeline(fileName), memory, 100000)}");

            Console.ReadLine();

        }

        // Average the number of page hits for random algorithm with a given pipeline and number of iterations
        // Parameters:
        //      LinkedList<Page> pipeline   - pipeline to use
        //      Memory memory               - memory to use pipeline on
        //      int numIter                 - number of iterations to use
        static double RandomAvgHits(LinkedList<Page> pipeline, Memory memory, int numIter)
        {
            double sumHits = 0; // sum of hits
            // runs the pipeline repeatedly and adds hits to some
            for (int i = 0; i < numIter; i++)
            {
                memory.Clear();
                memory.Pipeline(pipeline);
                sumHits += memory.PageHits;
            }
            // returns average hits
            return(sumHits / numIter);
        }
    }
}
