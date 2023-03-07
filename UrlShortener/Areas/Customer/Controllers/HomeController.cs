using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UrlShortener.Models;
using UrlShortener.Services;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Identity;
using System;
using QRCoder;
using System.Drawing;

namespace UrlShortener.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly UrlDataService _urlDataService;

        public HomeController(UrlDataService urlDataService)
        {
            _urlDataService = urlDataService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UrlPair urlPair)
        {
            if (urlPair.LongUrl != null)
            {
                urlPair = _urlDataService.cleanLongUrl(urlPair);

                // create the HttpClient object
                var client = new HttpClient();

                // set the base address for your API
                client.BaseAddress = new Uri("https://localhost:7125/ShortenApi/");

                // make the GET request to the API to get the long URL
                var response = await client.GetAsync(urlPair.LongUrl);

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

        [Route("{subUrl}")]
        public async Task<IActionResult> shortUrl(string subUrl)
        {
            // create the HttpClient object
            var client = new HttpClient();

            // set the base address for your API
            client.BaseAddress = new Uri("https://localhost:7125/ShortenApi/");

            // make the GET request to the API to get the long URL
            var response = await client.GetAsync(subUrl);

            if (response.IsSuccessStatusCode)
            {
                // deserialize the response using Newtonsoft.Json
                var content = await response.Content.ReadAsStringAsync();
                var url = JsonConvert.DeserializeObject<UrlPair>(content);

                string ip = GetIp();
                await _urlDataService.AddEntryAsync(url, ip);

                // redirect to the long URL
                return Redirect("https://www." + url.LongUrl);
            }
            else
            {
                // handle the case where the API returns an error status code
                return View("Error");
            }
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactModel? cm)
        {
            if(ModelState.IsValid)
            {
                ViewBag.thankYou = true;
                _urlDataService.AddMessage(cm);
                TempData["Success"] = "message sent succesfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult ApiDocumantation()
        {
            return View();
        }

        public string GetIp()
        {
            var address = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            return address;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}