using Humanizer.Bytes;
using IDistributedCacheApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.CodeAnalysis.RulesetToEditorconfig;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Internal.VisualStudio.PlatformUI;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Text.Json;

namespace IDistributedCacheApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDistributedCache _distributedCache;

        public HomeController(ILogger<HomeController> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;
        }

        public IActionResult Index()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();

            options.SlidingExpiration = TimeSpan.FromSeconds(20);

            _distributedCache.SetString("murat", "silay", options);

            return View();
        }

        public async Task<IActionResult> ClassKaydet()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();

            options.SlidingExpiration = TimeSpan.FromSeconds(40);

            Product product = new Product
            {
                Id = 1,
                Description = "Test",
                Name = "TestName"
            };

            var jsonProduct = JsonSerializer.Serialize(product);

            await _distributedCache.SetStringAsync("product:2", jsonProduct, options);


            return View();
        }

        public IActionResult ResimKaydet()
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();

            options.SlidingExpiration = TimeSpan.FromSeconds(20);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/sddefault.jpg");
            var image = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("resim", image);



            return View();
        }
        public IActionResult ResimGoster()
        {
            var resim = _distributedCache.Get("resim");
           
            return File(resim, "image/jpg");
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
