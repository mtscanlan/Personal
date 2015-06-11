using System.Collections.Concurrent;

namespace StringPatternMatching {
	public class Product {

		public string family { get; set; }
		public string manufacturer { get; set; }
		public string model { get; set; }
		public string product_name { get; set; }

        public string Id { get; set; }
		public string FormattedFamily { get; set; }
        public string FormattedManufacturer { get; set; }
        public string FormattedModel { get; set; }
        public string FormattedName { get; set; }
		public ConcurrentBag<Listing> MatchedListings { get; set; }
	}
}
