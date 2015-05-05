using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Growth
{
    class Program
    {
        public static void check_growth(int d1, int d2)
        {
            Console.WriteLine(d2 >= d1 ? "Increase" : "Decrease");
        }

        static void Main(string[] args)
        {
            check_growth(400, 1000); // Increase
            check_growth(1000, 800); // Decrease
            Console.ReadKey();
        }
    }
}
