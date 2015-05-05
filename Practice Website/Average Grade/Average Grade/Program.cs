using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Average_Grade
{
    class Program
    {
        /*
           An easy way to understand how well the students performed at this year’s course is to compute the average of their final grades. If it is higher that the average for last year, that means the course was easier than last year.
            Given an array with all final grades for a course
            Your task is to
            write a function that computes the average of all the grades in the array and prints this number to standard output (stdout)
            the result must be rounded downwards to the nearest integer (e.g. both 7.1 and 7.9 are rounded to 7)
            Note that your function will receive the following arguments:
            grades
            which is the list of grades, represented as integer numbers
            Data constraints
            the length of the array given as input will not exceed 1000 elements
        */

        public static void compute_average_grade(params int[] grades)
        {
            // Console.WriteLine(grades.Sum() / grades.Count());
            int count = grades.Length;
            int sum = 0;
            for (int i = 0; i < count; i++)
            {
                sum += grades[i];
            }
            Console.WriteLine(sum / count);
        }

        static void Main(string[] args)
        {
            compute_average_grade(1, 2, 8, 4, 5, 8, 3); // 4
            Console.ReadKey();
        }
    }
}
