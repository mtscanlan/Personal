using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsCoprime
{
    class Program
    {
        static bool IsCoprime(int value1, int value2)
        {
            while (value1 != 0 && value2 != 0)
            {
                if (value1 > value2)
                    value1 %= value2;
                else
                    value2 %= value1;
            }
            return Math.Max(value1, value2) == 1;
        }

        static void Main(string[] args)
        {
            int[] A = { 1, 2, 3, 4, 5, 6, 7 };
            int[] outCoprimes = new int[A.Length - 1];
            for (int i = 1; i < A.Length; i++)
            {
                int numMatches = 0;
                for (int j = 0; j < A[i]; j++)
                {
                    if (IsCoprime(A[i], j)) numMatches++;
                }
                outCoprimes[i - 1] = numMatches;
            }
            Console.WriteLine(string.Join(",", outCoprimes));
            Console.ReadKey();
        }
    }
}
