﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Listing {

		public string title { get; set; }
		public string manufacturer { get; set; }
		public string currency { get; set; }
		public string price { get; set; }

		[JsonIgnore]
		public HashSet<string> Words { get; set; }
	}
}
