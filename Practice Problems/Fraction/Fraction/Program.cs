using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fraction
{
    class Program
    {
        /*
           Given a rational, decimal value, write a function prints out the simplest possible fraction.
            The decimal value is given as a string value.
            Example input:
            D: 1.6
            Example output:
            8/5
        */

        public static void compute_fraction(string s)
        {
            double d = Convert.ToDouble(s);
            string[] split = "1.6".Split('.');
            double factor = split[1].Length;
            double multiply = Math.Pow(10.0, (double)factor);
            int returnVal = GCD((int)multiply, (int)(d * multiply));
            Console.WriteLine("{0}/{1}", (d * multiply) / returnVal, multiply / returnVal);
        }

        static int GCD(int a, int b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }

        static void Main(string[] args)
        {
            compute_fraction("1.6"); // 8/5
            Console.ReadKey();
        }
    }
}
