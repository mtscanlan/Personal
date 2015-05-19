using System;

namespace Find_Character {
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

		public static void find_chr(string c, string t) {
			int index = t.IndexOf(c);
			Console.WriteLine(index >= 0 ? index + 1 : index);
		}

		static void Main(string[] args) {
			find_chr("d", "abcdefabcdef");
			find_chr("z", "abcdefabcdef");
			Console.ReadKey();
		}
	}
}
