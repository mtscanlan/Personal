using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coins
{
    class Program
    {
        /*
           You are given a list of coin values [V1, V2, ..., VN] and a required change E.
            Your task is to write an algorithm that prints to the standard output (stdout) the minimum number of coins required to give the change E.
            Notes:
            the coins are sorted in ascending order by their value.
            it is guaranteed that the change can be obtained i.e. there won't be any tests like V: [10, 50], E: 75.
            Expected complexity: O(E * N)
            Example input:
            V: [1, 5, 10]
            E: 28
            Example output:
            6
        */

        public static void minimum_coins(int e, params int[] v)
        {
            int originalE = e, totalCoins = 0, offset = 1;
            for (int i = v.Length - offset; i >= 0; i--)
            {
                totalCoins += e / v[i];
                e %= v[i];
                if (i == 0 && e != 0)
                {
                    offset++;
                    i = v.Length - offset;
                    e = originalE;
                    totalCoins = 0;
                }
            }
            Console.WriteLine(totalCoins);
        }

        static void Main(string[] args)
        {
            minimum_coins(28, 1, 5, 10); // 6
            Console.ReadKey();
        }
    }
}
