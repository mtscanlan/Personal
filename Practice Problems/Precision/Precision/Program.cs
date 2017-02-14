using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Precision
{
    class Program
    {
        /*
           Sometimes you want to do calculations maintaining precision. For example, if you have 95% and 5% of a given number, those numbers should add back to the original despite any rounding that needs to occur.
            Given two percentages p1, p2 and an integer number v your task is to print to the standard output two integer numbers, the first one representing p1 percent from v and the second one p2 percent from v. The two percentage numbers will always add up to 1.
            If rounding is necessary, round the first number half up (.5 and up goes to 1) and ensure total of calculated values is total given value v originally.
            Example input:
            0.95 0.05 100
            Example output:
            95 5
            Example input:
            0.5 0.5 1
            Example output:
            1 0
        */

        public static void precision(double p1, double p2, int v)
        {
            int averageOne = 0, averageTwo = 0;
            averageOne = (int)(p1 * (double)v + 0.5);
            averageTwo = (int)(p2 * (double)v + 0.49);
            Console.WriteLine(averageOne + " " + averageTwo);
        }

        static void Main(string[] args)
        {
            precision(0.95, 0.05, 100); // 95 5
            precision(0.5, 0.5, 1); // 1 0
            Console.ReadKey();
        }
    }
}
