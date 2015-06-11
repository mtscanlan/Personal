using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace StringPatternMatching {
	public class Helper {

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

        public static int SlidingStringDistance(string wordOne, string wordTwo, double threshold)
        {
            if (wordOne == null || wordTwo == null) return -1;

            wordOne = wordOne.ToLower();
            wordTwo = wordTwo.ToLower();

            if (wordOne.Length == wordTwo.Length)
            {
                double stringDistance = UserDefinedFunctions.StringDistance(wordOne, wordTwo);
                return stringDistance >= threshold ? 0 : -1;
            }

            Func<string, string, int> findMatchIndex = (shortWord, longWord) =>
            {
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
