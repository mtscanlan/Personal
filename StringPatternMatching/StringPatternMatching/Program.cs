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

			// Read from the files and populate the Products and Listings objects.
			long increment = 0;
			Helper.ReadFileAndPopulateData(@"products.txt", p => {
				var product = JsonConvert.DeserializeObject<Product>(p);
				Products.AddOrUpdate(Interlocked.Increment(ref increment), product, (k, v) => product);
			});

			increment = 0;
			Helper.ReadFileAndPopulateData(@"listings.txt", l => {
				var listing = JsonConvert.DeserializeObject<Listing>(l);
				listing.KeyWordsString = Helper.TrimCharacters(listing.title);
				Listings.AddOrUpdate(Interlocked.Increment(ref increment), listing, (k, v) => listing);
			});
			Console.WriteLine("Finished Reading Files : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Match listings to their respective products.
			ConcurrentDictionary<string, ConcurrentBag<string>> unmatchedresults = new ConcurrentDictionary<string, ConcurrentBag<string>>();
			Parallel.ForEach(Products, product => {
				product.Value.MatchedListings = new ConcurrentBag<long>();
				Parallel.ForEach(Listings, listing => {
					if (UserDefinedFunctions.StringDistance(listing.Value.manufacturer, product.Value.manufacturer) >= 0.85) {
						string trimmedProductModel = Helper.TrimCharacters(product.Value.model);
						if (Helper.SlidingStringDistance(trimmedProductModel, listing.Value.KeyWordsString, 0.92)) {
							// TODO: Need to filter based on price.
							product.Value.MatchedListings.Add(listing.Key);
						}
					}
				});
			});
			Console.WriteLine("Finished Matching Products : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Populate the Results object from our matched listings in the Product object. 
			Parallel.ForEach(Products, x => Results.Add(
				new Result(x.Value.product_name, Listings.Where(l => x.Value.MatchedListings.Contains(l.Key)).Select(l => l.Value))
			));
			Console.WriteLine("Populated Results : {0}s", sw.ElapsedMilliseconds / 1000f);


			// Print the results.
			Helper.PrintJson(@"results.txt", new List<Result>(Results));
			sw.Stop();
			Console.WriteLine("Ding! Results Complete : {0}s", sw.ElapsedMilliseconds / 1000f);
			Console.ReadKey();
		}
	}
}