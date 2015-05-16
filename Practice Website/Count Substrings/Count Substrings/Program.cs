using System;

namespace Count_Substrings {
	class Program {

		/*
			Given a character c and and a string t
			Your task is to
			write a function that prints to standard output (stdout)
			the position of the first occurrence of character c in string t
			-1 if the character is not found
			Note that your function will receive the following arguments:
			c
			which is a string of length 1 that contains the character you must search for
			t
			which is the string where you must search
		*/

		public static void count_substr(string s, string t) {
			int count = 0, index = -1;
			while ((index = t.IndexOf(s)) != -1) {
				count++;
				t = t.Substring(index + s.Length);
			}
			Console.WriteLine(count);
		}

		static void Main(string[] args) {
			count_substr("aa", "aaabaaaaa");
			count_substr("abc", "ababcdabc");
			Console.ReadKey();
		}
	}
}
