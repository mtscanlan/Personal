using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sorted_Merge
{
    class Program
    {
        /*
           Given 2 sorted arrays, merge them into one single sorted array and print its elements to standard output.
            Expected complexity: O(N)
            Example input:
            a: 2 3 7 8 8
            b: 7 8 13
            Example output:
            2 3 7 7 8 8 8 13
         */

        public static void merge_arrays(int[] a, int[] b)
        {
            int referenceA = 0, referenceB = 0;
            int size = a.Length + b.Length;
            for (int i = 0; i < size; i++)
            {
                if (referenceA < a.Length && a[referenceA] <= b[referenceB])
                {
                    Console.Write(a[referenceA++] + " ");
                }
                else if (referenceB < b.Length)
                {
                    Console.Write(b[referenceB++] + " ");
                }
            }
        }

        static void Main(string[] args)
        {
            int[] x = {2, 3, 7, 8, 8};
            int[] y = {7, 8, 13};
            merge_arrays(x, y); // 2 3 7 7 8 8 8 13
            Console.ReadKey();
        }
    }
}
