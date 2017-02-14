using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caesar_Shift
{
    class Program
    {
        /*
           Create a function that takes an input string and encrypts it using a caesar shift of +1.
            Please print the shifted string to the standard output (stdout)
            Example input:
            CAT
            Example output:
            DBU
        */

        public static void caesar_shift(string s)
        {
            foreach (char c in s)
            {
                if (c == 'Z')
                {
                    Console.Write((char)(c - 25));
                }
                else if (c != ' ')
                {
                    Console.Write((char)(c + 1));
                }
                else
                {
                    Console.Write(c);
                }
            }
        }

        static void Main(string[] args)
        {
            caesar_shift("CAT"); // DBU
            Console.ReadKey();
        }
    }
}
