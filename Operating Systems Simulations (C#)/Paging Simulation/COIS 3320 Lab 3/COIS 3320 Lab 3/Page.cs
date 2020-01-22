using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COIS_3320_Lab_3
{
    // Class to store page data
    public class Page
    {
        private readonly int job;       // job page is realted to
        private readonly int pageNum;   // job page number reference
        private long timestamp;         // time last accessed for LRU algorithm comparision

        // Constructor accepts values for job and page number
        // sets timestamp to sentinal value
        public Page(int job, int pageNum)
        {
            this.job = job;
            this.pageNum = pageNum;
            timestamp = -1;
        }

        // getters for job/page number/timestamp and setter for timestamp
        public int Job
        {
            get { return job; }
        }
        public int PageNum
        {
            get { return pageNum; }
        }
        public long Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        // Given a 2 column cvs file name/path, constructs a list of pages using the first element of each row as job and the second as page number
        // Parameters:
        //      string fileName - name/path of cvs file to load (assumes file will contain table with 2 columns
        public static LinkedList<Page> LoadPipeline(string fileName)
        {
            LinkedList<Page> pipeline = new LinkedList<Page>();     // list of pages to store data
            string[] currentLine;                                   // current line of file, seperated into columns
            // Moves through file and loads each line into an array, seperated by column
            // Then creates a page using the data and adds it to the page pipeline
            using (StreamReader reader = new StreamReader(File.OpenRead(fileName)))
            {
                while (!reader.EndOfStream)
                {
                    currentLine = reader.ReadLine().Split(',');
                    pipeline.AddLast(new Page(Convert.ToInt32(currentLine[0]), Convert.ToInt32(currentLine[1])));
                }
            }
            return pipeline;
        }

        // Given a linked list of pages, resets all timestamps to sentinal values
        // Parameters:
        //      LinkedList<Page> pipeline   - list of pages to reset timestamps
        public static void ResetPipeline(LinkedList<Page> pipeline)
        {
            // iterates through list and sets timestamp of each page to sentinal value
            foreach (Page page in pipeline)
            {
                page.Timestamp = -1;
            }
        }
    }
}
