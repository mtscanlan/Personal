using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductListing.Utilities;

namespace ProductManufacturerParser
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> manufacturerWords = new List<string>();
            Dictionary<string, int> manufacturers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { { "hewlett packard", 0 }, { "fuji", 0 } };
            FileInput.ReadFileLineAction(@"..\StringPatternMatching\products.txt", p =>
            {
                dynamic product = JsonConvert.DeserializeObject<dynamic>(p);
                string manufacturer = product.manufacturer;
                manufacturerWords.AddRange(manufacturer.Split(' '));
                manufacturers[manufacturer.ToLower()] = manufacturers.Count;
            }).Wait();
            
            FileInput.ReadFileLineAction(@"..\StringPatternMatching\listings.txt", p =>
            {
                dynamic listing = JsonConvert.DeserializeObject<dynamic>(p);
                string manufacturer = ((string)listing.manufacturer).ToLower();
                if (manufacturerWords.Intersect(manufacturer.Split(' '), StringComparer.OrdinalIgnoreCase).Any())
                {
                    if (manufacturers.ContainsKey(manufacturer))
                    {
                        return;
                    }

                    manufacturers[manufacturer] = manufacturers.Count;
                }
            }).Wait();

            using (StreamWriter file = new StreamWriter("manufacturers.json"))
            {
                file.Write(JsonConvert.SerializeObject(manufacturers));
            }
        }
    }
}
