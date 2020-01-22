/*  Program:    COIS 3380 Lab 2
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-02-13
 *  Purpose:    
 *      Takes a file and reverses the contents into a new file
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

// Maximum size for file name
#define MAXFILENAME 512

// Takes a file and reverses the contents into a new file
// Takes 2 arguments, file name/path of input and output files
int main(int argc, char *argv[])
{
    int inputFd;                    // File descrpitor for input file
    int outputFd;                   // File descrpitor for input file
    char inputFile[MAXFILENAME];    // Input file name/path
    char outputFile[MAXFILENAME];   // Output file name/path
    char buffer[1];                 // Buffer for copying from input to output
    int fileSize;                   // Size of input file
    int i;                          // for loop iterator

    // Checks if too few or too many arguments, exits with code 1 if so
    if (argc < 3)
    {
        printf("Too few parameters\n");
        return (1);
    }
    else if (argc > 3)
    {
        printf("Too many parameters\n");
        return (1);
    }

    // Copies filenames from arguments
    strncpy(inputFile, argv[1], MAXFILENAME);
    strncpy(outputFile, argv[2], MAXFILENAME);

    printf("Reversing file %s into file %s\n", inputFile, outputFile);

    // Opens input and output files, exits with code 2 if error occurs
    if ((inputFd = open(inputFile, O_RDONLY)) == -1)
    {
        perror("Error opening input file");
        return (2);
    }
    if ((outputFd = open(outputFile, O_WRONLY | O_CREAT | O_TRUNC, 0644)) == -1)
    {
        perror("Error opening output file");
        close(inputFd);
        return (2);
    }
    // Seek to end of input file and store size of file
    if ((fileSize = lseek(inputFd, 1, SEEK_END)) == -1)
    {
        perror("Error while seeking in input file");
        close(inputFd);
        close(outputFd);
        return (2);
    }

    // Iterates through input file in reverse and copies contents to output file, exits with code 3 if error occurs
    for (i = 0; i < fileSize - 1; i++)
    {
        // Seek backwards in input file by 2
        if (lseek(inputFd, -2, SEEK_CUR) == -1)
        {
            perror("Error while seeking in input file");
            close(inputFd);
            close(outputFd);
            return (3);
        }
        // read 1 char from input file
        if (read(inputFd, buffer, 1) == -1)
        {
            perror("Error while reading from input file");
            close(inputFd);
            close(outputFd);
            return (3);
        }
        // write char to output file
        if (write(outputFd, buffer, 1) == -1)
        {
            perror("Error while writing to output file");
            close(inputFd);
            close(outputFd);
            return (3);
        }
    }

    printf("Finished reversing file\n");

    // Close files and exit
    close(inputFd);
    close(outputFd);
    return (0);
}
