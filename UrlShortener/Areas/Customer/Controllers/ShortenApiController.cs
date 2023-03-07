using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Areas.Customer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShortenApiController : ControllerBase
    {
        private readonly UrlDataService _urlService;
        public ShortenApiController(UrlDataService urlService)
        {
            _urlService = urlService;
        }

        [HttpGet("{url}/{userName}")]
        public ActionResult<UrlPair> Get(string url, string userName)
        {
            if (url.Contains('.'))
            {
                var urlPair = _urlService.GetUrlLongToPair(url);

                if (urlPair is null)
                    urlPair = _urlService.Create(url, userName);

                return urlPair;
            }
            else
            {
                var urlPair = _urlService.GetUrlShortToPair(url);

                if (urlPair is null)
                    urlPair = _urlService.Create(url, userName);

                return urlPair;
            }
        }

        [HttpGet("{url}")]
        public ActionResult<UrlPair> Get(string url)
        {
            if (url.Contains('.'))
            {
                var urlPair = _urlService.GetUrlLongToPair(url);

                if (urlPair is null)
                    urlPair = _urlService.Create(url, null);

                return urlPair;
            }
            else
            {
                var urlPair = _urlService.GetUrlShortToPair(url);

                if (urlPair is null)
                    urlPair = _urlService.Create(url, null);

                return urlPair;
            }
        }
    }
}
