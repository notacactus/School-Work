/*  Program:    COIS 3380 Lab 3
 *  Author:     Ryland Whillans
 *  SN:         0618437
 *  Date:       2019-02-26
 *  Purpose:    
 *      Accepts a directory path (or uses current directory if none supplied), and outputs formatted information about files in the directory based on arguments passed
 *      Arguments:
 *          -i - inode number
 *          -p - access permissions
 *          -a - last access date
 *          -m - last modification date
 *          -s [number] - files with size under given number
 *          -b [number] - files with size over given number
 *          -d - show directories
 *          [path] - path to directory to use
 *  Dependancies:
 *      See include statements
 *  Software/Language:
 *      C
 */

#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <time.h>
#include <dirent.h>
#include <errno.h>
#include <unistd.h>

#define MAX_PATH_SIZE 4096  // Maximum size of file path
#define MAX_ARG_FLAGS 9     // Maximum number of arguments excluding the directory path
#define TRUE 1              // Value for true
#define FALSE 0             // Value for false

// Function prototypes, parse arguments and print file data based on arguments respectively
int ParseArgs(int argc, char *argv[], int argFlags[], char directoryPath[]);
int PrintFile(char *fileName, char *directoryPath, int argFlags[], int *fileCount);

int main(int argc, char *argv[])
{
    DIR *dp;                            // Directory stream  
    struct dirent *dir;                 // directory entry
    char directoryPath[MAX_PATH_SIZE];  // path to directory
    int argFlags[MAX_ARG_FLAGS];        // array to hold values for flags and their arguments [i, p, a, m, s, s argument, b, b argument, d]
    int fileCount = 0;                  // counter for number of valid files found

    // Attemps to parse arguments and exits with code 1 if failed
    if (ParseArgs(argc, argv, argFlags, directoryPath) == -1)
    {
        printf("Error parsing arguments\n");
        exit(1);
    }

    // Attempts to open directory with given path, exits with code 2 if failed
    printf("Contents of Directory: %s\n", directoryPath);
    if ((dp = opendir(directoryPath)) == NULL)
    {
        perror("Error opening directory");
        exit(2);
    }

    // Outputs headers based on flags
    printf("%s%s%s%sSize      Filename\n", (argFlags[0] == TRUE ? "Inode   " : ""), (argFlags[1] == TRUE ? "Permissions  " : ""), (argFlags[2] == TRUE ? "Last Access Date     " : ""), (argFlags[3] == TRUE ? "Last Modification Date  " : ""));
    // Traverses directory and attempts to output file information based on flags, exits with code 3 if failed
    while ((dir = readdir(dp)) != NULL)
    {
        if (PrintFile(dir->d_name, directoryPath, argFlags, &fileCount) == -1)
        {
            perror("Error printing file");
            exit(3);
        }
    }
    // Outputs number of directories found
    printf("%i directory entries found\n", fileCount);
    closedir(dp);
    exit(0);
}

// Parses arguments passed to the program and sets their values in an array, determines if directory was passed as argument and stores it or uses current directory if not
// Parameters:
//      int argc                - number of arguments passed
//      char *argv[]            - array containing all passed arguments
//      int argFlags[]          - array of flags to set based on passed arguments
//      char directoryPath[]    - string to contain directory path   
// Returns: 0 on success, -1 on failure    
int ParseArgs(int argc, char *argv[], int argFlags[], char directoryPath[])
{
    int arg;    // current argument
    int i;      // loop iterator
    // Sets all flags to false
    for (i = 0; i < 9; i++)
        argFlags[i] = FALSE;

    // loops through arguments and sets flags and associated argument values, values for flags with arguments stored in following array index
    while ((arg = getopt(argc, argv, "is:b:pamd")) != -1)
    {
        switch (arg)
        {
        case 'i':
            argFlags[0] = TRUE;
            break;
        case 'p':
            argFlags[1] = TRUE;
            break;
        case 'a':
            argFlags[2] = TRUE;
            break;
        case 'm':
            argFlags[3] = TRUE;
            break;
        case 's':
            argFlags[4] = TRUE;
            argFlags[5] = atoi(optarg);
            break;
        case 'b':
            argFlags[6] = TRUE;
            argFlags[7] = atoi(optarg);
            break;
        case 'd':
            argFlags[8] = TRUE;
            break;
        default:
            return (-1);
        }
    }
    // if all arguments corresponded to flags, set path to current directory
    if (optind == argc)
        strncpy(directoryPath, ".", MAX_PATH_SIZE);
    // if one argument remains, use as path
    else if (optind == argc - 1)
        strncpy(directoryPath, argv[optind], MAX_PATH_SIZE);
    // if multiple remain, fails
    else
        return (-1);
    return (0);
}

