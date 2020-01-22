using System;
using System.Collections.Generic;

namespace COIS_3320_Lab_2
{
    // Allows generation and storage of a set of Jobs and allows simulation of various scheduling methods to be tested on it
    class JobSet
    {
        private Job[] jobs;         // Stores job set
        private int switchCount;    // counts how many times a context switch has occured in the current scheduling test

        // Constructor for single type of job
        // Parameters:
        //      int length          - determines number of jobs to generate
        //      Random rnd          - used to generate random numbers
        //      double arrivalMu    - average arrival time
        //      double arrivalSigma - arrival time expected deviation
        //      double runMu        - average runtime
        //      double runSigma     - runtime expected deviation
        public JobSet(int length, Random rnd, double arrivalMu, double arrivalSigma, double runMu, double runSigma)
        {
            double arrivalCounter = 0;      // accumulator for arrival time
            jobs = new Job[length];         // instantiates array of jobs with given size
            switchCount = 0;                // sets number of context switches to 0
            // generates jobs to populate jobs array with random arrival time and runtime based on given values
            for (int i = 0; i < length; i++)
            {
                arrivalCounter += Gaussian.Generate(rnd, arrivalMu, arrivalSigma);
                jobs[i] = new Job(Gaussian.Generate(rnd, runMu, runSigma), arrivalCounter);
            }
        }

        // Constructor for 2 types of job
        // Parameters:
        //      Same as above
        //      double runMu2       - average runtime for second job type
        //      double runSigma2    - runtime standard devation for second job type
        //      double weight       - weight to determine how many of each job type to run (should be between 0 and 1)
        public JobSet(int length, Random rnd, double arrivalMu, double arrivalSigma, double runMu, double runSigma, double runMu2, double runSigma2, double weight)
        {
            double arrivalCounter = 0;      // accumulator for arrival time
            jobs = new Job[length];         // instantiates array of jobs with given size
            switchCount = 0;                // sets number of context switches to 0
            // generate jobs of 2 types with random arrival time and runtime based on weighting/given values
            for (int i = 0; i < length; i++)
            {
                arrivalCounter += Gaussian.Generate(rnd, arrivalMu, arrivalSigma);
                jobs[i] = (rnd.NextDouble() < weight) ? new Job(Gaussian.Generate(rnd, runMu, runSigma), arrivalCounter) : new Job(Gaussian.Generate(rnd, runMu2, runSigma2), arrivalCounter);
            }
        }

        // Returns the job object at given index of jobs array
        public Job GetJob(int index)
        {
            return jobs[index];
        }
        // Property for length of jobs array
        public int Length
        {
            get { return jobs.Length; }
        }
        // getter for context switch count
        public int SwitchCount
        {
            get { return switchCount; }
        }

        // Calculates avg response time by suming response times of each job in array and dividing by total number
        public double AvgResponse()
        {
            double sum = 0;     // accumulator for sum
            foreach (Job job in jobs)
                sum += job.Response;
            return sum / jobs.Length;
        }


        // Calculates avg turnaround time by suming turnaround times of each job in array and dividing by total number
        public double AvgTurnaround()
        {
            double sum = 0;     // accumulator for sum
            foreach (Job job in jobs)
                sum += job.Turnaround;
            return sum / jobs.Length;
        }

        // resets the remaining/start/end time of each job and sets switch counter to 0 to allow for another simulation to be run
        public void reset()
        {
            switchCount = 0;
            foreach (Job job in jobs)
                job.reset();
        }

        // Runs a simulation of First-In-First-Out/First-Come-First-Serve scheduling on the job set 
        public void FIFO()
        {
            double currentTime = 0; // Keeps track of elapsed time
            reset();                // resets results of any previous simulation
            // iterates through job set sequentially executing jobs
            for (int i = 0; i < Length; i++)
                // Execute next job fully
                GetJob(i).Execute(ref currentTime);
            // Set switch count to length of job set (each job has a single context switch)
            switchCount = Length;
        }

