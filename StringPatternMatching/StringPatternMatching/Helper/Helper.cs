using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Helper {
        
        public static double ConvertForex(string p, double val) {
            switch (p) {
                case "CAD":
                    return val * 0.8;
                case "EUR":
                    return val * 1.11;
                case "GBP":
                    return val * 1.53;
                case "USD":
                default:
                    return val;
            }
        }

        public static double Median(IEnumerable<double> sequence, double percentile = 0.5) {
            SortedSet<double> sortedSequence = new SortedSet<double>(sequence);
            int N = sortedSequence.Count;
            return N == 0 ? 0 : sortedSequence.ElementAt(N / 2);
        }

		public static void PrintJson(string path, object jsonObject) {
			using (TextWriter writer = new StreamWriter(path))
				writer.Write(JsonConvert.SerializeObject(jsonObject, Formatting.Indented));
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

		public static int SlidingStringDistance(string wordOne, string wordTwo, double threshold)
        {
            if (wordOne == null || wordTwo == null) return -1;
            if (wordOne.Length == wordTwo.Length) {
                double stringDistance = UserDefinedFunctions.StringDistance(wordOne, wordTwo);
                return stringDistance >= threshold ? 0 : -1;
            }

            Func<string, string, int> findMatchIndex = (shortWord, longWord) =>
            {
                shortWord = shortWord.ToLower();
                longWord = longWord.ToLower();
                bool foundCondition = false;
                int index;
                for (index = 0; index < longWord.Length - shortWord.Length; index++)
                {
                    double score = UserDefinedFunctions.StringDistance(shortWord, longWord.Substring(index, shortWord.Length));
                    foundCondition = score >= threshold;
                    if (foundCondition) break;
                }
                if (!foundCondition) index = -1;
                return index;
            };

            return wordOne.Length > wordTwo.Length ? findMatchIndex(wordTwo, wordOne) : findMatchIndex(wordOne, wordTwo);
        }
    }
}
