using Application.Queries;
using CryptoQuoteApp.Helpers.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CryptoQuoteApp.Controllers
{
    [SecurityHeadersAttribute]
    public class CryptoController : Controller
    {
        private readonly IMediator _mediator;

        public CryptoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetQuotes(string code)
        {
            var result = await _mediator.Send(new GetCryptoQuoteQuery { CryptoCode = code });

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error);
                return View("Index"); // Return to index with error
            }

            return View("Quotes", result.Data);
        }
    }

}
