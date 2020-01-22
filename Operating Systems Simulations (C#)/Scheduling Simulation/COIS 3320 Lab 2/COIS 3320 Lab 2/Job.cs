
namespace COIS_3320_Lab_2
{
    // Class to store various properties of a job with basic functions to manipulate the properties
    class Job
    {
        private readonly double runTime;        // job length
        private readonly double arrivalTime;    // arrival time
        private double remainingTime;           // remaining time
        private double startTime;               // start time
        private double endTime;                 // finish time

        // Constructor accepts values for runTime and arrivalTime
        // calls reset function to assign default values to remaining time, start time, and finish time
        public Job(double runTime, double arrivalTime)
        {
            this.runTime = runTime;
            this.arrivalTime = arrivalTime;
            reset();
        }

        // getters for each field
        public double RunTime
        {
            get { return runTime; }
        }
        public double ArrivalTime
        {
            get { return arrivalTime; }
        }
        public double RemainingTime
        {
            get { return remainingTime; }
        }
        public double StartTime
        {
            get { return startTime; }
        }
        public double EndTime
        {
            get { return endTime; }
        }

        // boolean properties to check if job has finished and started
        public bool Finished
        {
            get { return remainingTime == 0; }
        }
        public bool Started
        {
            get { return remainingTime < runTime; }
        }

        // given a time, compares to arrival time and returns true if the job has arrived
        // Parameters:
        //       double currentTime  - current time passed from jobset
        public bool ArrivedAt(double currentTime)
        {
            return arrivalTime <= currentTime;
        }

        // double properties to calculate and return response time and turnaround time
        public double Response
        {
            get { return startTime - arrivalTime; }
        }
        public double Turnaround
        {
            get { return endTime - arrivalTime; }
        }

        // Simulates executing the job for a given length of time
        // Parameters:
        //      ref double currentTime  - current time passed from jobset, called by reference so alterations to the time are persistant
        //      double amount           - amount of time to execute job for, default value of -1 signifies full execution
        // Returns true if job is finished and false otherwise
        public bool Execute(ref double currentTime, double exectutionTime = -1)
        {
            // Checks whether job has already been started
            if (!Started)
            {
                // if job has arrived, set start time to timer
                if (ArrivedAt(currentTime))
                    startTime = currentTime;
                // If job has not arrived, set start time and timer to arrival time
                else
                {
                    startTime = arrivalTime;
                    currentTime = arrivalTime;
                }
            }
            // If job is to be fully executed or execution time exceed remaining time; increase timer by remaining time, then set end time to timer, remaining time to 0, and return true
            if (exectutionTime < 0 || exectutionTime >= remainingTime)
            {
                currentTime += remainingTime;
                endTime = currentTime;
                remainingTime = 0;
                return true;
            }
            // if execution time is less than remaining time; increase timer by execution time, decrease remaining time by execution time, and return false
            else
            {
                currentTime += exectutionTime;
                remainingTime -= exectutionTime;
                return false;
            }
        }

        // resets job to initial state: remaining time = run time and start/end time = sentinal value (-1)
        public void reset()
        {
            remainingTime = runTime;
            startTime = -1;
            endTime = -1;
        }
    }
}