        // Runs a simulation of Shortest-Job-First scheduling on the job set 
        public void SJF()
        {
            double currentTime = 0; // Keeps track of elapsed time
            int shortest;           // keeps track of shortest job at a given time
            reset();                // resets results of any previous simulation
            // iterates through job set performing the shortest available job at each iteration until all jobs are completed
            for (int i = 0; i < Length; i++)
            {
                shortest = -1;  // sets shortest job to sentinal value
                // iterates through job set to find shortest available job, stops if finds an unavailable job
                for (int j = 0; j < Length && GetJob(j).ArrivedAt(currentTime); j++)
                {
                    // if next job isn't finished, check if it is the shortest yet found and save position if so
                    if (!GetJob(j).Finished)
                    {
                        if (shortest < 0 || GetJob(j).RunTime < GetJob(shortest).RunTime)
                            shortest = j;
                    }
                }
                // if no available jobs found, execute next job
                if (shortest < 0)
                    GetJob(i).Execute(ref currentTime);
                // if any jobs available execute shortest available job
                else
                    GetJob(shortest).Execute(ref currentTime);
            }
            // Set switch count to length of job set (each job has a single context switch)
            switchCount = Length;
        }

        // Runs a simulation of pre-emptive Shortest-Job-First scheduling on the job set with a given pre-emptive clock time
        public void STCF(double preemptTime)
        {
            double currentTime = 0; // Keeps track of elapsed time
            int shortest = -1;      // keeps track of shortest job at a given time
            int previous = -1;      // keeps track of previous job to check for context switch
            bool finished = false;  // flag to stop when all jobs finished
            reset();                // resets results of any previous simulation
            // loops until all jobs finished
            while (!finished)
            {
                shortest = -1;  // sets shortest job to sentinal value
                // iterates through job set to find shortest available job
                for (int i = 0; i < Length; i++)
                {
                    // if next job is not available stops iteration
                    if (!GetJob(i).ArrivedAt(currentTime))
                    {
                        // checks if any available jobs were found, if not sets next job as shortest
                        if (shortest < 0)
                            shortest = i;
                        break;
                    }
                    // if next job is available and not finished check if it is the shortest yet found and save position if so
                    else if (!GetJob(i).Finished)
                    {
                        if (shortest < 0 || GetJob(i).RemainingTime < GetJob(shortest).RemainingTime)
                            shortest = i;
                    }
                }
                // if no jobs were left to complete end simulation
                if (shortest < 0)
                    finished = true;
                else
                {
                    // if shortest job is not the same as previous iteration, increment context switch counter and set shortest job as new previous
                    if (shortest != previous)
                    {
                        switchCount++;
                        previous = shortest;
                    }
                    // Execute shortest job for duration of preemption
                    GetJob(shortest).Execute(ref currentTime, preemptTime);
                }
            }
        }
        
        public void RR(double quantum)
        {
            double currentTime = 0; // Keeps track of elapsed time
            int previous = -1;      // keeps track of previous job to check for context switch
            int position = 0;       // keeps track of next job to arrive
            LinkedList<int> running = new LinkedList<int>();    // keeps track of all jobs currently executing
            reset();                // resets results of any previous simulation
            // loops until all jobs finished
            while (position < Length)
            {
                // Add next job to queue and increment position
                running.AddLast(position);
                position++;
                // iterate through queue of running jobs, executing for a quantum of time until no jobs currently executing
                while (running.Count != 0)
                {
                    // if next job is not the same as previous iteration, increment context switch counter and set job as new previous
                    if (running.First.Value != previous)
                    {
                        switchCount++;
                        previous = running.First.Value;
                    }
                    // execute first job in queue for quantum of time
                    GetJob(running.First.Value).Execute(ref currentTime, quantum);
                    // if any new jobs have arrived, add them to the end of queue
                    while (position < Length && GetJob(position).ArrivedAt(currentTime))
                    {
                        running.AddLast(position);
                        position++;
                    }
                    // if first job did not finish add to end of queue
                    if (!GetJob(running.First.Value).Finished)
                        running.AddLast(running.First.Value);
                    // remove first job from front of queue
                    running.RemoveFirst();
                }
            }

        }
    }
}
