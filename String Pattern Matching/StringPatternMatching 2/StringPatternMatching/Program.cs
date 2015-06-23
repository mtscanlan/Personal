using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Program {
		
		private static readonly ConcurrentBag<Product> Products = new ConcurrentBag<Product>();
		private static readonly ConcurrentBag<Result> Results = new ConcurrentBag<Result>();

		private static void Main(string[] args) {
			var settings = new ConnectionSettings(Helper.ELASTIC_SEARCH_URI, Helper.DEFAULT_INDEX);
			var client = new ElasticClient(settings);
			client.DeleteIndex(Helper.DEFAULT_INDEX);

			// Read from the products file and populate the Products container
			Helper.ReadFileAndPopulateData(Helper.PRODUCTS_PATH, p => {
				var product = JsonConvert.DeserializeObject<Product>(p);

				product.FormattedFamily = product.family == null ? "" : Helper.RegexReplace.Replace(product.family, " ").ToLower();
				product.FormattedModel = product.model == null ? "" : Helper.RegexReplace.Replace(product.model, " ").ToLower();
				product.FormattedManufacturer = product.manufacturer == null ? "" : Helper.RegexReplace.Replace(product.manufacturer, " ").ToLower();

				Products.Add(product);
			});

			// Read from the listings file and populate Elastic Search
			int index = 0;
			Helper.ReadFileAndPopulateData(Helper.LISTINGS_PATH, l => {
				var listing = JsonConvert.DeserializeObject<IndexedListing>(l);

				listing.FormattedTitle = listing.title == null ? "" : Helper.RegexReplace.Replace(listing.title, " ").ToLower();
				listing.FormattedTitle = Helper.Trimmer.Replace(listing.FormattedTitle, " ");
				listing.FormattedManufacturer = listing.manufacturer == null ? "" : Helper.RegexReplace.Replace(listing.manufacturer, " ").ToLower();

				listing.id = Interlocked.Increment(ref index);
				client.Index(listing);
			});
			Debug.WriteLine("Finished reading files and populating data.");

			// Match listings to their respective products.
			Parallel.ForEach(Products, product => {
				product.MatchedListings = new HashSet<IndexedListing>();

				ISearchResponse<IndexedListing> matchedListings = client.Search<IndexedListing>(s => s
					.Take(index)
					.Query(q =>
						// Search on products manufacturer to the listings manufacturer and the products family and model to the listings title.
						q.Match(m => m.Query(product.FormattedManufacturer).OnField(f => f.FormattedManufacturer)) &&
						q.Match(m => m.Query(product.FormattedFamily).OnField(f => f.FormattedTitle)) &&
						q.Match(m => m.Query(product.FormattedModel).OnField(f => f.FormattedTitle).Operator(Operator.And))));
				
				Results.Add(new Result(product.product_name, matchedListings.Documents.Select(l => new Listing(l))));
			});
			Debug.WriteLine(String.Format("Matched {0} listings to {1} products.", 
				Results.Sum(r => r.listings.Count()), 
				Results.Where(r => r.listings.Count() > 0).Count()));

			// Print the results, need to convert this to a List due to issues with serializing a ConcurrentBag.
			Helper.PrintJson(Helper.RESULTS_PATH, new List<Result>(Results), Formatting.None);
			Debug.WriteLine(String.Format("Populated and saved Results to {0}", 
				Helper.RESULTS_PATH));

			Console.ReadKey();
		}
	}
}