using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Longest_Improvement
{
    class Program
    {
        /*
           A student's performance in lab activities should always improve, but that is not always the case.
            Since progress is one of the most important metrics for a student, let’s write a program that computes the longest period of increasing performance for any given student.
            For example, if his grades for all lab activities in a course are: 9, 7, 8, 2, 5, 5, 8, 7 then the longest period would be 4 consecutive labs (2, 5, 5, 8).
            Given an array with the lab grades of a student
            Your task is to
            write a function that computes and prints to standard output (stdout) the longest period of increasing performance for the student that has these grades
            Note that your function will receive the following arguments:
            grades
            which is an array containing the grades of the student
            Data constraints
            the length of the array given as input will not exceed 1000 elements
        */

        public static void longest_improvement(params int[] grades)
        {
            int highest = 0;
            int current = 1;
            int prev = grades[0];
            for (int i = 1; i < grades.Length; i++)
            {
                if (grades[i] >= prev)
                {
                    current++;
                }
                else if (current > highest)
                {
                    highest = current;
                    current = 1;
                }
                prev = grades[i];
            }
            Console.WriteLine(highest);
        }

        static void Main(string[] args)
        {
            longest_improvement(9, 7, 8, 2, 5, 5, 8, 7); // 4
            Console.ReadKey();
        }
    }
}
