using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using ProductListing.Models;

namespace ProductListing.Utilities
{
    public class FileOutput
    {
        public static void PrintJson(string path, Dictionary<string, IEnumerable<Listing>> productListings)
        {
            Trace.WriteLine($"Writing to file \"{path}\"");
            using (TextWriter writer = new StreamWriter(path))
            {
                foreach (var obj in productListings)
                {
                    writer.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented));
                }
            }
        }
    }
}
