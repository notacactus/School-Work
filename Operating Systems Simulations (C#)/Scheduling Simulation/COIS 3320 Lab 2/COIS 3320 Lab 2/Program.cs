/*  Program:    COIS 3320 Assignment 2
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2018-10-26
 *  Purpose:    Generates a number of jobs with runtimes in 3 different distrubution and simulates running various scheduling algorithms on each set of jobs
 *  Dependancies:
 *      See include statements
 *  Software/Language:
 *      Microsoft Visual Studio 2017 C#
 *  Assumptions:    All jobs arrive in sequence with arrival times seperated by a normally distributed value with mu = 160, sigma = 15
 *                  1000 jobs are generated of each distribution
 */
using System;

namespace COIS_3320_Lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Random rnd = new Random();  // Random number generator used to generate job properties
            int numJobs = 1000;         // Number of jobs to generate for each job mixture
            int numOutput = 20;         // Number of jobs to output results for testing
            
            // Generates each job mixture
            JobSet jobs1 = new JobSet(numJobs, rnd, 160, 15, 150, 20);
            JobSet jobs2 = new JobSet(numJobs, rnd, 160, 15, 50, 5, 250, 50, 0.8);
            JobSet jobs3 = new JobSet(numJobs, rnd, 160, 15, 250, 50, 50, 5, 0.8);

            // Runs each scheduling algorithm on first job mixture and outputs response/turnaround time and context switches for each algorithm
            // uncomment a JobStats line after running an algorithm to see various stats for a number of individual jobs
            Console.WriteLine("Job Mixture 1 Runtime Distribution: 100% - Mu = 150, Sigma = 20\n");
 
            jobs1.FIFO();
            //JobStats(jobs1, numOutput);
            Console.WriteLine($"{"First-Come-First-Served", -30}: {JobResults(jobs1)}");
            
            jobs1.SJF();
            //JobStats(jobs1, numOutput);
            Console.WriteLine($"{"Shortest-Job-First",-30}: {JobResults(jobs1)}");
            
            jobs1.STCF(40);
            //JobStats(jobs1, numOutput);
            Console.WriteLine($"{"STCF (40 unit pre-emption)",-30}: {JobResults(jobs1)}");

            jobs1.RR(50);
            //JobStats(jobs1, numOutput);
            Console.WriteLine($"{"Round Robin (50 unit quanta)",-30}: {JobResults(jobs1)}");
            
            jobs1.RR(75);
            //JobStats(jobs1, numOutput);
            Console.WriteLine($"{"Round Robin (75 unit quanta)",-30}: {JobResults(jobs1)}");


            // Runs each scheduling algorithm on second job mixture and outputs response/turnaround time and context switches for each algorithm
            Console.WriteLine("\n" + new string('-', 100) +
                "\n\nJob Mixture 2 Runtime Distribution: 80% - Mu = 50, Sigma = 5; 20% - Mu = 250, Sigma = 50\n");

            jobs2.FIFO();
            //JobStats(jobs2, numOutput);
            Console.WriteLine($"{"First-Come-First-Served",-30}: {JobResults(jobs2)}");

            jobs2.SJF();
            //JobStats(jobs2, numOutput);
            Console.WriteLine($"{"Shortest-Job-First",-30}: {JobResults(jobs2)}");

            jobs2.STCF(40);
            //JobStats(jobs2, numOutput);
            Console.WriteLine($"{"STCF (40 unit pre-emption)",-30}: {JobResults(jobs2)}");

            jobs2.RR(50);
            //JobStats(jobs2, numOutput);
            Console.WriteLine($"{"Round Robin (50 unit quanta)",-30}: {JobResults(jobs2)}");

            jobs2.RR(75);
            //JobStats(jobs2, numOutput);
            Console.WriteLine($"{"Round Robin (75 unit quanta)",-30}: {JobResults(jobs2)}");


            // Runs each scheduling algorithm on third job mixture and outputs response/turnaround time and context switches for each algorithm
            Console.WriteLine("\n" + new string('-', 100) +
                "\n\nJob Mixture 3 Runtime Distribution: 80% - Mu = 250, Sigma = 50; 20% - Mu = 50, Sigma = 5\n");
            
            jobs3.FIFO();
            //JobStats(jobs3, numOutput);
            Console.WriteLine($"{"First-Come-First-Served",-30}: {JobResults(jobs3)}");

            jobs3.SJF();
            //JobStats(jobs3, numOutput);
            Console.WriteLine($"{"Shortest-Job-First",-30}: {JobResults(jobs3)}");

            jobs3.STCF(40);
            //JobStats(jobs3, numOutput);
            Console.WriteLine($"{"STCF (40 unit pre-emption)",-30}: {JobResults(jobs3)}");

            jobs3.RR(50);
            //JobStats(jobs3, numOutput);
            Console.WriteLine($"{"Round Robin (50 unit quanta)",-30}: {JobResults(jobs3)}");

            jobs3.RR(75);
            //JobStats(jobs3, numOutput);
            Console.WriteLine($"{"Round Robin (75 unit quanta)",-30}: {JobResults(jobs3)}");
            
            Console.ReadLine();
        }

        // Displays run/arrival/start/end/response/turnaround time for a given number of jobs in a job set
        static void JobStats(JobSet jobs, int numOutput)
        {
            for (int i = 0; i < numOutput; i++)
                Console.WriteLine($"RunTime: {jobs.GetJob(i).RunTime,-5:f1} | ArrivalTime: {jobs.GetJob(i).ArrivalTime,-7:f1} | StartTime: {jobs.GetJob(i).StartTime,-7:f1} | EndTime: {jobs.GetJob(i).EndTime,-7:f1} | ResponseTime: {jobs.GetJob(i).Response,-7:f1} | TurnaroundTime: {jobs.GetJob(i).Turnaround,-7:f1}");
        }
        // Returns a formatted string containing the avg response/turnaround time and context switches for a job set
        static string JobResults(JobSet jobs)
        {
            return $"AvgResponse: {jobs.AvgResponse(),-7:f1} | AvgTurnaround {jobs.AvgTurnaround(),-8:f1} | ContextSwitchs: {jobs.SwitchCount,-5}";
        }
    }
}
