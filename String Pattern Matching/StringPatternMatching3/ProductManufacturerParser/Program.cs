using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Utilities;

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
                manufacturers[manufacturer.ToLower()] = 0;
            }).Wait();

            HashSet<string> garbageEntries = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
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

                    manufacturers[manufacturer] = 1;
                }
                else
                {
                    garbageEntries.Add(manufacturer);
                }
            }).Wait();

            using (StreamWriter file = new StreamWriter("manufacturers.json"))
            {
                file.Write(JsonConvert.SerializeObject(manufacturers));
            }

            using (StreamWriter file = new StreamWriter("garbage.json"))
            {
                file.Write(JsonConvert.SerializeObject(garbageEntries));
            }
        }
    }
}
