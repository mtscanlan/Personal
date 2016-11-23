using System.Collections.Generic;

namespace ProductListing.Models
{
    public class Result
    {
        public string product_name { get; }
        public IEnumerable<Listing> listings { get; }

        public Result(string name, IEnumerable<Listing> list)
        {
            product_name = name;
            listings = list;
        }
    }
}
