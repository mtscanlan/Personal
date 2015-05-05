using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Sum
{
    class Program
    {
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
