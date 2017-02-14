using System;
using System.Diagnostics;

namespace Binary
{
    class Program
    {
        /*
            Given an integer number
            Your task is to
            write a function that prints to the standard output (stdout) its binary format
            Note that your function will receive the following argument:
            n
            which is the integer number described above
        */

        public static void convert_to_binary(int n)
        {
            //This can be done using Convert.ToString(n,2) which will be faster
            string binary = string.Empty;
            while (n != 0)
            {
                binary += (n % 2).ToString();
                n /= 2;
            }
            //Console.WriteLine(binary);
        }

        public static void convert_to_binary2(int n)
        {
            var x = Convert.ToString(n,2);
        }

        static void Main(string[] args)
        {
            int iterations = 1000000;
            Stopwatch x = new Stopwatch();
            x.Start();
            for (int i = 0; i < iterations; i++)
            {
                convert_to_binary(i); // 1111111
            }
            x.Stop();
            Console.WriteLine(x.ElapsedTicks);
            x.Reset();
            x.Start();
            for (int i = 0; i < iterations; i++)
            {
                convert_to_binary2(i); // 1111111
            }
            x.Stop();
            Console.WriteLine(x.ElapsedTicks);
            Console.ReadKey();
        }
    }
}
