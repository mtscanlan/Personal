using System.Collections.Generic;
using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Listing {

		public string manufacturer { get; set; }
		public string title { get; set; }
		public string currency { get; set; }
		public string price { get; set; }

		[JsonIgnore]
		public string FormattedTitle { get; internal set; }
		[JsonIgnore]
		public string[] FormattedManufacturerKeyWords { get; internal set; }
		[JsonIgnore]
		public HashSet<string> KeyWords { get; internal set; }

		// testing
		[JsonIgnore]
		public bool Flag { get; internal set; }

		public override string ToString() {
			return JsonConvert.SerializeObject(this);
		}

	}
}
