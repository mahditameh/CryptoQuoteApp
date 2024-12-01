using Applications.Contracts;
using Applications.DTO;
using Domins;
using System.Runtime.CompilerServices;


[assembly: InternalsVisibleTo("CryptoTest")]
namespace Applications.Services
{

    internal class CryptoService : ICryptoService
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly string _baseCurrency = "USD";
        public CryptoService(ICryptoRepository cryptoRepository)
        {
            _cryptoRepository = cryptoRepository;
        }

        public async Task<CryptoQuoteDto> GetCryptoQuoteAsync(string cryptoCode)
        {
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
                // Calculate equivalent amount in selected currency
                equivalentAmounts[currency] = priceInUSD * currenciesRates[currency];
            }

            return equivalentAmounts;
        }
    }
}
