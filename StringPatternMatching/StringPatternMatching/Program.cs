using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

			long increment = 0;

			Helper.ReadFileAndPopulateData(@"products.txt", x => {
				var product = JsonConvert.DeserializeObject<Product>(x);
				var keyWords = Helper.GetKeyWords(product.family, product.manufacturer, product.model, product.product_name);
				product.Words = new HashSet<string>(keyWords);
				Products.AddOrUpdate(Interlocked.Increment(ref increment), product, (k, v) => product);
			});

			var productWords = new HashSet<string>(Products.SelectMany(x => x.Value.Words));

			Helper.ReadFileAndPopulateData(@"listings.txt", x => {
				var listing = JsonConvert.DeserializeObject<Listing>(x);
				var keyWords = Helper.GetKeyWords(listing.title, listing.manufacturer, listing.currency, listing.price);
				listing.Words = new HashSet<string>(keyWords.Intersect(productWords));
				Listings.AddOrUpdate(Interlocked.Increment(ref increment), listing, (k, v) => listing);
			});

			sw.Stop();
			Console.WriteLine("Finished Reading Files : {0}s", sw.ElapsedMilliseconds / 1000f);
			sw.Reset();

			// testing
			HashSet<string> manufacturers = new HashSet<string>(Listings.Select(x => x.Value.manufacturer));
			HashSet<string> manufacturers2 = new HashSet<string>(Products.Select(x => x.Value.manufacturer));

			sw.Start();

			MatchProductsToListings();

			sw.Stop();
			Console.WriteLine("Finished Matching Products : {0}s", sw.ElapsedMilliseconds / 1000f);
			sw.Reset();
			sw.Start();

			Parallel.ForEach(Products, x => Results.Add(
				new Result(x.Value.product_name,
					Listings.Where(l => x.Value.MatchedListings.Contains(l.Key)).Select(l => l.Value))
				));

			sw.Stop();
			Console.WriteLine("Populated Results : {0}s", sw.ElapsedMilliseconds / 1000f);
			sw.Reset();
			sw.Start();

			using (TextWriter writer = new StreamWriter(@"results.txt"))
				writer.Write(JsonConvert.SerializeObject(Results));

			sw.Stop();
			Console.WriteLine("Ding! Results Complete : {0}s", sw.ElapsedMilliseconds / 1000f);
			Console.ReadKey();
		}

		private static void MatchProductsToListings() {
			ConcurrentDictionary<string, ConcurrentBag<string>> unmatchedresults = new ConcurrentDictionary<string, ConcurrentBag<string>>();
			Parallel.ForEach(Products, p => {
				// testing
				unmatchedresults[p.Value.manufacturer] = new ConcurrentBag<string>();

				p.Value.MatchedListings = new HashSet<long>();
				Parallel.ForEach(Listings, l => {
					SqlDouble manufacturerScore = 0;
					if ((manufacturerScore = UserDefinedFunctions.StringDistance(l.Value.manufacturer, p.Value.manufacturer,
						() => l.Value.manufacturer.IndexOf(p.Value.manufacturer, StringComparison.OrdinalIgnoreCase) >= 0)) >= 0.85) {
						// Cool, we have it narrowed down to manufacturer, this still isn't 100% accurate.
						Console.WriteLine("{0}{1}{2}", manufacturerScore, l.Value.manufacturer, p.Value.manufacturer);
					} else {
						// testing
						unmatchedresults[p.Value.manufacturer].Add(l.Value.manufacturer);
					}
				});
			});

			// testing
			Dictionary<string, HashSet<string>> unmatchedoutput = new Dictionary<string, HashSet<string>>();
			foreach (var res in unmatchedresults) {
				unmatchedoutput[res.Key] = new HashSet<string>(res.Value);
			}
			using (TextWriter writer = new StreamWriter(@"unmatchedresults.txt"))
				writer.Write(JsonConvert.SerializeObject(unmatchedoutput, Formatting.Indented));

		}
	}
}