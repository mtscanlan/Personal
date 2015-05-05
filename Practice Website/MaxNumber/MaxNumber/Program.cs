using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaxNumber
{
    class Program
    {
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
