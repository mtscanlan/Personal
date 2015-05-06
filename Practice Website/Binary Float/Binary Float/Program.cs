using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void print_float(string s) // My way, just as fast.
        {
            string[] parts = s.Split('.');
            double total = 0;
            int integerPartLength = parts[0].Length;
            char[] a = parts[0].ToCharArray();
            char[] b = parts[1].ToCharArray();
            double power = Math.Pow(2, integerPartLength - 1);
            for (int i = 0; i < integerPartLength; i++)
            {
                int x = a[i] - 48;
                total += (int)(x * power);
                power /= 2;
            }
            int fractionPartLength = parts[1].Length;
            power = 2;
            for (int i = 0; i < fractionPartLength; i++)
            {
                int x = b[i] - 48;
                total += x / power;
                power *= 2;
            }
            Console.WriteLine("{0}", total);
        }

        public static void print_float2(string s) // top solution
        {
            string[] parts = s.Split('.');
            int ip = Convert.ToInt32(parts[0], 2);
            int fp = Convert.ToInt32(parts[1], 2) << 1;

            double total = (double)ip + ((double)fp / (2 << parts[1].Length));
            Console.WriteLine("{0}", total);
        }


        static void Main(string[] args)
        {
            print_float("100.0011"); // 4.1875
            print_float2("100.0011"); // 4.1875
            Console.ReadKey();
        }
    }
}
