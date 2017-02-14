using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balanced_Brackets
{
    class Program
    {
        /*
           Given a string of open and closed brackets output "Balanced" if the brackets are balanced or "Unbalanced" otherwise.
            A string is balanced if it consists entirely of pairs of opening/closing brackets (in that order), none of which mis-nest.
            Expected complexity: O(N)
            Example input:
            (())())
            Example output:
            Unbalanced
            Example input:
            (()())
            Example output:
            Balanced
        */

        public static void balanced_brackets(string s)
        {
            int count = 0;
            bool fail = false;
            foreach (char c in s)
            {
                if (c == '(') count++;
                else if (c == ')') count--;
                if (count < 0) fail = true;
            }
            Console.WriteLine("{0}", fail || count != 0 ? "Unbalanced" : "Balanced");
        }

        static void Main(string[] args)
        {
            balanced_brackets("(())())"); // Unbalanced
            balanced_brackets("(()())"); // Balanced
            Console.ReadKey();
        }
    }
}
