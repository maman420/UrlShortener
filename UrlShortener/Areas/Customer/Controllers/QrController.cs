using Microsoft.AspNetCore.Mvc;
using UrlShortener.Services;

namespace UrlShortener.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class QrController : Controller
    {
        private readonly QrGeneratorService _qrGeneratorService;
        public QrController(QrGeneratorService qrGeneratorService)
        {
            _qrGeneratorService = qrGeneratorService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Index(string txtQRCode)
        {
            if (txtQRCode != null)
            {
                var imageInBytes = _qrGeneratorService.generate(txtQRCode);
                return View(imageInBytes);
            }
            else
            {
                return View();
            }
        }
    }
}
