using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Helper {
		
		public const string LISTINGS_PATH = @"listings.txt";
		public const string PRODUCTS_PATH = @"products.txt";
		public const string RESULTS_PATH = @"results.txt";

		public static readonly Regex RegexReplace = new Regex("[^a-zA-Z\\d:]");
		public static readonly Regex Trimmer = new Regex(@"\s\s+");

		public static void PrintJson(string path, IEnumerable<object> jsonObject, Formatting formatting) {
			using (TextWriter writer = new StreamWriter(path))
				jsonObject.ForEach(j => writer.WriteLine(JsonConvert.SerializeObject(j, formatting)));
		}

		private static async Task<IEnumerable<string>> ReadCharacters(string path) {
			string line;
			var lines = new List<string>();
			using (var sr = new StreamReader(path))
				while ((line = await sr.ReadLineAsync()) != null)
					lines.Add(line);
			return lines;
		}

		public static void ReadFileAndPopulateData(string path, Action<string> parallelAction) {
			var text = ReadCharacters(path);
			Parallel.ForEach(text.Result, parallelAction);
		}
	}
}
