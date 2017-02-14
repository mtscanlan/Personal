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

        public static string FizzBuzz(int n)
        {
            string value = n.ToString();

            if (n % 15 == 0)
                value = "FizzBuzz";
            else if (n % 3 == 0)
                value = "Fizz";
            else if (n % 5 == 0)
                value = "Buzz";

            return value;
        }

        private static void DisplayFizzBuzz(int v, bool reverse)
        {
            var iteration = (reverse ?
                Enumerable.Range(1, v).Select(FizzBuzz).Reverse() :
                Enumerable.Range(1, v).Select(FizzBuzz));

            foreach (var value in iteration)
            {
                Console.WriteLine(value);
            }
        }

        static void Main(string[] args)
        {
            DisplayFizzBuzz(105, reverse: false);
            Console.ReadKey();
        }
    }
}
