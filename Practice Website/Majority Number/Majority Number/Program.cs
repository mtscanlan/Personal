using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majority_Number
{
    class Program
    {
        /*
           Given an array of integer numbers your task is to print to the standard output (stdout) the majority number.
            One number is considered majority if it occurs more than N / 2 times in an array of size N.
            Note: If no number is majority then print "None"
            Expected complexity: O(N) time, O(1) memory
            Example input:
            1 1 2 3 4 1 6 1 7 1 1
            Example output:
            1
            Example input:
            1 2 2 3
            Example output:
            None
        */

        public static void majority(params int[] v)
        {
            int count = 0, i = 0, majorityElement = 0;
            for (i = 0; i < v.Length; i++)
            {
                if (count == 0)
                    majorityElement = v[i];
                if (v[i] == majorityElement)
                    count++;
                else
                    count--;
            }
            count = 0;
            for (i = 0; i < v.Length; i++)
                if (v[i] == majorityElement)
                    count++;
            if (count > v.Length / 2) Console.WriteLine(majorityElement);
            else Console.WriteLine("None");
        }

        static void Main(string[] args)
        {
            majority(1, 1, 2, 3, 4, 1, 6, 1, 7, 1, 1); // 1
            majority(1, 2, 2, 3); // None
            Console.ReadKey();
        }
    }
}
