using Contentful.Core.Configuration;
using Contentful.Core.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Products.Models
{
    public class Brand
    {
        public string CompanyName { get; set; }
        public Asset Logo { get; set; }
        [JsonProperty("companyDescription")]
        public string Description { get; set; }
        public string Website { get; set; }
        public string Twitter { get; set; }
        public string Email { get; set; }
        public List<string> Phone { get; set; }
    }
}