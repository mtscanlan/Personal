using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vowel_Count
{
    class Program
    {
        /*
            Given a string
            Your task is to
            write a function that prints to the standard output (stdout) the number of vowels it contains
            Note that your function will receive the following argument:
            s
            which is the string described above
         */

        public static void count_vowels(string s)
        {
            Regex regex = new Regex("[aeiouAEIOU]");
            Console.WriteLine(regex.Matches(s).Count);
        }

        static void Main(string[] args)
        {
            count_vowels("Get some"); // 3
            Console.ReadKey();
        }
    }
}
