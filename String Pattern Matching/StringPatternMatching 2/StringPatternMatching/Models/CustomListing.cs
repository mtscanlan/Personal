using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringPatternMatching {
    class CustomListing : Listing {

        public string Id { get; set; }
        public string FormattedTitle { get; internal set; }

        public CustomListing() { }
        public CustomListing(Listing listing) : base(listing) { }
    }
}
