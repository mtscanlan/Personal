using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    public class Product
    {
        public string product_name { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public string family { get; set; }
        public DateTime _announceddate { get; private set; }
        public string announceddate { get { return _announceddate.ToString(); } set { _announceddate = DateTime.Parse(value); } }
        public HashSet<string> Words { get; set; }
        public List<int> MatchedListings { get; set; }
    }

    public class Listing
    {
        public string title { get; set; }
        public string manufacturer { get; set; }
        public string currency { get; set; }
        public string price { get; set; }
        public HashSet<string> Words { get; set; }
    }

    public class Program
    {
        private const string REGEX_PATTERN_REPLACE_SPACE = "(?<!\\d)\\.(?!\\d)|[,_]";
        private const string REGEX_PATTERN_REPLACE_EMPTY = "[-]";

        private static ConcurrentBag<Product> products = new ConcurrentBag<Product>();
        private static ConcurrentBag<Listing> listings = new ConcurrentBag<Listing>();

        static void Main(string[] args)
        {
            ReadFileAndPopulateData(@"products.txt", (x) => {
                Product product = JsonConvert.DeserializeObject<Product>(x);
                string[] keyWords = GetKeyWords(product.family, product.manufacturer, product.model, product.product_name);
                product.Words = new HashSet<string>(keyWords);
                products.Add(product);
            });

            ReadFileAndPopulateData(@"listings.txt", (x) => {
                Listing listing = JsonConvert.DeserializeObject<Listing>(x);
                string[] keyWords = GetKeyWords(listing.title, listing.manufacturer, listing.currency, listing.price);
                listing.Words = new HashSet<string>(keyWords);
                listings.Add(listing);
            });

            Console.ReadKey();
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
