using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Missing_Number
{
    class Program
    {
        /*
           Given an array containing all numbers from 1 to N with the exception of one print the missing number to the standard output.
            Example input:
            array: 5 4 1 2
            Example output:
            3
            Note: This challenge was part of Microsoft interviews. The expected complexity is O(N).
        */

        public static void find_missing_number(params int[] v)
        {
            int sum = 0;
            int givenSum = v.Length + 1;
            for (int i = 0; i < v.Length; i++)
            {
                sum += v[i];
                givenSum += i + 1;
            }
            Console.WriteLine(givenSum - sum);
        }

        static void Main(string[] args)
        {
            find_missing_number(5, 4, 1, 2); // 3
            Console.ReadKey();
        }
    }
}
