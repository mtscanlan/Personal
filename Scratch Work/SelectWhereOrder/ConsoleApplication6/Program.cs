using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication6
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> x = new List<int>() { 1, 2, 3, 4, 5 };
            long iterations = 10000000;

            while (true)
            {
                System.Diagnostics.Stopwatch y = new System.Diagnostics.Stopwatch();
                y.Start();
                for (var i = 0; i < iterations; i++)
                {
                    var z = x.Select(s => s).Where(w => w != 5);
                }
                y.Stop();
                Console.WriteLine("Select first " + y.ElapsedTicks);

                y.Reset();
                y.Start();
                for (var i = 0; i < iterations; i++)
                {
                    var z = x.Where(w => w != 5).Select(s => s);
                }
                y.Stop();
                Console.WriteLine("Where first " + y.ElapsedTicks);
            }

            Console.ReadKey();
        }
    }
}
