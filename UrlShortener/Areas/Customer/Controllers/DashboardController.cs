using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly UrlDataService _urlDataService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardController(UrlDataService dataService, IHttpContextAccessor httpContextAccessor)
        {
            _urlDataService = dataService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UrlPair urlPair)
        {
            var userName = _httpContextAccessor.HttpContext.User.Identity.Name;

            if (urlPair.LongUrl != null)
            {
                urlPair = _urlDataService.cleanLongUrl(urlPair);

                // create the HttpClient object
                var client = new HttpClient();

                // set the base address for your API
                client.BaseAddress = new Uri("https://localhost:7125/ShortenApi/");

                // make the GET request to the API to get the long URL
                var response = await client.GetAsync($"{urlPair.LongUrl}/{userName}");

                if (response.IsSuccessStatusCode)
                {
                    // deserialize the response using Newtonsoft.Json
                    var content = await response.Content.ReadAsStringAsync();
                    var url = JsonConvert.DeserializeObject<UrlPair>(content);

                    // adding the current url to the short sub url
                    string currentUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/";
                    url.ShortUrl = url.ShortUrl.Insert(0, currentUrl);

                    // redirect to the long URL
                    return View(url);
                }
                else
                {
                    // handle the case where the API returns an error status code
                    return View("Error");
                }
            }
            else
            {
                return View();
            }
        }

        public IActionResult Details(string longUrl)
        {
            var urlPair = _urlDataService.GetUrlLongToPair(longUrl);
            if(urlPair != null)
            {
                ViewBag.urlPair = urlPair;
                return View();
            }
            else
            {
                return View("Error");
            }
        }
    }
}
