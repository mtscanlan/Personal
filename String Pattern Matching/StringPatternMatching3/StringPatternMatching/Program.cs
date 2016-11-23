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
        private static Dictionary<string, ConcurrentBag<Listing>> _results = new Dictionary<string, ConcurrentBag<Listing>>();

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
            _results = _products.ToDictionary(k => k.ProductName, v => new ConcurrentBag<Listing>());
            foreach(var listing in _listings)
            {
                var matchedProducts = new ConcurrentBag<Product>();

                Parallel.ForEach(_products, product => 
                {
                    if (product.MatchProductToListing(listing, manufacturerFilter))
                    {
                        matchedProducts.Add(product);
                    }
                });

                if (matchedProducts.Count == 0) { /*Do Nothing*/ }
                else if (matchedProducts.Count == 1)
                {
                    Product tempProduct = matchedProducts.First();
                    _results[tempProduct.ProductName].Add(listing);
                }
                else
                {
                    if (!listing.FormattedTitleWords.Intersect(new string[] { "for", "pour", "für" }).Any())
                    {
                        var x = matchedProducts.OrderByDescending(ob => ob.FormattedModelWords.Count()).First();
                        ;
                    }
                }
            }
            Trace.WriteLine($"Matched {_results.Sum(r => r.Value.Count())} "
                + $"listings to {_results.Where(r => r.Value.Count() > 0).Count()} products.");


            // ********************************************************** REMOVE
            _listings.Where(w => !w.IsMatched).OrderBy(ob => ob.Manufacturer).ToList().ForEach(fe => Debug.WriteLine(fe));



            // 4.) Print the results, need to convert this to a List due to issues with serializing a ConcurrentBag.
            FileOutput.PrintJson(Settings.Default.ResultsFileName, _results.ToDictionary(kv => kv.Key, kv => kv.Value as IEnumerable<Listing>));

            // 5.) Done
            Console.WriteLine("Complete, press any key to exit...");
            Console.ReadKey();
        }
    }
}