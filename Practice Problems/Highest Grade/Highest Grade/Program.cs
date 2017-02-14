using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Highest_Grade
{
    class Program
    {
        /*
           Given an array with all final grades for a course
            Your task is to
            write a function that finds the highest grade and prints this grade to standard output (stdout)
            Note that your function will receive the following arguments:
            grades
            which is the list of grades, represented as integer numbers
            Data constraints
            the length of the array given as input will not exceed 1000 elements
        */

        public static void max_grade(params int[] grades)
        {
            int max = -1;
            for (int i = 0; i < grades.Length; i++)
            {
                max = grades[i] > max ? grades[i] : max;
            }
            Console.WriteLine(max);
        }

        static void Main(string[] args)
        {
            max_grade(1, 2, 8, 4, 5, 8, 3); // 8
            Console.ReadKey();
        }
    }
}
