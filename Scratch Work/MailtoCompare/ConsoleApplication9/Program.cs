using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication9
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new Uri("MAILTO:igloo1@mailinator.com");
            var y = new Uri("MAILTO:igloofour@mailinator.com");
            Console.WriteLine(x == y);
            Console.ReadKey();
        }
    }
}
