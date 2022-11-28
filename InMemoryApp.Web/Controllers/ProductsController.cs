using InMemoryApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemoryApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IMemoryCache memoryCache;

        public ProductsController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
          
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
                options.AbsoluteExpiration= DateTime.Now.AddMinutes(1);
                options.SlidingExpiration=TimeSpan.FromSeconds(10);
            options.Priority = CacheItemPriority.NeverRemove;
                memoryCache.Set<string>("zaman", DateTime.Now.ToString(),options);
            Product products = new Product()
            {
                Id = 1,
                Name = "Kalem",
                Price = 50
            };
          memoryCache.Set<Product>("product:1", products);
          
           
            return View();
        }
        public IActionResult Show()
        {
            memoryCache.TryGetValue("zaman", out string zamancache);
            ViewBag.zaman = zamancache;
            ViewBag.products = memoryCache.Get<Product>("product:1");
            return View();
        }
    
    }
}
