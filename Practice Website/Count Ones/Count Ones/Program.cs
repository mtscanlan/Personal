using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Count_Ones
{
    class Program
    {
        /*
            Given an integer N
            Your task is to
            write a function that prints to the standard output (stdout) the number of 1's present in its binary representation
            Note that your function will receive the following arguments:
            n
            which is the integer number N described above
        */

        public static void count_one(int a)
        {
            int numOnes = 0;
            while (a != 0)
            {
                numOnes += a % 2;
                a /= 2;
            }
            Console.WriteLine(numOnes);
        }

        static void Main(string[] args)
        {
            count_one(5); // 2
            Console.ReadKey();
        }
    }
}
