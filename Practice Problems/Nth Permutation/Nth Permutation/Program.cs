using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nth_Permutation
{
    class Program
    {
        /*
           Given an array of integer numbers print to the standard output the nth circular permutation to the right.
            Expected complexity: O(N)
            The first 4 permutations to the right for the 7 1 2 array are: 2 7 1 -> 1 2 7 -> 7 1 2 -> 2 7 1
            Example input:
            array: 7 1 2
            N: 4
            Example output:
            2 7 1
        */

        public static void nth_perm(int n, params int[] v)
        {
            int mod = n % v.Length + 1;
            for (int i = mod; i < v.Length; i++)
            {
                Console.Write(v[i] + " ");
            }
            for (int i = 0; i < mod; i++)
            {
                Console.Write(v[i] + " ");
            }
        }

        static void Main(string[] args)
        {
            nth_perm(4, 7, 1, 2); // 2 7 1
            Console.ReadKey();
        }
    }
}
