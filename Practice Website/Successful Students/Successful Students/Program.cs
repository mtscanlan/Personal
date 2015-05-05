using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Successful_Students
{
    class Program
    {
        /*
           Given an array with all final grades for a course and the minimum grade that a student needs to have in order to pass the course
            Your task is to
            write a function that counts the number of students that passed and prints this number to standard output (stdout)
            Note that your function will receive the following arguments:
            grades
            which is the list of grades, represented as integer numbers
            min_grade
            which is the minimum grade that a student can get, so that he passes the course
            Data constraints
            the length of the array given as input will not exceed 1000 elements
        */

        public static void count_successful_students(int min_grade, params int[] grades)
        {
            int result = grades.Where(x => x >= min_grade).ToList().Count;
            Console.WriteLine(result);
        }

        static void Main(string[] args)
        {
            count_successful_students(5, 1, 2, 8, 4, 5, 8, 3); // 3
            Console.ReadKey();
        }
    }
}
