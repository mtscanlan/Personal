using System.Runtime.Serialization;
using Newtonsoft.Json;
using ProductListing.Utilities;

namespace ProductListing.Models
{
    public class Product
    {
        internal string _family;
        internal string[] _familyWords;
        internal string _model;
        internal string[] _modelWords;

        [JsonProperty("family")]
        public string Family { get; set; } = string.Empty;
        [JsonProperty("manufacturer")]
        public string Manufacturer { get; set; } = string.Empty;
        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;
        [JsonProperty("product_name")]
        public string ProductName { get; set; } = string.Empty;
        
        public Product() { }

        public Product(
            string family,
            string manufacturer,
            string model,
            string productName)
        {
            Family = family;
            Manufacturer = manufacturer;
            Model = model;
            ProductName = productName;
        }

        [OnDeserialized]
        private void OnDeserializingMethod(StreamingContext context)
        {
            _family = Family == null ? string.Empty : RegexHelper.RegexReplace.Replace(Family, " ").ToLower();
            _familyWords = _family.Split(' ');
            _model = Model == null ? string.Empty : RegexHelper.RegexReplace.Replace(Model, " ").ToLower();
            _modelWords = _model.Split(' ');
        }
    }
}
