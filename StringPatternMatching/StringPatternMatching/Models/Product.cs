using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace StringPatternMatching {
	public class Product {

		public string product_name { get; set; }
		public string manufacturer { get; set; }
		public string model { get; set; }
		public string family { get; set; }

        public HashSet<string> ExceptWords { get; internal set; }
        public HashSet<string> IncludeWords { get; set; }
        public ConcurrentBag<long> MatchedListings { get; set; }
        public bool ProductModelIsInt { get; set; }
        public string TrimmedProductModel { get; set; }
    }
}
