using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Program {

		private static readonly ConcurrentBag<Result> Results = new ConcurrentBag<Result>();
		private static readonly ConcurrentDictionary<long, Product> Products = new ConcurrentDictionary<long, Product>();
		private static readonly ConcurrentDictionary<long, Listing> Listings = new ConcurrentDictionary<long, Listing>();

		private static void Main(string[] args) {
			var sw = new Stopwatch();
			sw.Start();

			// Read from the files and populate the Products objects.
			long increment = 0;
			Helper.ReadFileAndPopulateData(@"products.txt", p => {
                // TODO: Split this up so it's more readable
                var product = JsonConvert.DeserializeObject<Product>(p);
                Regex regexReplace = new Regex("[ ,_\\+\\-]");

                var keyWords = Regex.Replace(String.Join(" ", product.family, product.manufacturer), "[ ,_\\+\\-]", " ").Split(' '); 
                var productFamily = product.family == null ? "" : regexReplace.Replace(product.family, "");
                var productMfg = product.manufacturer == null ? "" : regexReplace.Replace(product.manufacturer, "");
                var productName = product.product_name == null ? "" : regexReplace.Replace(product.product_name, "");
                product.TrimmedProductModel = product.model == null ? "" : regexReplace.Replace(product.model, "");

                product.IncludeWords = new HashSet<string>() { productFamily, productMfg, product.TrimmedProductModel, productName };
                keyWords.ForEach(k => product.IncludeWords.Add(k));
                product.ExceptWords = new HashSet<string>(keyWords);

                int productModelAsInt = -1;
                product.ProductModelIsInt = Int32.TryParse(product.TrimmedProductModel, out productModelAsInt);

                Products.AddOrUpdate(Interlocked.Increment(ref increment), product, (k, v) => product);
            });

            // Create a "word cloud" of product key words.
            var allExcludeWords = new HashSet<string>(Products.SelectMany(x => x.Value.ExceptWords));
            var allIncludeWords = new HashSet<string>(Products.SelectMany(x => x.Value.IncludeWords));
            for (char letter = 'a'; letter <= 'z'; letter++)
                allIncludeWords.Add(letter.ToString());
            allIncludeWords.Remove("");
            allIncludeWords.Remove("Zoom");


            // With the productWords "word cloud", read from the files and populate the Listings object.
            increment = 0;
			Helper.ReadFileAndPopulateData(@"listings.txt", l => {
				var listing = JsonConvert.DeserializeObject<Listing>(l);

                IEnumerable<string> keyWords = Regex.Replace(listing.title, "[*.,_/\\\\(\\)\\+\\-]", " ").Split(' ');
                if (String.IsNullOrEmpty(listing.manufacturer))
                    listing.manufacturer = keyWords.First();
                keyWords = keyWords.Intersect(allIncludeWords, StringComparer.OrdinalIgnoreCase);
                listing.KeyWordsString = listing.manufacturer + String.Join("", keyWords);
                listing.Words = keyWords;
				Listings.AddOrUpdate(Interlocked.Increment(ref increment), listing, (k, v) => listing);
			});
            Console.WriteLine("Finished Reading Files : {0}s", sw.ElapsedMilliseconds / 1000f);
            
			// Match listings to their respective products.
			ConcurrentDictionary<string, ConcurrentBag<string>> unmatchedresults = new ConcurrentDictionary<string, ConcurrentBag<string>>();
			Parallel.ForEach(Products, product => {
				product.Value.MatchedListings = new ConcurrentBag<long>();
                IEnumerable<KeyValuePair<long, Listing>> listingsWithKeyWords = Listings.Where(l => !String.IsNullOrWhiteSpace(l.Value.KeyWordsString));
                Parallel.ForEach(listingsWithKeyWords, listing => {
                    if (Helper.SlidingStringDistance(listing.Value.manufacturer, product.Value.manufacturer, 0.85) != -1) {
                        var keyWords = String.Join("", listing.Value.Words.Except(allExcludeWords, StringComparer.OrdinalIgnoreCase));
                        bool possibleSubstring = product.Value.TrimmedProductModel.Length <= keyWords.Length;
                        if (possibleSubstring && Helper.SlidingStringDistance(keyWords, product.Value.TrimmedProductModel, 1.0) != -1) {
                            if (!product.Value.ProductModelIsInt || listing.Value.Words.Contains(product.Value.model)) {
                                listing.Value.HasParent = true;
                                product.Value.MatchedListings.Add(listing.Key);
                            }
                        }
                    }
				});
			});
			Console.WriteLine("Finished Matching Products : {0}s", sw.ElapsedMilliseconds / 1000f);

            // Populate the Results object from our matched listings in the Product object. 
            Parallel.ForEach(Products, p => {
                var distinctListings = p.Value.MatchedListings.Distinct();
                var listings = Listings.Where(l => distinctListings.Contains(l.Key)).Select(l => l.Value);
                // Filter based on price. +/- 50% of the median price.
                double medianPrice = Helper.Median(listings.Select(l => Helper.ConvertForex(l.currency, Convert.ToDouble(l.price))));
                IEnumerable<int> medianRange = Enumerable.Range((int)medianPrice / 2, (int)medianPrice);
                Results.Add(new Result(p.Value.product_name, listings.Where(l =>
                    medianRange.Contains((int)Helper.ConvertForex(l.currency, Convert.ToDouble(l.price))))));
            });
			Console.WriteLine("Populated Results : {0}s", sw.ElapsedMilliseconds / 1000f);


			// Print the results.
			Helper.PrintJson(@"results.txt", new List<Result>(Results));
            Helper.PrintJson(@"rejects.txt", new HashSet<Listing>(Listings.Where(l => !l.Value.HasParent).Select(l => l.Value)));
			sw.Stop();
			Console.WriteLine("Ding! Results Complete : {0}s", sw.ElapsedMilliseconds / 1000f);
			Console.ReadKey();
		}
	}
}