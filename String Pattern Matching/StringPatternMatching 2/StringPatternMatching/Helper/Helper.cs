using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Helper {

		public static readonly Uri ELASTIC_SEARCH_URI = new Uri("http://localhost:9200");
		public const string DEFAULT_INDEX = "camera_listings";
		public const string LISTINGS_PATH = @"listings.txt";
		public const string PRODUCTS_PATH = @"products.txt";
		public const string REGEX_REPLACE_PATTERN = "[^a-zA-Z\\d:]";
		public const string RESULTS_PATH = @"results.txt";

		public static readonly Regex RegexReplace = new Regex(REGEX_REPLACE_PATTERN);
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
			text.Result.ForEach(parallelAction);
		}
	}
}
