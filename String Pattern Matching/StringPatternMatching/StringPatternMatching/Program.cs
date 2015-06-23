using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Program {
		
		private static readonly ConcurrentBag<Result> Results = new ConcurrentBag<Result>();
		private static readonly ConcurrentBag<Product> Products = new ConcurrentBag<Product>();
		private static readonly ConcurrentBag<IndexedListing> Listings = new ConcurrentBag<IndexedListing>();

		private static void Main(string[] args) {

			// Read from the files and populate the Products objects.
			Helper.ReadFileAndPopulateData(@"products.txt", p => {
				var product = JsonConvert.DeserializeObject<Product>(p);

				product.FormattedManufacturer = product.manufacturer == null ? new string[0] :
					Helper.RegexReplace.Replace(product.manufacturer, " ").ToLower().Split(' ');
				product.FormattedName = product.product_name == null ? new string[0] :
					Helper.RegexReplace.Replace(product.product_name, " ").ToLower().Split(' ');

				Products.Add(product);
			});
						
			Helper.ReadFileAndPopulateData(@"listings.txt", l => {
				var listing = JsonConvert.DeserializeObject<IndexedListing>(l);

				var formattedTitle = listing.title == null ? "" : 
					Helper.RegexReplace.Replace(listing.title, " ").ToLower();
				formattedTitle = Helper.Trimmer.Replace(formattedTitle, " ");
				listing.FormattedTitle = formattedTitle.Split(' ');
				listing.FormattedManufacturer = listing.manufacturer == null ? new string[0] :
					Helper.RegexReplace.Replace(listing.manufacturer, " ").ToLower().Split(' ');
				
				Listings.Add(listing);
			});
			Debug.WriteLine("Finished reading files and populating data.");

			// Match listings to their respective products.
			Parallel.ForEach(Products, product => {
				product.MatchedListings = new ConcurrentBag<IndexedListing>();

				Parallel.ForEach(Listings, listing => {
					if (listing.FormattedManufacturer.Intersect(product.FormattedManufacturer).Count() == product.FormattedManufacturer.Count() &&
						listing.FormattedTitle.Intersect(product.FormattedName).Count() == product.FormattedName.Count()) {
						product.MatchedListings.Add(listing);
					}
				});
				
                Results.Add(new Result(product.product_name, product.MatchedListings.Distinct().Select(l => new Listing(l))));
			});
			Debug.WriteLine(String.Format("Matched {0} listings to {1} products.",
				Results.Sum(r => r.listings.Count()), Results.Where(r => r.listings.Count() > 0).Count()));

			// Print the results, need to convert this to a List due to issues with serializing a ConcurrentBag.
			Helper.PrintJson(Helper.RESULTS_PATH, new List<Result>(Results), Formatting.None);
			Debug.WriteLine(String.Format("Populated and saved results to {0}", Helper.RESULTS_PATH));

			Console.ReadKey();
		}
	}
}