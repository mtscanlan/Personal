using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
				Regex regexReplaceWithSpace = new Regex(REGEX_REPLACE_PATTERN_SPACE);

				product.FormattedFamily = product.family == null ? "" : regexReplaceWithSpace.Replace(product.family, "").ToLower();
				product.FormattedManufacturer = product.manufacturer == null ? "" : regexReplaceWithSpace.Replace(product.manufacturer, "").ToLower();
				product.FormattedModel = product.model == null ? "" : regexReplaceWithSpace.Replace(product.model, "").ToLower();

				Products.AddOrUpdate(Interlocked.Increment(ref increment), product, (k, v) => product);
			});

			// Create a "word cloud" of manufacturers, this will be used to filter some listings.
			var wordCloudFormattedManufacturer = new HashSet<string>(Products.Values.Select(p => p.FormattedManufacturer));

			// With the FormattedName "word cloud", read from the files and populate the Listings object.
			increment = 0;
			JaroWinklerComparer jaroComparer = new JaroWinklerComparer(1.0);
			Helper.ReadFileAndPopulateData(@"listings.txt", l => {
				var listing = JsonConvert.DeserializeObject<Listing>(l);
				var title = listing.title.ToLower();
				Regex regexReplace = new Regex(REGEX_REPLACE_PATTERN);
				Regex regexReplaceWithSpace = new Regex(REGEX_REPLACE_PATTERN_SPACE);

				listing.FormattedTitle = regexReplaceWithSpace.Replace(title, "");
				listing.KeyWords = new HashSet<string>(regexReplace.Replace(title, " ").Split(' '));
				var manufacturerKeyWords = regexReplace.Replace(listing.manufacturer.ToLower(), " ").Split(' ');
                listing.FormattedManufacturerKeyWords =
					manufacturerKeyWords.Intersect(wordCloudFormattedManufacturer, jaroComparer).ToArray();

				if (listing.FormattedManufacturerKeyWords.Count() > 0 &&
					wordCloudFormattedManufacturer.Contains(listing.FormattedManufacturerKeyWords.First()) &&
					listing.KeyWords.Intersect(wordCloudFormattedManufacturer).Count() > 0) {
					Listings.AddOrUpdate(Interlocked.Increment(ref increment), listing, (k, v) => listing);
				} 
			});
			Console.WriteLine("Finished reading files and populating data : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Match listings to their respective products.
			var maxFormattedNameLength = Products.Max(p => p.Value.product_name.Length);
			Parallel.ForEach(Products.Values, product => {
				product.MatchedListings = new ConcurrentBag<long>();
				Parallel.ForEach(Listings, listing => {
					string matchingString = Helper.ExtractMatchingString(
						product.FormattedManufacturer,
						listing.Value,
						maxFormattedNameLength);

					if (!String.IsNullOrEmpty(matchingString)) {
						if (Helper.SlidingStringDistance(product.FormattedModel, matchingString, 1.0) != -1) {
							listing.Value.Flag = true;
							product.MatchedListings.Add(listing.Key);
						}
					}
				});
			});
			Console.WriteLine("Finished matching products : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Populate the Results object from our matched listings in the Product object. 
			Parallel.ForEach(Products, p => {
				// Get the collection of all listings matched to this product.
				var distinctListingKeys = p.Value.MatchedListings.Distinct();
				var listings = Listings.Where(l => distinctListingKeys.Contains(l.Key)).Select(l => l.Value);
				// Add the result to the Results collection.
				Results.Add(new Result(p.Value.product_name, listings));
			});
			Console.WriteLine("Populated results : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Print the results, need to convert this to a List due to issues with serializing a ConcurrentBag.
			Helper.PrintJson(@"results.txt", new List<Result>(Results), Formatting.Indented);

			// testing
			Helper.PrintJson(@"rejects.txt", new List<Listing>(Listings.Where(l => !l.Value.Flag).Select(l => l.Value)), Formatting.None);

			sw.Stop();
			Console.WriteLine("Ding! Results complete : {0}s", sw.ElapsedMilliseconds / 1000f);
			Console.ReadKey();
		}
	}
}

/*  TODO :   
"product_name": "Nikon_Coolpix_600",
  "listings": [
    {
      "manufacturer": "Nikon",
      "title": "Nikon Coolpix 7600 Digitalkamera (7 Megapixel) in schwarz",
      "currency": "EUR",
      "price": "100.95"
    },
*/