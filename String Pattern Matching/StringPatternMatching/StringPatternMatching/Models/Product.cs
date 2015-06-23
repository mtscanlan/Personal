using System.Collections.Concurrent;
using System.Collections.Generic;

namespace StringPatternMatching {
	public class Product {

		public string family { get; set; }
		public string manufacturer { get; set; }
		public string model { get; set; }
		public string product_name { get; set; }

		public string[] FormattedManufacturer { get; internal set; }
		public string[] FormattedName { get; internal set; }
		public ConcurrentBag<IndexedListing> MatchedListings { get; set; }

		public Product() { }
	}
}
