using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Sum
{
    class Program
    {
        /*
            Given two integer numbers A and B
            Your task is to
            write a function that prints to the standard output (stdout) their sum
            Note that your function will receive the following arguments:
            a
            which is the integer number A described above
            b
            which is the integer number B described above
         */

        public static void get_sum(int a, int b)
        {
            Console.WriteLine(a + b);
        }

        static void Main(string[] args)
        {
            get_sum(5, 3); // 8
            Console.ReadKey();
        }
    }
}
