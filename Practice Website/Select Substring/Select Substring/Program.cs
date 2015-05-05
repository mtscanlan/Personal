using System;

namespace Select_Substring
{
    class Program
    {
        /*
           A common operation with text editors is selecting a block of text for further manipulation. Let’s see how an editor extracts the characters between two given positions in a string.
            Given a string S and two integer numbers p1, p2
            Your task is to
            write a function that extracts all the characters between positions p1 and p2 in the given string and prints these characters to standard output (stdout)
            Note that your function will receive the following arguments:
            s
            which is the string from which you must extract the specified characters
            p1
            which is the position of the left-most character that you must extract
            p2
            which is the position of the right-most character that you must extract
            Data constraints
            the string will contain at most 1000 characters
        */

        public static void select_substring(string s, int p1, int p2)
        {
            Console.WriteLine(s.Substring(p1 - 1, p2 - p1 + 1));
        }

        static void Main(string[] args)
        {
            select_substring("abcdefghi", 2, 4); // bcd
            Console.ReadKey();
        }
    }
}
