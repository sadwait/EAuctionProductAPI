using Newtonsoft.Json;
using SellerAPI.Models;
using System;

namespace SellerAPI.Models
{
    public class Product
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "productName")]
        public string ProductName { get; set; }

        [JsonProperty(PropertyName = "shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty(PropertyName = "detailedDescription")]
        public string DetailedDescription { get; set; }

        [JsonProperty(PropertyName = "category")]
        public string Category { get; set; }

        [JsonProperty(PropertyName = "startingPrice")]
        public string StartingPrice { get; set; }

        [JsonProperty(PropertyName = "bidEndDate")]
        public DateTime BidEndDate { get; set; }

        [JsonProperty(PropertyName = "sellerInfo")]
        public Seller SellerInfo { get; set; }
    }
}
