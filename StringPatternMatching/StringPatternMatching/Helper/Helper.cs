using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Helper {

		public static void PrintJson(string path, object jsonObject) {
			using (TextWriter writer = new StreamWriter(path))
				writer.Write(JsonConvert.SerializeObject(jsonObject, Formatting.Indented));
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

		public static bool SlidingStringDistance(string shortWord, string longWord, double threshold) {
			bool foundCondition = false;
			for (int i = 0; i < longWord.Length - shortWord.Length; i++) {
				double score = UserDefinedFunctions.StringDistance(shortWord, longWord.Substring(i, shortWord.Length));
				foundCondition = score >= threshold;
				if (foundCondition) break;
			}
			return foundCondition;
		}

		public static string TrimCharacters(string words) {
			return Regex.Replace(words, "[,_\\+\\- ]", "");
		}
	}
}
