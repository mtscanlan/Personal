namespace StringPatternMatching {
	public class IndexedListing : Listing {

		public long id { get; set; }
		public string[] FormattedManufacturer { get; set; }
		public string[] FormattedTitle { get; set; }

		public IndexedListing() { }

        public IndexedListing(Listing listing) : base(listing) { }
    }
}
