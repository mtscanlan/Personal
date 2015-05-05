using System;
using System.Collections.Generic;

namespace Common_Courses
{
    class Program
    {
        /*
           A teacher wants to compare the performance of two students. To understand them better, he’s looking at all the other courses they took, but it’s hard to spot the common courses just from a glance.
            Given two arrays that contain the course IDs of two different students
            Your task is to
            write a function that prints to standard output (stdout) all the course IDs that are contained in both arrays, sorted in ascending order
            Note that your function will receive the following arguments:
            courses1
            which is the list of course IDs for the first student
            courses2
            which is the list of course ids for the second student
            Data constraints
            the length of the array given as input will not exceed 1000 elements
        */

        public static void get_common_courses(int[] courses1, int[] courses2)
        {
            SortedSet<int> matchingCourses = new SortedSet<int>();
            for (int i = 0; i < courses1.Length; i++)
            {
                int courseFromOne = courses1[i];
                for (int j = 0; j < courses2.Length; j++)
                {
                    if (courseFromOne == courses2[j]) matchingCourses.Add(courseFromOne);
                }
            }
            Console.WriteLine(string.Join(" ", matchingCourses));
        }

        static void Main(string[] args)
        {
            int[] x = {1, 2, 8, 4, 5, 8, 3};
            int[] y = {8, 2, 2, 7, 10};
            get_common_courses(x, y); // 2 8
            Console.ReadKey();
        }
    }
}
