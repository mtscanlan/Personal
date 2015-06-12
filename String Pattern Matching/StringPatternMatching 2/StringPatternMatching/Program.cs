using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace StringPatternMatching {
    public class Program {

        private const string REGEX_REPLACE_PATTERN = "[ *\\.,_/\\\\(\\)\\+\\-]";
        private const string REGEX_REPLACE_PATTERN_NOSPACE = "[*\\.,_/\\\\(\\)\\+\\-]";
        private static readonly ConcurrentBag<Result> Results = new ConcurrentBag<Result>();
        private static readonly ConcurrentBag<Product> Products = new ConcurrentBag<Product>();
        private static readonly ConcurrentDictionary<string, CustomListing> Listings = new ConcurrentDictionary<string, CustomListing>();

        private static void Main(string[] args) {
            var uri = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(uri, "my-application");
            var client = new ElasticClient(settings);
            var regexReplace = new Regex(REGEX_REPLACE_PATTERN);
            var regexReplaceNoSpace = new Regex(REGEX_REPLACE_PATTERN_NOSPACE);

            var sw = new Stopwatch();
            sw.Start();

            // Read from the files and populate the Products objects.
            long id = 0;
            Helper.ReadFileAndPopulateData(@"products.txt", p => {
                var product = JsonConvert.DeserializeObject<Product>(p);
                product.FormattedFamily = product.family == null ? "" : regexReplace.Replace(product.family, "").ToLower();
                product.FormattedManufacturer = product.manufacturer == null ? "" : regexReplace.Replace(product.manufacturer, "").ToLower();
                product.FormattedModel = product.model == null ? "" : regexReplace.Replace(product.model, "").ToLower();
                product.FormattedName = product.product_name == null ? "" : regexReplaceNoSpace.Replace(product.product_name, " ").ToLower();
                Products.Add(product);
            });

            // With the FormattedName "word cloud", read from the files and populate the Listings object.
            id = 0;
            JaroWinklerComparer jaroComparer = new JaroWinklerComparer(1.0);
            var wordCloudFormattedManufacturer = Products.Select(p => p.FormattedManufacturer).Distinct();
            Helper.ReadFileAndPopulateData(@"listings.txt", l => {
                CustomListing listing = JsonConvert.DeserializeObject<CustomListing>(l);
                if (!String.IsNullOrWhiteSpace(listing.title)) {
                    listing.FormattedTitle = regexReplaceNoSpace.Replace(listing.title, "").ToLower();
                    var keyWords = listing.FormattedTitle.Split(' ').Distinct();
                    var manufacturerKeyWords = regexReplace.Replace(listing.manufacturer.ToLower(), " ").Split(' ');
                    var formattedManufacturerKeyWords = manufacturerKeyWords.Intersect(wordCloudFormattedManufacturer, jaroComparer).ToArray();

                    if (formattedManufacturerKeyWords.Count() > 0 &&
                        wordCloudFormattedManufacturer.Contains(formattedManufacturerKeyWords.First()) &&
                        keyWords.Intersect(wordCloudFormattedManufacturer).Count() > 0)
                    {
                        listing.Id = Interlocked.Increment(ref id).ToString();
                        Listings.AddOrUpdate(listing.Id, listing, (k,v) => listing);
                        client.Index(listing);
                    }
                }
            });
            Console.WriteLine("Finished reading files and populating data : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Match listings to their respective products.
			Parallel.ForEach(Products, product => {
				product.MatchedListings = new ConcurrentBag<Listing>();
				ISearchResponse<CustomListing> matchedListings = client.Search<CustomListing>(s =>
					s.Take(Listings.Count).Query(q => {
						if (!String.IsNullOrEmpty(product.FormattedFamily)) {
							return q.Match(m => m.Query(product.FormattedManufacturer).OnField(f => f.FormattedTitle)) &&
                            q.Match(m => m.Query(product.FormattedFamily).OnField(f => f.FormattedTitle)) &&
							q.Match(m => m.Query(product.FormattedModel).OnField(f => f.FormattedTitle));
						} else {
							return q.Match(m => m.Query(product.FormattedManufacturer).OnField(f => f.FormattedTitle)) &&
							q.Match(m => m.Query(product.FormattedModel).OnField(f => f.FormattedTitle));
                        }
                    }));

                matchedListings.Documents.ForEach(doc => {
					Listings[doc.Id].Flag = "true";
					product.MatchedListings.Add(new Listing(doc));
                });
			});
			Console.WriteLine("Finished matching products : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Populate the Results object from our matched listings in the Product object. 
			Parallel.ForEach(Products, p => {
				Results.Add(new Result(p.product_name, p.MatchedListings.Distinct()));
			});
			Console.WriteLine("Populated results : {0}s", sw.ElapsedMilliseconds / 1000f);

			// Print the results, need to convert this to a List due to issues with serializing a ConcurrentBag.
			Helper.PrintJson(@"results.txt", new List<Result>(Results), Formatting.Indented);


			Helper.PrintJson(@"rejects.txt", new List<Listing>(Listings.Values.Where(l => l.Flag == "false")), Formatting.None);

			sw.Stop();
			Console.WriteLine("Ding! Results complete : {0}s", sw.ElapsedMilliseconds / 1000f);
			Console.ReadKey();
		}
	}
}