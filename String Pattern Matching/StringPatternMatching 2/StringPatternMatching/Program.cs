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
#if DEBUG
            Trace.Listeners.Add(new ConsoleTraceListener());
#endif
            Trace.WriteLine("Connecting to the elastic search database.");
            var settings = new ConnectionSettings(Helper.ELASTIC_SEARCH_URI, Helper.DEFAULT_INDEX);
            var client = new ElasticClient(settings);
            client.DeleteIndex(Helper.DEFAULT_INDEX);

            HashSet<string> productManufacturerCloud = new HashSet<string>();
            // Add a few more keywords for erroneous listings.
            productManufacturerCloud.Add("hewlett");
            productManufacturerCloud.Add("packard");
            productManufacturerCloud.Add("digital");
            productManufacturerCloud.Add("fuji");

            Trace.WriteLine(String.Format("Reading from \"{0}\"", Helper.PRODUCTS_PATH));
            Task productTask = Helper.ReadFileAndPopulateData(Helper.PRODUCTS_PATH, p =>
            {
                var product = JsonConvert.DeserializeObject<Product>(p);

                // Standardize the product family, model and manufacturer values.
                product.FormattedModel = product.model == null ? String.Empty : Helper.RegexReplace.Replace(product.model, " ").ToLower();
                product.FormattedModelNoSpace = product.FormattedModel.Replace(" ", "");
                int modelAsNumeric = 0;
                product.FormattedFamily = product.family == null || !Int32.TryParse(product.FormattedModel, out modelAsNumeric) ? 
                    String.Empty : Helper.RegexReplace.Replace(product.family, " ").ToLower();
                product.FormattedManufacturer = product.manufacturer == null ? String.Empty : Helper.RegexReplace.Replace(product.manufacturer, " ").ToLower();

                product.FormattedManufacturer.Split(' ').ForEach(o => productManufacturerCloud.Add(o));
                Products.Add(product);
            });
            
            HashSet<IndexedListing> testing = new HashSet<IndexedListing>();
            HashSet<IndexedListing> rejects = new HashSet<IndexedListing>();

            int index = 0;
            Trace.WriteLine(String.Format("Reading from \"{0}\" and populating the search database.", Helper.LISTINGS_PATH));
            Task listingTask = Helper.ReadFileAndPopulateData(Helper.LISTINGS_PATH, l =>
            {
                var listing = JsonConvert.DeserializeObject<IndexedListing>(l);

                listing.FormattedManufacturer = listing.manufacturer == null ? String.Empty : Helper.RegexReplace.Replace(listing.manufacturer, " ").ToLower();

                if (String.IsNullOrWhiteSpace(listing.FormattedManufacturer) || productManufacturerCloud.Intersect(listing.FormattedManufacturer.Split(' ')).Any())
                {
                    listing.FormattedTitle = listing.title == null ? String.Empty : Helper.RegexReplace.Replace(listing.title, " ").ToLower();
                    listing.FormattedTitle = Helper.RegexTrimmer.Replace(listing.FormattedTitle, " ");

                    // Set the id and add the listing to the elastic search database.
                    listing.id = Interlocked.Increment(ref index);
                    client.Index(listing);
                }
            });

            // Wait for ReadFileAndPopulateData tasks to complete.
            Task.WaitAll(productTask, listingTask);
            Trace.WriteLine("Finished reading files and populating data.");

            Trace.WriteLine("Matching listings to their respective products.");
            Parallel.ForEach(Products, product =>
            {
                ISearchResponse<IndexedListing> matchedListings = client.Search<IndexedListing>(s => s
                    .Take(index)
                    .Query(q =>
                        q.Match(m => m.Query(product.FormattedManufacturer).OnField(f => f.FormattedTitle).Operator(Operator.And)) &&
                        q.Match(m => m.Query(product.FormattedFamily).OnField(f => f.FormattedTitle)) &&
                        (q.Match(m => m.Query(product.FormattedModel).OnField(f => f.FormattedTitle).Operator(Operator.And)) || 
                        q.Match(m => m.Query(product.FormattedModelNoSpace).OnField(f => f.FormattedTitle)))));

                Results.Add(new Result(product.product_name, matchedListings.Documents.Select(l => new Listing(l))));
            });
            Trace.WriteLine(String.Format("Matched [{0}] listings to [{1}] products.",
                Results.Sum(r => r.listings.Count()),
                Results.Where(r => r.listings.Count() > 0).Count()));

            // Print the results, need to convert this to a List due to issues with serializing a ConcurrentBag.
            Helper.PrintJson(Helper.RESULTS_PATH, new List<Result>(Results), Formatting.Indented);
            Trace.WriteLine(String.Format("Saved results to \"{0}\"", Helper.RESULTS_PATH));

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
		}
	}
}