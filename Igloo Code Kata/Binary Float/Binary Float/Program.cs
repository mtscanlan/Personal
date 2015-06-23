using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Binary_Float
{
    class Program
    {
        /*
            Given a binary string containing a fractional part your task is to print to the standard output (stdout) its numeric value (as a float).
            Example input:
            s: 100.0011
            Example output:
            4.1875
        */

        public static double print_float2(string s)
        {
            string[] parts = s.Split('.');
            int ip = Convert.ToInt32(parts[0], 2);
            int fp = Convert.ToInt32(parts[1], 2) << 1;

            return (double)ip + ((double)fp / (2 << parts[1].Length));
        }

        public static double print_float(string s)
        {
            string[] parts = s.Split('.');
            double total = 0;
            char[] integerPart = parts[0].ToCharArray(), fractionPart = parts[1].ToCharArray();

            int multiple = 2 << integerPart.Length - 1;
            foreach (char c in integerPart)
            {
                multiple = multiple >> 1;
                int x = c - '0'; // assumption that integerPart will be numeric
                total += x * multiple;
            }

            foreach (char c in fractionPart)
            {
                multiple = multiple << 1;
                int x = c - '0'; // assumption that integerPart will be numeric
                total += (float)x / multiple;
            }
            return total;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("{0}", print_float2("100.0011")); // 4.1875
            Console.WriteLine("{0}", print_float("100.0011")); // 4.1875
            Console.ReadKey();
        }
    }
}
