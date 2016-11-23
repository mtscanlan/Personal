using System.Runtime.Serialization;
using Newtonsoft.Json;
using ProductListing.Utilities;

namespace ProductListing.Models
{
    public class Listing
    {
        [JsonIgnore]
        internal string _title;
        [JsonIgnore]
        internal string[] _titleWords;

        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("currency")]
        public string Currency { get; set; }
        [JsonProperty("price")]
        public string Price { get; set; }
        public bool IsMatched { get; set; } = false;

        public Listing() { }

        public Listing(
            string manufacturer,
            string title,
            string currency,
            string price)
        {
            Manufacturer = manufacturer;
            Title = title;
            Currency = currency;
            Price = price;
        }

        [OnDeserialized]
        private void OnDeserializingMethod(StreamingContext context)
        {
            _title = Title == null ? string.Empty : RegexHelper.RegexReplace.Replace(Title, " ").ToLower();
            _title = RegexHelper.RegexTrimmer.Replace(_title, " ");
            _titleWords = _title.Split(' ');
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}