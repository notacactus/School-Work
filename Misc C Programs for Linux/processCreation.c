/*  Program:    COIS 3380 Lab 4
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-03-13
 *  Purpose:    
 *      Demonstates the usage of fork and exec by creating various child processes and executing different tasks within each process
 *      will sleep for 5 in second child if any arguments passed
 *  Dependancies:
 *      See include statements
 *  Software/Language:
 *      C
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <fcntl.h>
#include <errno.h>
#include <unistd.h>
#include <sys/types.h>
#include <sys/wait.h>

int ForkWrapper();  // Wrapper for fork
int ReadCopy();     // Reads and copies file

// Creates 2 child and one grandchild process and executes different task in each
// if any arguments are passed, will sleep 5 in second child
int main(int argc, char *argv[])
{
    pid_t pid1, pid2, firstReturn;                                              // pids of first and second child, pid of first child to return
    char *headArgs[] = {"head", "-n", "20", "processCreation.c", (char *)0};    // arguments for exec function call of head
    int fileSize;                                                               // size of copied file
    
    // forks first child
    if ((pid1 = ForkWrapper()) == 0)
    {
        printf("\tI am the first child\n");
        // forks grandchild
        if ((pid1 = ForkWrapper()) == 0)
        {
            printf("\t\t\tI am the grandchild\n");
            sleep(3);
            // execs head function
            execv("/bin/head", headArgs);
            // output error if exec failed (still in process)
            perror("exec failed");
            exit(2);
        }
        printf("\tI am the first child, grandchild has pid %i\n", pid1);
        // waits for grandchild to finish
        while (waitpid(pid1, NULL, WNOHANG) == 0)
        {
            printf("\tFirst child: Try again\n");
            sleep(1);
        }
        exit(0);
    }
    printf("I am the parent, first child has pid %i\n", pid1);

    // forks second child
    if ((pid2 = ForkWrapper())  == 0)
    {
        printf("\t\tI am the second child\n");
        // calls function to copy file and outputs returned file size
        fileSize = ReadCopy();
        printf("\t\tSecond child: %i bytes transferred\n", fileSize);
        // if any arguments passed, sleep for 5
        if (argc > 1)
            sleep(5);
        exit(0);
    }
    printf("I am the parent again, second child has pid %i\n", pid2);
    
    // waits for either child to return and outptus pid
    if ((firstReturn = waitpid(-1, NULL, 0)) == -1)
    {
        printf("Error while waiting");
        exit(3);
    }
    printf("Parent - Child %i is done\n", firstReturn);
    // waits for other child to return and outputs pid
    if ((firstReturn = waitpid((firstReturn == pid1 ? pid2 : pid1), NULL, 0)) == -1)
    {
        printf("Error while waiting");
        exit(3);
    }
    printf("Parent - Other child %i is done\n", firstReturn);
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

// Reads the file specified by the assignment and copies it to a new file
// Returns the size of the file copied
int ReadCopy()
{
    int inputFd, outputFd, count;   // file descriptors for input file and copy file, number of characters read
    ssize_t numRead;                // number of characters read in current read operation
    char buffer[100];               // buffer for reading
    // opens file
    if ((inputFd = open("/home/COIS/3380/lab4_sourcefile", O_RDONLY)) == -1)
    {
        perror("Error opening input file");
        exit(3);
    }
    // opens/creates file to copy to
    if ((outputFd = open("lab4_sourcefile_copy", O_WRONLY | O_CREAT | O_TRUNC, 0644)) == -1)
    {
        perror("Error opening output file");
        close(inputFd);
        exit(3);
    }

    count = 0;
    // reads file and writes to copy file
    while ((numRead = read(inputFd, buffer, 100)) > 0)
    {
        if (write(outputFd, buffer, numRead) < numRead)
        {
            perror("Error writing to file");
            close(inputFd);
            close(outputFd);
            exit(2);
        }
        // increments count by amount read
        count += numRead;
    }
    close(inputFd);
    close(outputFd);
    // returns total number of characters read (file size)
    return count;
}