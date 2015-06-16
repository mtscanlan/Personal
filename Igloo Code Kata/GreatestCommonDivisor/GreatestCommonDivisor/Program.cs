using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatestCommonDivisor
{
    class Program
    {
        public static Dictionary<uint[], uint> collections = new Dictionary<uint[], uint>() {
           {new uint[] { 10, 15, 15 }, 5},
           {new uint[] { 141, 752, 235 }, 47},
           {new uint[] { 9638, 4758, 10248, 5490 }, 122},
           {new uint[] { 25, 525 }, 25},
           {new uint[] { 60032, 181888, 555520, 386176 }, 896},
           {new uint[] { 1540, 1364, 1628 }, 44},
           {new uint[] { 1088376, 1733589 }, 1347},
           {new uint[] { 46733280, 7398270, 57472140, 5308215 }, 8745},
           {new uint[] { 230000, 3033700, 5099100 }, 2300},
           {new uint[] { 29108200, 8454710, 8543590, 21114555 }, 5555},
           {new uint[] { 6873, 44556, 17775 }, 237}
        };
    

        public static uint PairGCD(uint value1, uint value2) {
            while (value1 != 0 && value2 != 0)
                if (value1 > value2)
                    value1 %= value2;
                else
                    value2 %= value1;

            return value1 > value2 ? value1 : value2;
        }

        public static uint GCD(params uint[] numbers) {
            uint currentGCD = 0;
            if (numbers.Length > 0) {
                currentGCD = numbers[0];
                for (int i = 1; i < numbers.Length; i++) {
                    // Return greatest common divisor for all numbers in the provided array.
                    uint thisGCD = PairGCD(currentGCD, numbers[i]);
                    currentGCD = thisGCD < currentGCD ? thisGCD : currentGCD;
                    if (currentGCD == 1) break;
                }
            }
            return currentGCD;
        }


        static void Main(string[] args) {
            int iterations = 1000000;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < iterations; i++)
                foreach (var testData in collections)
                    Debug.Assert(GCD(testData.Key) == testData.Value);
            sw.Stop();
            Console.Write("Elapsed = {0}s",sw.ElapsedMilliseconds / 1000f);
            Console.ReadKey();
        }
    }
}
