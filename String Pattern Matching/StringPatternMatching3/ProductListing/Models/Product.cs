using System.Runtime.Serialization;
using Newtonsoft.Json;
using ProductListing.Utilities;

namespace ProductListing.Models
{
    public class Product
    {
        public string FormattedFamily { get; private set; }
        public string[] FormattedFamilyWords { get; private set; }
        public string FormattedModel { get; private set; }
        public string[] FormattedModelWords { get; private set; }

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
            FormattedFamily = Family == null ? string.Empty : RegexHelper.RegexReplace.Replace(Family, " ").ToLower();
            FormattedFamilyWords = FormattedFamily.Split(' ');

            FormattedModel = Model == null ? string.Empty : RegexHelper.RegexReplace.Replace(Model, " ").ToLower();
            FormattedModelWords = FormattedModel.Split(' ');
        }
    }
}
