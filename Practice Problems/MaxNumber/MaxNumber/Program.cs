using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxNumber
{
    class Program
    {
        /*
            Given two integer numbers a and b
            Your task is to
            write a function that prints to the standard output (stdout) the maximum value between the two
            Note that your function will receive the following arguments:
            a
            which is the integer number a described above
            b
            which is the integer number b described above
         */

        public static void get_max(int a, int b)
        {
            Console.WriteLine(a > b ? a : b);
        }

        static void Main(string[] args)
        {
            get_max(4, 2); // 4
            Console.ReadKey();
        }
    }
}
