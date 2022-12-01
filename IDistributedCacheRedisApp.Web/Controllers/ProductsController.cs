using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

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
           await distributedCache.SetStringAsync("name", "Fatih", cacheEntryOptions);
            return View();
        }
        public async Task<IActionResult> Show()
        {
            string name = await distributedCache.GetStringAsync("name");
            ViewBag.name = name;
            return View();
        }
        public async Task<IActionResult> Remove()
        {
            await distributedCache.RemoveAsync("name");
            return View();
        }
    }
}