// Determines if file should be printed based on flag settings and if so, outputs file data based on flags
// Parameters:

//      char *fileName          - string containing file name   
//      char *directoryPath     - string containing directory path   
//      int argFlags[]          - array of flags to determine what to print
//      int *fileCount          - counts if file was printed
// Returns: 0 on success, -1 on failure , 1 if file not needed
int PrintFile(char *fileName, char *directoryPath, int argFlags[], int *fileCount)
{
    struct stat sb;                 // contains file information
    char filePath[MAX_PATH_SIZE];   // string to contain file path 
    char formatTime[20];            // string to store formatted time stamp
    
    // constructs file path out of directory path and file name
    strncpy(filePath, directoryPath, MAX_PATH_SIZE);
    strncat(filePath, "/", 2);
    strncat(filePath, fileName, MAX_PATH_SIZE);

    // attempts to get file info, exits if failed
    if (stat(filePath, &sb) == -1)
        return (-1);
    // Checks if file needs to print and exits method if not
    if ((strcmp(fileName, ".") == 0 || strcmp(fileName, "..") == 0) ||  // does not print . and .. directory
        (S_ISDIR(sb.st_mode) && argFlags[8] == FALSE) ||                // does not print directories if d flag not set
        (argFlags[4] == TRUE && sb.st_size > argFlags[5]) ||            // does not print if over s flag size
        (argFlags[6] == TRUE && sb.st_size < argFlags[7]))              // does not print if under b size flag
        return (1);
    // increase count if file prints
    *fileCount += 1;
    // print inode if i flag set
    if (argFlags[0] == TRUE)
        printf("%-8i", (int)sb.st_ino);
    // print permissions if p flag set
    if (argFlags[1] == TRUE)
        printf("%s%s%s%s%s%s%s%s%s    ", (sb.st_mode & S_IRUSR ? "r" : "-"), (sb.st_mode & S_IWUSR ? "w" : "-"), (sb.st_mode & S_IXUSR ? "x" : "-"),
                                         (sb.st_mode & S_IRGRP ? "r" : "-"), (sb.st_mode & S_IWGRP ? "w" : "-"), (sb.st_mode & S_IXGRP ? "x" : "-"),
                                         (sb.st_mode & S_IROTH ? "r" : "-"), (sb.st_mode & S_IWOTH ? "w" : "-"), (sb.st_mode & S_IXOTH ? "x" : "-"));
    // format and print access time if a flag set
    if (argFlags[2] == TRUE)
    {
        strftime(formatTime, 20, "%Y-%m-%d %H:%M:%S", localtime(&sb.st_atime));
        printf("%-21s", formatTime);
    }
    // format and print modify time if m flag set
    if (argFlags[3] == TRUE)
    {
        strftime(formatTime, 20, "%Y-%m-%d %H:%M:%S", localtime(&sb.st_mtime));
        printf("%-24s", formatTime);
    }
    // print size
    printf("%-10i", (int)sb.st_size);
    // print file name, in [] if directory 
    printf("%s%s%s\n", S_ISDIR(sb.st_mode) ? "[" : "", fileName, S_ISDIR(sb.st_mode) ? "]" : "");
    return (0);
}