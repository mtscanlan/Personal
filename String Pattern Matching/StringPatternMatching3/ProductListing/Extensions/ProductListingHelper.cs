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

            if (listing.IsMatched)
            {
                //if (productManufacturerKey == listingManufacturerKey && // match manufacturer exactly (or alias)
                //    listing._title.Contains(product._model) && // match formatted model in title exactly
                //    listing._titleWords.Intersect(product._familyWords).Any() && // match all words from family in title
                //    listing._titleWords.Intersect(product._modelWords).Count() == product._modelWords.Count()) // match all words from model in title
                //{
                //    ; // für, 
                //}
                return false;
            }
            
            return listing.IsMatched = 
                productManufacturerKey == listingManufacturerKey && // match manufacturer
                listing._title.Contains(product._model) && // match exact model in title
                listing._titleWords.Intersect(product._familyWords).Any() && // match any words from family in title
                listing._titleWords.Intersect(product._modelWords).Count() == product._modelWords.Count(); // match all words from model in title
        }
    }
}
