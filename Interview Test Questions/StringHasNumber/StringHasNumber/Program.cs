using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringHasNumber
{
    class Program
    {
        public static void has_number(string word)
        {
            bool containsNumber = false;
            foreach (char c in word.ToCharArray())
            {
                if (containsNumber = Char.IsDigit(c))
                {
                    break;
                }
            }
            Console.WriteLine(containsNumber ? "Yes" : "No");
        }

        static void Main(string[] args)
        {
            has_number(" hel1o world");
            Console.ReadKey();
        }
    }
}
