using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Relative_Sort
{
    class Program
    {
        /*
           Given an array of integer numbers your task is to print to the standard output (stdout) the initial array, but sorted in a special way:
            all negative numbers come first and their relative positions according to the initial array do not change
            the same with the positive integers, but they come last
            Expected complexity: O(N) time, extra memory O(1)
            Example input:
            -5 2 1 -2 3
            Example output:
            -5 -2 2 1 3
        */

        public static void relative_sort(params int[] v)
        {
            for (int j = 0; j < v.Length; j++)
            {
                if (v[j] < 0)
                {
                    Console.Write(v[j] + " ");
                }
            }
            for (int j = 0; j < v.Length; j++)
            {
                if (v[j] >= 0)
                {
                    Console.Write(v[j] + " ");
                }
            }
        }

        static void Main(string[] args)
        {
            relative_sort(-5, 2, 1, -2, 3); // -5 -2 2 1 3
            Console.ReadKey();
        }
    }
}
