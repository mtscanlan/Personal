using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pair_Product
{
    class Program
    {
        /*
           Write to the standard output the greatest product of 2 numbers to be divisible by 3 from a given array of pozitive integers.
Example input:
6, 8, 8, 7, 2, 5
Example output:
48
        */

        public static void max_prod(params int[] v)
        {
            int greatestProduct = 0;
            for (int i = 0; i < v.Length; i++)
            {
                for (int j = 0; j < v.Length; j++)
                {
                    if (i == j) continue;
                    int number = v[i] * v[j];
                    if (number > greatestProduct && number % 3 == 0)
                    {
                        greatestProduct = number;
                    }
                }
            }
            Console.WriteLine(greatestProduct);

        }

        static void Main(string[] args)
        {
            max_prod(6, 8, 8, 7, 2, 5); // 48
            Console.ReadKey();
        }
    }
}
