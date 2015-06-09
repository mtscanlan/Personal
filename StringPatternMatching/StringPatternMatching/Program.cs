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

		private const string REGEX_REPLACE_PATTERN = "[*\\.,_/\\\\(\\)\\+\\-]";
		private const string REGEX_REPLACE_PATTERN_SPACE = "[ *\\.,_/\\\\(\\)\\+\\-]";
		private static readonly ConcurrentBag<Result> Results = new ConcurrentBag<Result>();
		private static readonly ConcurrentDictionary<long, Product> Products = new ConcurrentDictionary<long, Product>();
		private static readonly ConcurrentDictionary<long, Listing> Listings = new ConcurrentDictionary<long, Listing>();

		private static void Main(string[] args) {
			var sw = new Stopwatch();
			sw.Start();

			// Read from the files and populate the Products objects.
			long increment = 0;
			Helper.ReadFileAndPopulateData(@"products.txt", p => {
				var product = JsonConvert.DeserializeObject<Product>(p);
				Regex regexReplace = new Regex(REGEX_REPLACE_PATTERN);
				Regex regexReplaceWithSpace = new Regex(REGEX_REPLACE_PATTERN_SPACE);

				product.FormattedFamily = product.family == null ? "" : regexReplace.Replace(product.family, "").ToLower();
				product.FormattedManufacturer = product.manufacturer == null ? "" : regexReplace.Replace(product.manufacturer, "").ToLower();
				// with space
				product.FormattedModel = product.model == null ? "" : regexReplaceWithSpace.Replace(product.model, "").ToLower();
				// to space
				product.FormattedName = product.product_name == null ? "" : regexReplace.Replace(product.product_name, " ").ToLower();

				//This will be used later for our word cloud.
				string productWords = String.Join(" ", product.FormattedFamily, product.FormattedManufacturer, product.FormattedModel, product.FormattedName);
				product.IncludeWords = new HashSet<string>(productWords.ToLower().Split(' '));

				Products.AddOrUpdate(Interlocked.Increment(ref increment), product, (k, v) => product);
			});

			// Create a "word cloud" of product key words.
			var allIncludeWords = new HashSet<string>(Products.SelectMany(x => x.Value.IncludeWords));
			for (char letter = 'a'; letter <= 'z'; letter++)
				allIncludeWords.Add(letter.ToString());
			allIncludeWords.Remove("");
			allIncludeWords.Remove("wide");
			allIncludeWords.Remove("zoom");
			allIncludeWords.Remove("digital");

			//Exclude single numbers
			var numberRange = new List<string>() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

			// With the productWords "word cloud", read from the files and populate the Listings object.
			increment = 0;
			Helper.ReadFileAndPopulateData(@"listings.txt", l => {
				var listing = JsonConvert.DeserializeObject<Listing>(l);

				IEnumerable<string> keyWords = Regex.Replace(listing.title, REGEX_REPLACE_PATTERN, " ").ToLower().Split(' ');
				if (String.IsNullOrEmpty(listing.manufacturer)) {
					listing.manufacturer = keyWords.First();
				}

				//Use the "word clouds" to exclude and filter unused words
				keyWords = keyWords.Except(numberRange);
				listing.Words = keyWords.Intersect(allIncludeWords, StringComparer.OrdinalIgnoreCase).ToArray();

				Listings.AddOrUpdate(Interlocked.Increment(ref increment), listing, (k, v) => listing);
			});
			Console.WriteLine("Finished Reading Files : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Match listings to their respective products.
			Parallel.ForEach(Products, product => {
				product.Value.MatchedListings = new ConcurrentBag<long>();

				Parallel.ForEach(Listings, listing => {
					var manufacturerKeywords = listing.Value.manufacturer.ToLower().Split(' ');

					if (!listing.Value.HasParent && manufacturerKeywords.Intersect(listing.Value.Words).Count() > 0) {
						string listingString = String.Join("", listing.Value.Words);
						bool isPossibleSubstring = listingString.Length >= product.Value.FormattedManufacturer.Length;

						if (isPossibleSubstring && Helper.SlidingStringDistance(listingString, product.Value.FormattedManufacturer, 1.0) != -1) {
							listingString = listingString.Replace(product.Value.FormattedManufacturer, "");
							isPossibleSubstring = listingString.Length >= product.Value.FormattedFamily.Length;

							if (isPossibleSubstring && !String.IsNullOrEmpty(product.Value.FormattedFamily)) {
								if (Helper.SlidingStringDistance(listingString, product.Value.FormattedFamily, 1.0) != -1) {
									listingString = listingString.Replace(product.Value.FormattedFamily, "");
									isPossibleSubstring = listingString.Length >= product.Value.FormattedModel.Length;

									if (isPossibleSubstring && Helper.SlidingStringDistance(listingString, product.Value.FormattedModel, 1.0) != -1) {
										listing.Value.HasParent = true;
										product.Value.MatchedListings.Add(listing.Key);
									}
								}
							} else {
								isPossibleSubstring = listingString.Length > product.Value.FormattedModel.Length;

								if (isPossibleSubstring && Helper.SlidingStringDistance(listingString, product.Value.FormattedModel, 1.0) != -1) {
									product.Value.MatchedListings.Add(listing.Key);
								}
							}
						}
					}
				});
			});
			Console.WriteLine("Finished Matching Products : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Populate the Results object from our matched listings in the Product object. 
			Parallel.ForEach(Products, p => {
				// Get the collection of all listings matched to this product.
				var distinctListingKeys = p.Value.MatchedListings.Distinct();
				var listings = Listings.Where(l => distinctListingKeys.Contains(l.Key)).Select(l => l.Value);

				// Filter based on price. +/- 50% of the median price.
				double medianPrice = Helper.Median(listings.Select(l => Helper.ConvertForex(l.currency, Convert.ToDouble(l.price))));
				IEnumerable<int> medianRange = Enumerable.Range((int)(medianPrice * 4) / 3, (int)medianPrice / 2);

				// Add the result to the Results collection.
				Results.Add(new Result(p.Value.product_name, listings.Where(l =>
					medianRange.Contains((int)Helper.ConvertForex(l.currency, Convert.ToDouble(l.price))))));
			});
			Console.WriteLine("Populated Results : {0}s", sw.ElapsedMilliseconds / 1000f);
			
			// Print the results, need to convert this to a List due to issues with serializing a ConcurrentBag.
			Helper.PrintJson(@"results.txt", new List<Result>(Results));

			// testing
			Helper.PrintJson(@"rejects.txt", new HashSet<Listing>(Listings.Where(l => !l.Value.HasParent).Select(l => l.Value)));

			sw.Stop();
			Console.WriteLine("Ding! Results Complete : {0}s", sw.ElapsedMilliseconds / 1000f);
			Console.ReadKey();
		}
	}
}