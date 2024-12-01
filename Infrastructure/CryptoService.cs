using Application.Contracts;
using Application.DTO;
using Domain;
using System.Runtime.CompilerServices;



[assembly: InternalsVisibleTo("CryptoTest")]
[assembly: InternalsVisibleTo("InfrastructureConfig")]
namespace Infrastructure.Services
{
    internal class CryptoService : ICryptoService
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly ICryptoValidator _cryptoValidator; // Add Validator
        private readonly string _baseCurrency = "USD";

        public CryptoService(ICryptoRepository cryptoRepository, ICryptoValidator cryptoValidator)
        {
            _cryptoRepository = cryptoRepository;
            _cryptoValidator = cryptoValidator; // Initialize Validator
        }

        public async Task<CryptoQuoteDto> GetCryptoQuoteAsync(string cryptoCode)
        {

            if (!await _cryptoValidator.IsValidCryptoSymbolAsync(cryptoCode))
            {
                throw new ArgumentException($"The cryptocurrency code '{cryptoCode}' is invalid.");
            }

            var cryptoPrice = await _cryptoRepository.GetCryptoPriceAsync(cryptoCode, _baseCurrency);

            return new CryptoQuoteDto()
            {
                Price = cryptoPrice,
                Symbol = cryptoCode.ToUpper(),
                ConvertedPrices = await GettingEquivalentRateOtherCurrencies(cryptoPrice, _baseCurrency)
            };
        }

        private async Task<Dictionary<string, decimal>> GettingEquivalentRateOtherCurrencies(decimal priceInUSD, string symbol)
        {
            var currenciesRates = await _cryptoRepository.GetExchangeRatesAsync();
            var equivalentAmounts = new Dictionary<string, decimal>();

            foreach (var currency in currenciesRates.Keys)
            {
                equivalentAmounts[currency] = priceInUSD * currenciesRates[currency];
            }

            return equivalentAmounts;
        }
    }

}
