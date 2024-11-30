using Applications.Contracts;
using CryptoQuoteApp.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace CryptoQuoteApp.Controllers
{
    [SecurityHeadersAttribute]
    public class CryptoController : Controller
    {
        private readonly ICryptoService _cryptoService;

        public CryptoController(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        // GET: /Crypto
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetQuotes(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return RedirectToAction("Index");
            }
            var quotes = await _cryptoService.GetCryptoQuoteAsync(code);



            return View("Quotes", quotes); // Return the Quotes view with results
        }
    }
}
