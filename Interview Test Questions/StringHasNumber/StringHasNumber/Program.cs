using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringHasNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            string s = " hel1o world";
            char[] ch = s.ToCharArray();
            bool containsNumber = false;
            foreach (char c in ch)
            {
                if (containsNumber = Char.IsDigit(c))
                {
                    break;
                }
            }
            Console.WriteLine(containsNumber ? "Yes" : "No");
            Console.ReadKey();
        }
    }
}
