using Contentful.Core.Models;

namespace Products.Models
{
    public class Category
    {
        public string Title { get; set; }
        public Asset Icon { get; set; }
        public string Description { get; set; }
    }
}