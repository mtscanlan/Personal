using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Result
    {
        string product_name;
        IEnumerable<Listing> listings;

        public Result(string name, IEnumerable<Listing> list)
        {
            product_name = name;
            listings = list;
        }
    }

    public class Product
    {
        public string product_name { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string family { get; set; }
        public DateTime _announceddate { get; private set; }
        public string announceddate { get { return _announceddate.ToString(); } set { _announceddate = DateTime.Parse(value); } }
        public HashSet<string> Words { get; set; }
        public HashSet<long> MatchedListings { get; set; }
    }

    public class Listing
    {
        public string title { get; set; }
        public string manufacturer { get; set; }
        public string currency { get; set; }
        public string price { get; set; }
        [JsonIgnore]
        public HashSet<string> Words { get; set; }
    }

    public partial class UserDefinedFunctions
    {
        /// <summary>
        /// This region contains code related to Jaro Winkler string distance algorithm. 
        /// </summary>
        #region Jaro Distance
        private const double defaultMismatchScore = 0.0;
        private const double defaultMatchScore = 1.0;

        /// <summary>
        /// gets the similarity of the two strings using Jaro distance.
        /// </summary>
        /// <param name="firstWord"></param>
        /// <param name="secondWord"></param>
        /// <returns>a value between 0-1 of the similarity</returns>
        public static SqlDouble StringDistance(string firstWord, string secondWord)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                if (firstWord == secondWord)
                {
                    return (SqlDouble)defaultMatchScore;
                }
                else
                {
                    //get half the length of the string rounded up - (this is the distance used for acceptable transpositions)
                    int halflen = Math.Min(firstWord.Length, secondWord.Length) / 2 + 1;
                    //get common characters
                    StringBuilder common1 = GetCommonCharacters(firstWord, secondWord, halflen);
                    int commonMatches = common1.Length;
                    //check for zero in common
                    if (commonMatches == 0)
                    {
                        return (SqlDouble)defaultMismatchScore;
                    }
                    StringBuilder common2 = GetCommonCharacters(secondWord, firstWord, halflen);
                    //check for same length common strings returning 0.0f is not the same
                    if (commonMatches != common2.Length)
                    {
                        return (SqlDouble)defaultMismatchScore;
                    }
                    //get the number of transpositions
                    int transpositions = 0;
                    for (int i = 0; i < commonMatches; i++)
                    {
                        if (common1[i] != common2[i])
                        {
                            transpositions++;
                        }
                    }
                    int j = 0;
                    j += 1;
                    //calculate jaro metric
                    transpositions /= 2;
                    double tmp1;
                    tmp1 = commonMatches / (3.0 * firstWord.Length) + commonMatches / (3.0 * secondWord.Length) +
                    (commonMatches - transpositions) / (3.0 * commonMatches);
                    return (SqlDouble)tmp1;
                }
            }
            return (SqlDouble)defaultMismatchScore;
        }
        /// <summary>
        /// returns a string buffer of characters from string1 within string2 if they are of a given
        /// distance seperation from the position in string1.
        /// </summary>
        /// <param name="firstWord">string one</param>
        /// <param name="secondWord">string two</param>
        /// <param name="distanceSep">separation distance</param>
        /// <returns>a string buffer of characters from string1 within string2 if they are of a given
        /// distance seperation from the position in string1</returns>
        private static StringBuilder GetCommonCharacters(string firstWord, string secondWord, int distanceSep)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                StringBuilder returnCommons = new StringBuilder(20);
                StringBuilder copy = new StringBuilder(secondWord);
                int firstLen = firstWord.Length;
                int secondLen = secondWord.Length;
                for (int i = 0; i < firstLen; i++)
                {
                    char ch = firstWord[i];
                    bool foundIt = false;
                    for (int j = Math.Max(0, i - distanceSep);
                    !foundIt && j < Math.Min(i + distanceSep, secondLen);
                    j++)
                    {
                        if (copy[j] == ch)
                        {
                            foundIt = true;
                            returnCommons.Append(ch);
                            copy[j] = '#';
                        }
                    }
                }
                return returnCommons;
            }
            return null;
        }
        #endregion
    };

    public class Program
    {
        private const string REGEX_PATTERN_REPLACE_SPACE = "(?<!\\d)\\.(?!\\d)|[,_]";
        private const string REGEX_PATTERN_REPLACE_EMPTY = "[-]";
        
        private static readonly ConcurrentBag<Result> Results = new ConcurrentBag<Result>();
        private static readonly ConcurrentDictionary<long, Product> Products = new ConcurrentDictionary<long, Product>();
        private static readonly ConcurrentDictionary<long, Listing> Listings = new ConcurrentDictionary<long, Listing>();

        static void Main(string[] args)
        {
            long increment = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            ReadFileAndPopulateData(@"products.txt", (x) => {
                var product = JsonConvert.DeserializeObject<Product>(x);
                string[] keyWords = GetKeyWords(product.family, product.manufacturer, product.model, product.product_name);
                product.Words = new HashSet<string>(keyWords);
                Products.AddOrUpdate(Interlocked.Increment(ref increment), product, (k, v) => product);
            });
            
            var productWords = new HashSet<string>(Products.SelectMany(x => x.Value.Words));

            ReadFileAndPopulateData(@"listings.txt", (x) => {
                var listing = JsonConvert.DeserializeObject<Listing>(x);
                string[] keyWords = GetKeyWords(listing.title, listing.manufacturer, listing.currency, listing.price);
                listing.Words = new HashSet<string>(keyWords.Intersect(productWords));
                Listings.AddOrUpdate(Interlocked.Increment(ref increment), listing, (k, v) => listing);
            });

            sw.Stop();
            Console.WriteLine("Finished Reading Files : {0}s", sw.ElapsedMilliseconds / 1000f);
            sw.Reset();
            sw.Start();
            MatchProductsToListings();
            sw.Stop();
            Console.WriteLine("Finished Matching Products : {0}s", sw.ElapsedMilliseconds / 1000f);
            sw.Reset();
            sw.Start();
            PopulateResults();
            sw.Stop();
            Console.WriteLine("Populated Results : {0}s", sw.ElapsedMilliseconds / 1000f);
            sw.Reset();
            sw.Start();
            OutputResultsAsJson();
            sw.Stop();
            Console.WriteLine("Ding! Results Complete : {0}s", sw.ElapsedMilliseconds / 1000f);
            Console.ReadKey();
        }

        private static void OutputResultsAsJson()
        {
            //write string to file
            using (TextWriter writer = new StreamWriter(@"results.txt"))
                writer.Write(JsonConvert.SerializeObject(Results));
        }

        private static void PopulateResults()
        {
            Parallel.ForEach(Products, x => Results.Add(
                new Result(x.Value.product_name, Listings.Where(l => x.Value.MatchedListings.Contains(l.Key)).Select(l => l.Value))
            ));
        }

        private static void MatchProductsToListings()
        {
            Parallel.ForEach(Products, (p) =>
            {
                p.Value.MatchedListings = new HashSet<long>();
                Parallel.ForEach(Listings, (l) =>
                {
 
                    foreach (var word in l.Value.Words)
                    {
                        var modelScore = UserDefinedFunctions.StringDistance(word, p.Value.model);
                        var nameScore = UserDefinedFunctions.StringDistance(word, p.Value.product_name);

                        if (nameScore > 0.9 || modelScore > 0.9)
                        {
                            p.Value.MatchedListings.Add(l.Key);
                        }
                    }
                });
            });
        }

        private static void ReadFileAndPopulateData(string path, Action<string> parallelAction)
        {
            Task<IEnumerable<string>> text = ReadCharacters(path);
            Parallel.ForEach(text.Result, parallelAction);
        }

        private static string[] GetKeyWords(params string[] stringsToJoin)
        {
            var modelKeyWords = String.Join(" ", stringsToJoin);
            var pattern = new Regex(REGEX_PATTERN_REPLACE_SPACE);
            var pattern2 = new Regex(REGEX_PATTERN_REPLACE_EMPTY);
            modelKeyWords = pattern.Replace(modelKeyWords, " ");
            modelKeyWords = pattern2.Replace(modelKeyWords, "");
            return modelKeyWords.Split(' ');
        }

        private static async Task<IEnumerable<string>> ReadCharacters(String fn)
        {
            string line;
            var lines = new List<string>();
            using (var sr = new StreamReader(fn))
                while ((line = await sr.ReadLineAsync()) != null)
                    lines.Add(line.Replace("announced-date", "announceddate"));
            return lines;
        }
    }
}
