using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FizzBuzz
{
    class Program
    {
        /*
           Given an integer number N
            Your task is to
            write a function that prints to the standard output (stdout) the numbers from 1 to N (one per line) with the following restrictions
            for multiples of three print “Fizz” instead of the number
            for the multiples of five print “Buzz” instead of the number
            for numbers which are multiples of both three and five print “FizzBuzz”
            Note that your function will receive the following arguments:
            n
            which is the integer number described above
            Data constraints
            the maximum value of N will not exceed 1000
            Efficiency constraints
            your function is expected to print the result in less than 2 seconds
         */

        public static void fizzbuzz(int n)
        {
            for (int i = 1; i <= n; i++)
            {
                if (i % 3 == 0 && i % 5 != 0)
                {
                    Console.WriteLine("Fizz");
                }
                else if (i % 3 != 0 && i % 5 == 0)
                {
                    Console.WriteLine("Buzz");
                }
                else if (i % 3 == 0 && i % 5 == 0)
                {
                    Console.WriteLine("FizzBuzz");
                }
                else
                {
                    Console.WriteLine(i);
                }
            }

        }

        static void Main(string[] args)
        {
            fizzbuzz(15); // fizz fizz buzz buzz oh what a relief it is.
            Console.ReadKey();
        }
    }
}
