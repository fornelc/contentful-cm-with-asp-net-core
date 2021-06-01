using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Products.Models;
using Contentful.Core;
using Contentful.Core.Search;

namespace Products.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IContentfulClient _client;

        public HomeController(ILogger<HomeController> logger, IContentfulClient client)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("HomeController - Index() - start.");
            var qb = QueryBuilder<Product>.New.ContentTypeIs("product");

            var products = await _client.GetEntries<Product>(qb);
            return View(products);
        }

        public async Task<IActionResult> Details(string itemid)
        {
            _logger.LogInformation("HomeController - Details() - Retrieving details of product with id {Id}", itemid);
            var qb = QueryBuilder<Product>.New.ContentTypeIs("product").FieldEquals(f => f.Sys.Id, itemid);

            var entry = (await _client.GetEntries<Product>(qb)).FirstOrDefault();
            return View(entry);
        }

        public async Task<IActionResult> ByTag(string id)
        {
            var qb = QueryBuilder<Product>.New.ContentTypeIs("product").FieldEquals(f => f.Tags, id);

            var entries = await _client.GetEntries<Product>(qb);
            return View("Index", entries);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
