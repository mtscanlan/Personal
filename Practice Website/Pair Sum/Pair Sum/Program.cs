using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pair_Sum
{
    class Program
    {
        /*
           Given an array of integer numbers and a value S
            Your task is to
            write a function that prints to the standard output "1" if two numbers from the given array add up to S or "0" otherwise
            Note that your function will receive the following arguments:
            v
            the array of integer numbers mentioned above
            s
            an integer number representing the S value described above
            Data constraints
            the length of the array will not exceed 10,000
        */

        public static void two_numbers_sum(int sum, params int[] v)
        {
            for (int i = 0; i < v.Length; i++)
            {
                for (int j = 0; j < v.Length; j++)
                {
                    if (i == j) break;
                    if (v[i] + v[j] == sum)
                    {
                        Console.WriteLine(1);
                        return;
                    }
                }
            }
            Console.WriteLine(0);
        }

        static void Main(string[] args)
        {
            two_numbers_sum(4, 2, 2, 20); // 1
            two_numbers_sum(5, 2, 2, 20); // 0
            Console.ReadKey();
        }
    }
}
