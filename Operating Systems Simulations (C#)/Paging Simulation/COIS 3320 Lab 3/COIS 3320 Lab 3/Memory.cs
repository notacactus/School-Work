using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_3320_Lab_3
{
    // class simulating memory in a system
    // contains physical memory and swap space and allows running LRU and random replacement algorithms on a pipeline of pages
    public class Memory
    {
        private Page[] memoryData;  // array of pages in physical memory
        private Page[] swapData;    // array of pages in swap space
        private bool randomSwap;    // flag to determine whether to run LRU or random replacement algorithm
        private int memCount;       // number of pages in physical memory
        private int swapCount;      // number of pages in swap space
        private int pageLoads;      // counter for new page loads
        private int pageHits;       // counter for page hits
        private int pageFaults;     // counter for page faults
        private int timer;          // timer to generate timestamps for LRU algorithm
        private Random rnd;                 // random number generator for random algorithm
        private LinkedList<int> errorJobs;  // List of failed jobs from out of memory errors
        private const int Normal_Job_Termination = -999;    // Constant to indicate finished job

        // Constructor takes size of physical memory and swap space
        public Memory(int memorySize, int swapSize)
        {
            // create a page array of given sizes for memory and swap space
            memoryData = new Page[memorySize];
            swapData = new Page[swapSize];
            // sets all counters to 0, algorithm to LRU, and creates empty list of failed jobs
            memCount = 0;
            swapCount = 0;
            pageLoads = 0;
            pageHits = 0;
            pageFaults = 0;
            timer = 0;
            rnd = new Random();
            randomSwap = false;
            errorJobs = new LinkedList<int>();
        }

        public int PageHits
        {
            get { return pageHits; }
        }

        // Functions to set algorithm to LRU and random
        public void LRU()
        {
            randomSwap = false;
        }
        public void Random()
        {
            randomSwap = true;
        }
        // Clears data from previous simulations
        public void Clear()
        {
            // sets all pages in memory/swap to null, all counters to 0, and clears error jobs
            for (int i = 0; i < memoryData.Length; i++)
                memoryData[i] = null;
            for (int i = 0; i < swapData.Length; i++)
                swapData[i] = null;
            memCount = 0;
            swapCount = 0;
            pageLoads = 0;
            pageHits = 0;
            pageFaults = 0;
            timer = 0;
            errorJobs.Clear();
        }

        // Simulates page number requiest pipeline with either LRU or random replacement algorithm
        // Parameters:
        //      LinkedList<Page> pipeline   - list of pages to process
        public void Pipeline(LinkedList<Page> pipeline)
        {
            int pageFound;  // index of page in swap space if found there, value of -1 indicates found in main memory, -2 indicates not found in memory
            // Loads each page from pipeline into memory in order
            foreach (Page pipePage in pipeline)
            {
                // if job is finished, remove all related pages from memory
                if (pipePage.PageNum == Normal_Job_Termination)
                    RemoveJob(pipePage.Job);
                // If job failed, do nothing. Otherwise search for page in memory and move to physical memory if in swap space. If not in memory load it in
                else if (!errorJobs.Contains(pipePage.Job))
                {
                    // find if page is currently in physical memory/swap space
                    pageFound = FindPage(pipePage);
                    // if page is in physical memory update timestamp and page hit counter
                    if (pageFound == -1)
                    {
                        pipePage.Timestamp = timer;
                        timer++;
                        pageHits++;
                    }
                    // if page in swap space, move to main memeory
                    else if (pageFound >= 0)
                    {
                        // if there is space to move from swap to main, do so and remove from swap space/update timestamp/swap count/page faults
                        if (AddToMem(pipePage))
                        {
                            swapData[pageFound] = null;
                            pageFaults++;
                            swapCount--;
                            pipePage.Timestamp = timer;
                            timer++;
                        }
                        // if no space to swap, remove all pages from related job in memory and add job to failed jobs
                        else
                        {
                            RemoveJob(pipePage.Job);
                            errorJobs.AddLast(pipePage.Job);
                        }
                    }
                    // if page not in physical memory or swap attempt to add to physical memory
                    else
                    {
                        // if space available, add to memory and update timestamp/new page load counter
                        if (AddToMem(pipePage))
                        {
                            pipePage.Timestamp = timer;
                            timer++;
                            pageLoads++;
                        }
                        // if no space to add, remove all pages from related job in memory and add job to failed jobs
                        else
                        {
                            RemoveJob(pipePage.Job);
                            errorJobs.AddLast(pipePage.Job);
                        }
                    }
                }
            }
        }

        // searches for a page in memory and, if found in swap, returns index; if found in physical memory, returns -1; if not found, returns -2
        // Parameters:
        //      Page page   - page to locate
        private int FindPage(Page page)
        {
            // if there are pages in physical memory search for the page
            if (memCount > 0)
            {
                // if page matches page in pysical memory, return -1
                for (int i = 0; i < memoryData.Length; i++)
                {
                    if (memoryData[i] != null && memoryData[i].PageNum == page.PageNum)
                    {
                        memoryData[i] = page;
                        return -1;
                    }
                }
            }
            // if there are pages in swap search for the page
            if (swapCount > 0)
            {
                // if page matches, return index
                for (int i = 0; i < swapData.Length; i++)
                {
                    if (swapData[i] != null && swapData[i].PageNum == page.PageNum)
                        return i;
                }
            }
            // if page not found return -2
            return -2;
        }

        // Adds a page to physical memory, returns true if page added, false if memory full
        // Parameters:
        //      Page page     - page to add
        private bool AddToMem(Page page)
        {
            // if physical memory if not full add page 
            if (memCount < memoryData.Length)
            {
                // finds first empty space and stores page
                for (int i = 0; i < memoryData.Length; i++)
                {
                    if (memoryData[i] == null)
                    {
                        memoryData[i] = page;
                        memCount++;
                        break;
                    }
                }
                return true;
            }
            // if swap space not full, moves page to swap based on algorithm and stores page/updates
            else if (swapCount < swapData.Length)
            {
                memoryData[MoveToSwap()] = page;
                swapCount++;
                return true;
            }
            // if all memory full return false
            else
                return false;
        }

        // moves a page from physical memory to swap based on set algorithm, returns index of page moved (assumes free space in swap)
        private int MoveToSwap()
        {
            int toSwap = -1;    // sets index to sentinal value
            // if random algoirthm set, generates a random index within range to swap
            if (randomSwap)
                toSwap = rnd.Next(0, memoryData.Length);
            // if LRU set, finds least recently accessed page
            else
            {
                // searches through pages in physical memory and stores index of page with lowest timestamp
                for (int i = 0; i < memoryData.Length; i++)
                {
                    if (memoryData[i] != null)
                    {
                        if (toSwap < 0 || memoryData[i].Timestamp < memoryData[toSwap].Timestamp)
                            toSwap = i;
                    }
                }
            }
            // Finds an empty space in swap and copies page at found index there
            for (int i = 0; i < swapData.Length; i++)
            {
                if (swapData[i] == null)
                {
                    swapData[i] = memoryData[toSwap];
                    break;
                }
            }
            // removes page from physical memory and returns index
            memoryData[toSwap] = null;
            return toSwap;
        }

        // removes all pages relates to a given job from physical memory/swap
        // Parameters:
        //      int job - id number of job to remove
        private void RemoveJob(int job)
        {
            // searches physical memory for pages with matching job and sets them to null
            for (int i = 0; i < memoryData.Length; i++)
            {
                if (memoryData[i] != null)
                {
                    if (memoryData[i].Job == job)
                    {
                        memoryData[i] = null;
                        memCount--;
                    }
                }
            }
            // searches swap space for pages with matching job and sets them to null
            for (int i = 0; i < swapData.Length; i++)
            {
                if (swapData[i] != null)
                {
                    if (swapData[i].Job == job)
                    {
                        swapData[i] = null;
                        swapCount--;
                    }
                }
            }
        }
        
        // Displays statistics after a simulation has been performed
        // Parameters:
        //      string fileName - file name/path that was used for simulation
        public void DisplayStatistics(string fileName)
        {
            // Outputs file name, algorithm used, first-loads, page hits, and page faults
            Console.WriteLine(new String('-', 100));
            Console.WriteLine($"\nResults of {(randomSwap ? "Random" : "Least Recently Used")} replacement algorithm on {fileName}:\n");
            Console.WriteLine($"Number of First-Loads: {pageLoads} | Number of Page Hits: {pageHits} | Number of Page Faults: {pageFaults}\n");
            // Outputs pages remaining in physical memory and swap and outputs table showing which pages are left and where
            Console.WriteLine($"   |{"",5}{$"Pages in Physical Memory: {memCount}", -33} {"|",-6} Pages in Swap Space: {swapCount}");
            Console.WriteLine($"___|{new String('_', 39)}|{new String('_', 40)}");
            for (int i = 0; i < Math.Max(memoryData.Length, swapData.Length); i++)
            {
                Console.WriteLine($"{i,3}|{"",3}{(i < memoryData.Length ? (memoryData[i] != null ? $"Job: {memoryData[i].Job,3} | Page Num Ref: {memoryData[i].PageNum}" : "Empty") : "N/A"), -36}{"|",-4}{(i < swapData.Length ? (swapData[i] != null ? $"Job: {swapData[i].Job,3} | Page Num Ref: {swapData[i].PageNum}" : "Empty") : "N/A")}");
            }
            // Outputs number of failed jobs and lists all failed jobs
            Console.WriteLine($"\nNumber of Page Management Errors: {errorJobs.Count}\n");
            Console.Write("Failed Jobs: ");
            foreach (int job in errorJobs)
                Console.Write($"Job {job} ");
            Console.WriteLine("\n\n" + new String('-', 100));
        }
    }
}
