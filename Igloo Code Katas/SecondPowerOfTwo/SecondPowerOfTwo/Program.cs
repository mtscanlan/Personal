using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecondPowerOfTwo
{
    class Program
    {
        /* 
         * For this week's challenge, create a function that 
         * takes in a list of integers and returns the second-highest 
         * power of two from that list, or 0 if there are fewer 
         * than 2 powers of two. To change things up, the functions 
         * will be scored by how short their IL is. Check out LINQPad 
         * to get the IL from a C# function.
         */

        static void Main(string[] args)
        {
            Console.WriteLine(CalculateSecondHighestNumber(new int[] { 3, 5, 8, 1, 12, 16, 2, 11, 16, 64 }));
            Console.WriteLine(CalculateSecondHighestNumber(new int[] { }));
            Console.WriteLine(CalculateSecondHighestNumber(new int[] { 3, 4, 5, 6, 7, 8 }));
            Console.WriteLine(CalculateSecondHighestNumber(new int[] { 64, 64, 2 }));
            Console.WriteLine(CalculateSecondHighestNumber(new int[] { 1, 2 }));
            Console.WriteLine(CalculateSecondHighestNumber(new int[] { 3, 4, 5, 6, 7 }));
            Console.ReadKey();
        }

        public static int CalculateSecondHighestNumber(params int[] values)
        {
            int highest = 0, secondhighest = 0;
            for (int i = 0; i < values.Length; i++)
            {
                int number = values[i];
                while (((number & 1) == 0) && number > 1)
                {
                    number >>= 1;
                }

                if (number == 1 && values[i] > highest)
                {
                    secondhighest = highest;
                    highest = values[i];
                }
                else if (number == 1 && values[i] != highest && values[i] > secondhighest)
                {
                    secondhighest = values[i];
                }
            }
            return secondhighest;
        }
    }
}
