using Contentful.Core.Models;
using Newtonsoft.Json;

using System.Collections.Generic;

namespace Products.Models
{
    public class Product
    {
        public SystemProperties Sys { get; set; }
     
        [JsonProperty("productName")]
        public string Name { get; set; }

        public string Slug { get; set; }

        public string Description { get; set; }

        public List<Asset> Image { get; set; }

        public List<string> Tags { get; set; }

        public IEnumerable<Category> Categories { get; set; }    

        public Brand Brand { get; set; }

        public string Sizetypecolor { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string SKU { get; set; }

        public string AvailableAt { get; set; }
    }
}