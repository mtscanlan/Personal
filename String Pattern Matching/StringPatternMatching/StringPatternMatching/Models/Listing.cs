using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Listing {

		public string manufacturer { get; set; }
		public string title { get; set; }
		public string currency { get; set; }
		public string price { get; set; }

		public Listing() { }

		public Listing(Listing listing) {
			manufacturer = listing.manufacturer;
			title = listing.title;
			currency = listing.currency;
			price = listing.price;
		}

		public override string ToString() {
			return JsonConvert.SerializeObject(this);
		}

	}
}