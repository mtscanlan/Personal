using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringPatternMatching {
    public class CustomListing : Listing {

		public string FormattedTitle { get; set; }

		public CustomListing() { }

        public CustomListing(Listing listing) : base(listing) { }
    }
}
