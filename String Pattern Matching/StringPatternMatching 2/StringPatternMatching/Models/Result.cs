using System.Collections.Generic;

namespace StringPatternMatching {
	public class Result {

		public string product_name;
		public IEnumerable<Listing> listings;

		public Result(string name, IEnumerable<Listing> list) {
			product_name = name;
			listings = list;
		}
	}
}
