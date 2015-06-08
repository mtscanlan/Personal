using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace StringPatternMatching {
	public class Listing {

		public string title { get; set; }
		public string manufacturer { get; set; }
		public string currency { get; set; }
		public string price { get; set; }

		[JsonIgnore]
		public string KeyWordsString { get; set; }
        [JsonIgnore]
        public IEnumerable<string> Words { get; internal set; }
        [JsonIgnore]
        public bool HasParent { get; set; }

        public override string ToString() {
			return JsonConvert.SerializeObject(this);
		}
	}
}
