using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Progress
{
    class Program
    {
        /*
           Students are graded for their activity in each lab session.
            It is expected that a student’s performance improves in time, which means that he should always receive a grade equal to or higher than the grade he received in the previous lab.
            Given an array with the lab grades of a student
            Your task is to
            write a function that checks whether the grades received by each student are in ascending order
            your function must print to standard output (stdout):
            1 if the grades are in ascending order (e.g. 1, 3, 3, 7)
            0 if the grades are not in ascending order (e.g. 1, 3, 7, 3)
            Note that your function will receive the following arguments:
            grades
            which is an array containing the grades of the student
            Data constraints
            the length of the array given as input will not exceed 1000 elements
        */

        public static void is_sorted(params int[] grades)
        {
            int prev = 0;
            int returnVal = 1;
            for (int i = 0; i < grades.Length; i++)
            {
                if (grades[i] < prev)
                {
                    returnVal = 0;
                    break;
                }
                prev = grades[i];
            }
            Console.WriteLine(returnVal);
        }

        static void Main(string[] args)
        {
            is_sorted(1, 3, 3, 7); // 1
            Console.ReadKey();
        }
    }
}
