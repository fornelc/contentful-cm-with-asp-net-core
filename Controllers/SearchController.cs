using System.Collections.Generic;
using System.Threading.Tasks;
using Contentful.Core;
using Contentful.Core.Models;
using Contentful.Core.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nest;
using Products.Models;
using Products.Models.DTO;

namespace Products.Controllers
{
    public class SearchController : Controller
    {
        private readonly IElasticClient _client;

        private readonly IContentfulClient _contentfulClient;

        private readonly ILogger<SearchController> _logger;

        public SearchController(IElasticClient client, IContentfulClient contentfulClient,
                                ILogger<SearchController> logger)
        {
            _client = client;
            _contentfulClient = contentfulClient;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string query)
        {
            _logger.LogInformation("SearchController - Index - Searching in elasticsearch for documents using the query term '{Id}'", query);
            var productsResultDTO = new ProductsResultDTO();
            var productsResult = new List<Product>();
            if (!string.IsNullOrWhiteSpace(query))
            {
                var search = new SearchDescriptor<dynamic>()
                    .Query(qu => qu
                        .QueryString(queryString => queryString
                            .Query(query)));

                var result = _client.Search<dynamic>(search);

                await BuildProductsResult(productsResult, result);
            }

            productsResultDTO.Results = productsResult;
            return View(productsResultDTO);
        }

        private async Task BuildProductsResult(List<Product> productsResult, ISearchResponse<dynamic> result)
        {
            if (result != null && result.Hits.Count > 0)
            {
                foreach (var hit in result.Hits)
                {
                    var product = new Product
                    {
                        Sys = RetrieveSystemProperty(hit)
                    };

                    var fields = hit.Source["fields"];
                    product.Name = RetrieveStringField((Dictionary<string, object>)fields["productName"]);
                    product.Image = await RetrieveAssetField((Dictionary<string, object>)fields["image"]);
                    product.Description = RetrieveStringField((Dictionary<string, object>)fields["description"]);

                    productsResult.Add(product);
                }

            }
        }

        private static SystemProperties RetrieveSystemProperty(IHit<dynamic> hit)
        {
            var sysHit = hit.Source["sys"];
            SystemProperties sys = new SystemProperties
            {
                Id = sysHit["id"]
            };
            return sys;
        }

        private static string RetrieveStringField(Dictionary<string, object> dict)
        {
            var productName = "";
            foreach (KeyValuePair<string, object> keyValue in dict)
            {
                productName = (string)keyValue.Value;
            }

            return productName;
        }

        private async Task<List<Asset>> RetrieveAssetField(Dictionary<string, object> dict)
        {
            List<Asset> images = new List<Asset>();

            foreach (KeyValuePair<string, object> lan in dict)
            {
                List<object> lanlist = (List<object>)lan.Value;
                Dictionary<string, object> lanDict = (Dictionary<string, object>)lanlist[0];
                foreach (KeyValuePair<string, object> item in lanDict)
                {
                    Dictionary<string, object> sysDict = (Dictionary<string, object>)item.Value;

                    var qb = QueryBuilder<Asset>.New.ContentTypeIs("product");
                    var image = await _contentfulClient.GetAsset((string)sysDict["id"], qb);

                    images.Add(image);
                }
            }

            return images;
        }
    }
}
