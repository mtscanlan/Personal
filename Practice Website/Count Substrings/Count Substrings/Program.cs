using System;

namespace Count_Substrings {
	class Program {

		/*
			String "aa" will have the following non-overlapping occurrences in string "aaabaaaaa"
			Given two strings s and t
			Your task is to
			write a function that prints to standard output (stdout) the number of non-overlapping occurrences of string s in string t
			Note that your function will receive the following arguments:
			s
			which is the string that you must search for
			t
			which is the string where you have to search
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
