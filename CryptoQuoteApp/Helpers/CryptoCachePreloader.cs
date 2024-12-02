using Domain.Services;

namespace CryptoQuoteApp.Helpers
{
    public class CryptoCachePreloader : BackgroundService
    {
        private readonly ICryptoValidator _cryptoValidator;
        private readonly ILogger<CryptoCachePreloader> _logger;

        public CryptoCachePreloader(ICryptoValidator cryptoValidator, ILogger<CryptoCachePreloader> logger)
        {
            _cryptoValidator = cryptoValidator;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await _cryptoValidator.IsValidCryptoSymbolAsync("BTC"); // Warm-up cache with any valid symbol
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "BackgroundServices");
            }
        }
    }

}
