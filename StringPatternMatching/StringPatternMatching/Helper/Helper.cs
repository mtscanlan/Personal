using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StringPatternMatching {
	public class Helper {
		private const string REGEX_PATTERN_REPLACE_SPACE = "(?<!\\d)\\.(?!\\d)|[,_]";
		private const string REGEX_PATTERN_REPLACE_EMPTY = "[-]";

		public static string[] GetKeyWords(params string[] stringsToJoin) {
			var modelKeyWords = string.Join(" ", stringsToJoin);
			modelKeyWords = RegexReplace(modelKeyWords, REGEX_PATTERN_REPLACE_SPACE, " ");
			modelKeyWords = RegexReplace(modelKeyWords, REGEX_PATTERN_REPLACE_EMPTY, "");
			return modelKeyWords.Split(' ');
		}

		private static async Task<IEnumerable<string>> ReadCharacters(string fn) {
			string line;
			var lines = new List<string>();
			using (var sr = new StreamReader(fn))
				while ((line = await sr.ReadLineAsync()) != null)
					lines.Add(line.Replace("announced-date", "announceddate"));
			return lines;
		}

		public static void ReadFileAndPopulateData(string path, Action<string> parallelAction) {
			var text = ReadCharacters(path);
			Parallel.ForEach(text.Result, parallelAction);
		}

		private static string RegexReplace(string words, string pattern, string toValue) {
			var regexReplace = new Regex(pattern);
			return regexReplace.Replace(words, toValue);

		}
	}
}
