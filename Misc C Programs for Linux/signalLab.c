/*  Program:    COIS 3380 Lab 5
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-03-20
 *  Purpose:    
 *      Demonstrates usage of signal hangler functions
 *      Counts number of SIGINT sent to program and exits by forking and sending signal to parent when SIGQUIT recieved
 *  Dependancies:
 *      See include statements
 *  Software/Language:
 *      C
 */

#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/wait.h>
#include <signal.h>

#define MAX_SIGINT 5 // Maximum number of SIGINT/ctrl-c presses that will reinstall function handler

void SigIntHandler(int signo);  // Function handler for SIGINT
void SigQuitHandler(int signo); // Function handler for SIGQUIT
void SigUsr1Handler(int signo); // Function handler for SIGUSR1
int ForkWrapper();              // Wrapper for fork

int sigIntCount = 0;            // counter for number of times ctrl-c pressed

int main(int argc, char *argv[])
{
    // setting up function handlers for SIGINT, SIGQUIT, and SIGUSR1
    if (signal(SIGINT, SigIntHandler) == SIG_ERR)
        printf("Error installing SIGINT handler\n");
    if (signal(SIGQUIT, SigQuitHandler) == SIG_ERR)
        printf("Error installing SIGQUIT handler\n");
    if (signal(SIGUSR1, SigUsr1Handler) == SIG_ERR)
        printf("Error installing SIGUSR1 handler\n");
    // Waits for signals infinitely
    while (1)
    {
        printf("Waiting for a signal...\n");
        pause();
    }
    exit(0);
}

// Function handler for SIQINT/ctrl-c
// Increments counter and if counter exceeded
void SigIntHandler(int signo)
{
    // increment counter and output number of times pressed
    sigIntCount++;
    printf(" Times pressed ctrl-c: %i\n", sigIntCount);
    // if max exceeded output message and set SIGINT handler to default
    if (sigIntCount > MAX_SIGINT)
    {
        printf("Max SIGINT count exceeded\n");
        if (signal(SIGINT, SIG_DFL) == SIG_ERR)
            printf("Error resetting SIGINT handler\n");
    }
}

// Function handler for SIQQUIT
// forks and sends SIGUSR1 to parent
void SigQuitHandler(int signo)
{
    pid_t pid;
    // forks then child sends SIGURS1 to parent and exits
    if ((pid = ForkWrapper()) == 0)
    {
        printf(" In child process for SIGQUIT handler\n");
        if (kill(getppid(), SIGUSR1) == -1)
            printf("Error sending SIGUSR1\n");
        exit(0);
    }
    //parent waits for child to finish
    if (waitpid(pid, NULL, 0) == -1)
    {
        printf("Error while waiting\n");
        exit(1);
    }
    return;
}

// Function handler for SIGUSR1
// outputs number of time ctrl-c pressed and exits
void SigUsr1Handler(int signo)
{
    printf("The program has ended. Times pressed ctrl-c: %i\n", sigIntCount);
    exit(0);
}

// Wrapper for fork function
// outputs error if fork fails
// otherwise returns pid
int ForkWrapper()
{
    pid_t pid;
    if ((pid = fork()) < 0)
    {
        perror("fork failed");
        exit(1);
    }
    return pid;
}