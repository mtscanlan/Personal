using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProductListing;
using StringPatternMatching.Properties;
using ProductListing.Models;
using ProductListing.Utilities;

namespace StringPatternMatching
{
    public class Program
    {
        private static ConcurrentBag<Product> _products = new ConcurrentBag<Product>();
        private static ConcurrentBag<Listing> _listings = new ConcurrentBag<Listing>();
        private static ConcurrentBag<Result> _results = new ConcurrentBag<Result>();

        private static void Main(string[] args)
        {
            Console.WriteLine("Matching listings to products, this will take a few seconds...");
#if DEBUG
            Trace.Listeners.Add(new TextWriterTraceListener("trace.log"));
#endif
            // 1.) ProductManufacturerParser was used to create a list of known manufacturers 
            // and copied to this project and is loaded here. Contains a list of key value pairs 
            // of manufacturer names and aliases to an index. ie "sony", "sony dslr", and 
            // "sony electronics" all have an index of 0.
            Trace.WriteLine($"Loading filtered manufacturer list from \"{Settings.Default.ProductsFileName}\"");
            string manufacturerJson = System.IO.File.ReadAllText(Settings.Default.ManufacturersFileName);
            var tempManufacturerDictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(manufacturerJson);
            var manufacturerFilter = new Dictionary<string, int>(tempManufacturerDictionary, StringComparer.OrdinalIgnoreCase);
            
            // 2.) Load the two text files products.txt and listings.txt
            Task productTask = FileInput.ReadFileLineAction(Settings.Default.ProductsFileName, p => 
                _products.Add(JsonConvert.DeserializeObject<Product>(p)));
            Task listingTask = FileInput.ReadFileLineAction(Settings.Default.ListingsFileName, l =>
            {
                var listing = JsonConvert.DeserializeObject<Listing>(l);

                // Filter the listings we know we won't match on, we are looking for 
                // a 100% match on the manufacturer (or it's known aliases).
                if (manufacturerFilter.ContainsKey(listing.Manufacturer))
                {
                    _listings.Add(listing);
                }
            });
            Task.WaitAll(productTask, listingTask);
            Trace.WriteLine("Finished reading files and populating elastic search database.");

            // 3.) Match products to listings by iterating over each listing for each product and populate a collection of 
            // Listing's using the MatchProductToListing(this Product, Listing, IDictionary<string, int>) method.
            Trace.WriteLine("Matching listings to products.");
            Parallel.ForEach(_products, product => 
            {
                var matchedListings = new ConcurrentBag<Listing>();

                Parallel.ForEach(_listings, listing => {
                    if (product.MatchProductToListing(listing, manufacturerFilter))
                    {
                        matchedListings.Add(listing);
                    }
                });

                _results.Add(new Result(product.ProductName, matchedListings.Distinct()));
            });
            Trace.WriteLine($"Matched {_results.Sum(r => r.listings.Count())} "
                + $"listings to {_results.Where(r => r.listings.Count() > 0).Count()} products.");


            // ********************************************************** REMOVE
            _listings.Where(w => !w.IsMatched).OrderBy(ob => ob.Manufacturer).ToList().ForEach(fe => Debug.WriteLine(fe));



            // 4.) Print the results, need to convert this to a List due to issues with serializing a ConcurrentBag.
            FileOutput.PrintJson(Settings.Default.ResultsFileName, new List<Result>(_results));

            // 5.) Done
            Console.WriteLine("Complete, press any key to exit...");
            Console.ReadKey();
        }
    }
}