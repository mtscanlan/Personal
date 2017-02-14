using System;
using System.Collections.Generic;
using System.Linq;
using ProductListing.Models;

namespace ProductListing
{
    public static class ProductListingHelper
    {
        public static bool MatchProductToListing(this Product product, Listing listing, IDictionary<string, int> manufacturerFilter)
        {
            if (listing == null)
            {
                throw new ArgumentNullException(nameof(listing), $"{nameof(listing)} cannot be null.");
            }

            int productManufacturerKey = manufacturerFilter[product.Manufacturer];
            int listingManufacturerKey = manufacturerFilter[listing.Manufacturer];
            
            return listing.IsMatched = 
                productManufacturerKey == listingManufacturerKey && // match manufacturer
                listing.FormattedTitle.Contains(product.FormattedModel) && // match exact model in title
                !listing.FormattedTitleWords.Intersect(new string[] { "for", "pour", "für" }).Any() && // remove false positives
                listing.FormattedTitleWords.Intersect(product.FormattedFamilyWords).Any() && // match any words from family in title
                listing.FormattedTitleWords.Intersect(product.FormattedModelWords).Count() == product.FormattedModelWords.Count(); // match all words from model in title
        }
    }
}
