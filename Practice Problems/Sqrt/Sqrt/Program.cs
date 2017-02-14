using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sqrt
{
    class Program
    {
        /*
           Given an integer number N, compute its square root without using any math library functions and print the result to standard output. Please round the result downwards to the nearest integer (e.g both 7.1 and 7.9 are rounded to 7)
            Expected complexity: O(logN), O(1)
            Example input:
            N: 17
            Example output:
            4
        */

        public static void compute_sqrt(int n)
        {
            int guess = 2;
            int currentRoot = 0;
            while (true)
            {
                int root = n / guess;
                if (root == currentRoot)
                {
                    break;
                }
                currentRoot = root;
                guess = (root + guess) / 2;
            }
            Console.WriteLine(currentRoot);
        }

        static void Main(string[] args)
        {
            compute_sqrt(17); // 4
            Console.ReadKey();
        }
    }
}
