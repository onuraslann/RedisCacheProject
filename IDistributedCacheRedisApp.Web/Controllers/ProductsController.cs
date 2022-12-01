using IDistributedCacheRedisApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IDistributedCacheRedisApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDistributedCache distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            this.distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();
            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
            Product product = new Product{
                 Id = 1,
                  Name="Bilgisayar",
                   Price=5000
            };
            string jsonProduct =JsonConvert.SerializeObject(product);
            await distributedCache.SetStringAsync("product:1", jsonProduct,cacheEntryOptions);


            return View();
        }
        public async Task<IActionResult> Show()
        {
            string jsonproduct = await distributedCache.GetStringAsync("product:1");
            Product p = JsonConvert.DeserializeObject<Product>(jsonproduct);
            ViewBag.product = p;
            return View();
        }
        public async Task<IActionResult> Remove()
        {
            await distributedCache.RemoveAsync("product:1");
            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/pop-index-2020-chevrolet-corvette-c8-102-1571146873.jpg");
            byte[] imageByte  =System.IO.File.ReadAllBytes(path);
            distributedCache.Set("resim", imageByte);
            return View();
        }
        public IActionResult ImageUrl()
        {
            byte[] resimbyte = distributedCache.Get("resim");

            return File(resimbyte,"image/jpg");
        }
    }
}
