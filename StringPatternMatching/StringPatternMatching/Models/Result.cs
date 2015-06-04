using System.Collections.Generic;

namespace StringPatternMatching {
	public class Result {
		private IEnumerable<Listing> listings;
		private string product_name;

		public Result(string name, IEnumerable<Listing> list) {
			product_name = name;
			listings = list;
		}
	}
}
