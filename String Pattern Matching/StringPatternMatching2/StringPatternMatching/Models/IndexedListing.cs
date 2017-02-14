namespace StringPatternMatching {
	public class IndexedListing : Listing {

		public long id { get; set; }
		public string FormattedManufacturer { get; set; }
		public string FormattedTitle { get; set; }
        public Product Parent { get; set; }
        public double Score { get; set; }

		public IndexedListing() { }

        public IndexedListing(Listing listing) : base(listing) { }
    }
}
