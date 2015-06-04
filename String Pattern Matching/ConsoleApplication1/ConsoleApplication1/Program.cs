using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ConsoleApplication1
{
    public class Result
    {
        private IEnumerable<Listing> listings;
        private string product_name;

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

        public string announceddate
        {
            get { return _announceddate.ToString(); }
            set { _announceddate = DateTime.Parse(value); }
        }

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

    public class UserDefinedFunctions
    {
        /// <summary>
        ///     This region contains code related to Jaro Winkler string distance algorithm.
        /// </summary>
        private const double defaultMismatchScore = 0.0;

        private const double defaultMatchScore = 1.0;

        /// <summary>
        ///     gets the similarity of the two strings using Jaro distance.
        /// </summary>
        /// <param name="firstWord"></param>
        /// <param name="secondWord"></param>
        /// <returns>a value between 0-1 of the similarity</returns>
        public static SqlDouble StringDistance(string firstWord, string secondWord, Func<bool> additionalCheck)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                if (firstWord == secondWord || additionalCheck())
                {
                    return defaultMatchScore;
                }
                //get half the length of the string rounded up - (this is the distance used for acceptable transpositions)
                var halflen = Math.Min(firstWord.Length, secondWord.Length)/2 + 1;
                //get common characters
                var common1 = GetCommonCharacters(firstWord, secondWord, halflen);
                var commonMatches = common1.Length;
                //check for zero in common
                if (commonMatches == 0)
                {
                    return defaultMismatchScore;
                }
                var common2 = GetCommonCharacters(secondWord, firstWord, halflen);
                //check for same length common strings returning 0.0f is not the same
                if (commonMatches != common2.Length)
                {
                    return defaultMismatchScore;
                }
                //get the number of transpositions
                var transpositions = 0;
                for (var i = 0; i < commonMatches; i++)
                {
                    if (common1[i] != common2[i])
                    {
                        transpositions++;
                    }
                }
                var j = 0;
                j += 1;
                //calculate jaro metric
                transpositions /= 2;
                double tmp1;
                tmp1 = commonMatches/(3.0*firstWord.Length) + commonMatches/(3.0*secondWord.Length) +
                       (commonMatches - transpositions)/(3.0*commonMatches);
                return tmp1;
            }
            return defaultMismatchScore;
        }

        /// <summary>
        ///     returns a string buffer of characters from string1 within string2 if they are of a given
        ///     distance seperation from the position in string1.
        /// </summary>
        /// <param name="firstWord">string one</param>
        /// <param name="secondWord">string two</param>
        /// <param name="distanceSep">separation distance</param>
        /// <returns>
        ///     a string buffer of characters from string1 within string2 if they are of a given
        ///     distance seperation from the position in string1
        /// </returns>
        private static StringBuilder GetCommonCharacters(string firstWord, string secondWord, int distanceSep)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                var returnCommons = new StringBuilder(20);
                var copy = new StringBuilder(secondWord);
                var firstLen = firstWord.Length;
                var secondLen = secondWord.Length;
                for (var i = 0; i < firstLen; i++)
                {
                    var ch = firstWord[i];
                    var foundIt = false;
                    for (var j = Math.Max(0, i - distanceSep);
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
    }

    public class Program
    {
        private const string REGEX_PATTERN_REPLACE_SPACE = "(?<!\\d)\\.(?!\\d)|[,_]";
        private const string REGEX_PATTERN_REPLACE_EMPTY = "[-]";
        private static readonly ConcurrentBag<Result> Results = new ConcurrentBag<Result>();
        private static readonly ConcurrentDictionary<long, Product> Products = new ConcurrentDictionary<long, Product>();
        private static readonly ConcurrentDictionary<long, Listing> Listings = new ConcurrentDictionary<long, Listing>();

        private static void Main(string[] args)
        {
            long increment = 0;
            var sw = new Stopwatch();
            sw.Start();
            ReadFileAndPopulateData(@"products.txt", x =>
            {
                var product = JsonConvert.DeserializeObject<Product>(x);
                var keyWords = GetKeyWords(product.family, product.manufacturer, product.model, product.product_name);
                product.Words = new HashSet<string>(keyWords);
                Products.AddOrUpdate(Interlocked.Increment(ref increment), product, (k, v) => product);
            });

            var productWords = new HashSet<string>(Products.SelectMany(x => x.Value.Words));

            ReadFileAndPopulateData(@"listings.txt", x =>
            {
                var listing = JsonConvert.DeserializeObject<Listing>(x);
                var keyWords = GetKeyWords(listing.title, listing.manufacturer, listing.currency, listing.price);
                listing.Words = new HashSet<string>(keyWords.Intersect(productWords));
                Listings.AddOrUpdate(Interlocked.Increment(ref increment), listing, (k, v) => listing);
            });

            sw.Stop();
            Console.WriteLine("Finished Reading Files : {0}s", sw.ElapsedMilliseconds/1000f);
            sw.Reset();

            HashSet<string> manufacturers = new HashSet<string>(Listings.Select(x => x.Value.manufacturer));
            HashSet<string> manufacturers2 = new HashSet<string>(Products.Select(x => x.Value.manufacturer));
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
                new Result(x.Value.product_name,
                    Listings.Where(l => x.Value.MatchedListings.Contains(l.Key)).Select(l => l.Value))
                ));
        }

        private static void MatchProductsToListings()
        {
            ConcurrentDictionary<string, ConcurrentBag<string>> unmatchedresults = new ConcurrentDictionary<string, ConcurrentBag<string>>();
            Parallel.ForEach(Products, p =>
            {
                unmatchedresults[p.Value.manufacturer] = new ConcurrentBag<string>();
                p.Value.MatchedListings = new HashSet<long>();
                Parallel.ForEach(Listings, l =>
                {
                    try
                    {
                        SqlDouble manufacturerScore = 0;
                        if (
                            (manufacturerScore =
                                UserDefinedFunctions.StringDistance(l.Value.manufacturer, p.Value.manufacturer,
                                    () =>
                                        l.Value.manufacturer.IndexOf(p.Value.manufacturer,
                                            StringComparison.OrdinalIgnoreCase) >= 0)) >= 0.85)
                        {
                            Console.WriteLine("{0}{1}{2}", manufacturerScore, l.Value.manufacturer, p.Value.manufacturer);
                            // Cool, we have it narrowed down to manufacturer, this still isn't 100% accurate.
                        }
                        else
                        {
                            unmatchedresults[p.Value.manufacturer].Add(l.Value.manufacturer);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("{0}{1}{2}", ex.ToString(), l.Value.manufacturer, p.Value.manufacturer);
                    }
                });
            });

            Dictionary<string, HashSet<string>> unmatchedoutput = new Dictionary<string, HashSet<string>>();
            foreach (var res in unmatchedresults)
            {
                unmatchedoutput[res.Key] = new HashSet<string>(res.Value);
            }
            using (TextWriter writer = new StreamWriter(@"unmatchedresults.txt"))
                writer.Write(JsonConvert.SerializeObject(unmatchedoutput, Formatting.Indented));

        }

        private static void ReadFileAndPopulateData(string path, Action<string> parallelAction)
        {
            var text = ReadCharacters(path);
            Parallel.ForEach(text.Result, parallelAction);
        }

        private static string[] GetKeyWords(params string[] stringsToJoin)
        {
            var modelKeyWords = string.Join(" ", stringsToJoin);
            var pattern = new Regex(REGEX_PATTERN_REPLACE_SPACE);
            var pattern2 = new Regex(REGEX_PATTERN_REPLACE_EMPTY);
            modelKeyWords = pattern.Replace(modelKeyWords, " ");
            modelKeyWords = pattern2.Replace(modelKeyWords, "");
            return modelKeyWords.Split(' ');
        }

        private static async Task<IEnumerable<string>> ReadCharacters(string fn)
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