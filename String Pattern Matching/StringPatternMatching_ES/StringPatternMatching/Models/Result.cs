using System.Collections.Generic;

namespace StringPatternMatching {
	public class Result {

		public string product_name { get; set; }
		public List<IndexedListing> listings { get; set; }

		public Result(string name, IEnumerable<IndexedListing> list) {
			product_name = name;
			listings = list as List<IndexedListing>;
		}
	}
}
