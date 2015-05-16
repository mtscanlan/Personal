using System;

namespace Count_Tokens {
	class Program {

		/*
			An alphabet is a set of characters. A token is a sequence of consecutive alphabet characters delimited by one or more characters that are not part of the alphabet.
			For example if the alphabet is "abxy" then "aabby", "bxa" are considered tokens because all their characters belong to the alphabet. While "abd", "efg" are not.
			Given a string a that contains the letters of an alphabet and a string t
			Your task is to
			write a function that prints to standard output (stdout) the number of tokens found in t
			Note that your function will receive the following arguments:
			a
			which is is a string of characters that define the alphabet
			t
			which is the string where you must search for tokens
		*/

		public static void count_tokens(string a, string t) {
			bool runningCondition = false;
			int count = 0;
			for (int i = 0; i < t.Length; i++) {
				bool foundCondition = false;
				for (int j = 0; j < a.Length; j++) {
					if (t[i] == a[j]) {
						foundCondition = true;
						break;
					}
				}
				if (foundCondition && !runningCondition) {
					runningCondition = true;
					count++;
				} else runningCondition = foundCondition;
			}
			Console.WriteLine(count);
		}

		static void Main(string[] args) {
			count_tokens("anmo", "anatomy"); // 2
			count_tokens("elr", "hello there"); // 2
			Console.ReadKey();
		}
	}
}
