using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StringPatternMatching
{
    public class Helper
    {
        public const string REGEX_REPLACE_PATTERN = "[^a-zA-Z\\d:]";
        public const string REGEX_TRIMMER_PATTERN = "\\s\\s+";

        public static readonly Regex RegexReplace = new Regex(REGEX_REPLACE_PATTERN);
        public static readonly Regex RegexTrimmer = new Regex(REGEX_TRIMMER_PATTERN);

        public static void PrintJson(string path, IEnumerable<object> jsonObject, Formatting formatting)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                foreach (var obj in jsonObject)
                {
                    writer.WriteLine(JsonConvert.SerializeObject(j, formatting));
                }
            }
        }

        private static IEnumerable<string> ReadCharacters(string path)
        {
            string line;
            var lines = new List<string>();
            using (var sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        public static async Task ReadFileAndPopulateData(string path, Action<string> parallelAction)
        {
            await Task.Run(() =>
            {
                foreach (var line in ReadCharacters(path))
                {
                    parallelAction(line);
                }
            });
        }
    }
}
