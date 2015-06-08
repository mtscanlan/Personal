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
                var keyWords = Regex.Replace(String.Join(" ", product.model, product.family, product.manufacturer, product.product_name), "[ ,_\\+\\-]", " ").Split(' ');
                product.Words = new HashSet<string>(keyWords);
                Products.AddOrUpdate(Interlocked.Increment(ref increment), product, (k, v) => product);
			});

            // Create a "word cloud" of product key words.
            var productWords = new HashSet<string>(Products.SelectMany(x => x.Value.Words));
            for (char letter = 'a'; letter <= 'z'; letter++)
                productWords.Add(letter.ToString());

            // With the productWords "word cloud", read from the files and populate the Listings object.
            increment = 0;
			Helper.ReadFileAndPopulateData(@"listings.txt", l => {
                // TODO: Split this up so it's more readable
				var listing = JsonConvert.DeserializeObject<Listing>(l);
                var keyWords = Regex.Replace(String.Join(" ", listing.title, listing.manufacturer, listing.currency, listing.price), "[,_/\\\\(\\)\\+\\-]", " ").Split(' ');
                listing.Words = keyWords.Intersect(productWords, StringComparer.OrdinalIgnoreCase);
                listing.KeyWordsString = String.Join("", listing.Words);
				Listings.AddOrUpdate(Interlocked.Increment(ref increment), listing, (k, v) => listing);
			});
            Console.WriteLine("Finished Reading Files : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Match listings to their respective products.
			ConcurrentDictionary<string, ConcurrentBag<string>> unmatchedresults = new ConcurrentDictionary<string, ConcurrentBag<string>>();
			Parallel.ForEach(Products, product => {
				product.Value.MatchedListings = new ConcurrentBag<long>();
				Parallel.ForEach(Listings, listing =>
                {
                    if (!listing.Value.HasParent && !String.IsNullOrWhiteSpace(listing.Value.KeyWordsString)) {
                        if (Helper.SlidingStringDistance(listing.Value.KeyWordsString, product.Value.manufacturer, 1.0) != -1) {
                            string trimmedProductModel = Regex.Replace(product.Value.model, "[ ,_\\+\\-]", "");
                            //int productModelInt = -1;
                            if (Helper.SlidingStringDistance(listing.Value.KeyWordsString, trimmedProductModel, 1.0) != -1) {
                                //if (!Int32.TryParse(product.Value.model, out productModelInt) ||
                                //    (listing.Value.Words.Contains(product.Value.model) &&
                                //    listing.Value.Words.Contains(product.Value.family))) {
                                //    listing.Value.HasParent = true;
                                    product.Value.MatchedListings.Add(listing.Key);
                                //}
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
			sw.Stop();
			Console.WriteLine("Ding! Results Complete : {0}s", sw.ElapsedMilliseconds / 1000f);
			Console.ReadKey();
		}
	}
}