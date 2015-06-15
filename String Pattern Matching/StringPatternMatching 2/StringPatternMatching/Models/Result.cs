using System.Collections.Generic;

namespace StringPatternMatching {
	public class Result {

		public string product_name { get; set; }
		public IEnumerable<Listing> listings { get; set; }

		public Result(string name, IEnumerable<Listing> list) {
			product_name = name;
			listings = list;
		}
	}
}
