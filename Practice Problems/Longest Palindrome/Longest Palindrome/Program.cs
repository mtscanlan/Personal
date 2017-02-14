using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Longest_Palindrome
{
    class Program
    {
        /*
            Given a string S, find the longest substring in S that is the same in reverse and print it to the standard output. If there are multiple substrings that have the same longest length, print the first one that appears in S from left to right.
            Expected complexity: O(N2)
            Example input:
            S: "abcdxyzyxabcdaaa"
            Example output:
            xyzyx
        */

        public static void longest_palind(string s)
        {
            string leader = string.Empty;
            int leadcount = 0;
            char[] s2 = s.ToArray();
            int index = 0;
            foreach (char c in s)
            {
                for (int i = 1; i <= s.Length; i++)
                {
                    if (index - i < 0 || index + i >= s.Length)
                    {
                        index++;
                        break;
                    }
                    if (s2[index + i] == s2[index - i])
                    {
                        if (i <= leadcount) continue;
                        leadcount = i;
                        leader = s.Substring(index - i, (2 * i) + 1);
                    }
                    else
                    {
                        index++;
                        break;
                    }
                }
            }
            Console.WriteLine(leader);
        }

        static void Main(string[] args)
        {
            longest_palind("abcdxyzyxabcdaaa"); // xyzyx
            Console.ReadKey();
        }
    }
}
