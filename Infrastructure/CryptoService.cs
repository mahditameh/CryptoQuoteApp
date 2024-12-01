using Application.Contracts;
using Application.DTO;
using Domain;
using Domain.Services;
using Domain.ValueObjects;
using System.Runtime.CompilerServices;



[assembly: InternalsVisibleTo("CryptoTest")]
[assembly: InternalsVisibleTo("InfrastructureConfig")]
namespace Infrastructure.Services
{
    internal class CryptoService : ICryptoService
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly CryptoDomainService _cryptoDomainService;

        public CryptoService(ICryptoRepository cryptoRepository, CryptoDomainService cryptoDomainService)
        {
            _cryptoRepository = cryptoRepository;
            _cryptoDomainService = cryptoDomainService;
        }

        public async Task<CryptoQuoteDto> GetCryptoQuoteAsync(string cryptoCode)
        {
            var cryptoPrice = new Price(await _cryptoRepository.GetCryptoPriceAsync(cryptoCode, "USD"), "USD");
            var exchangeRates = await _cryptoRepository.GetExchangeRatesAsync();

            var convertedPrices = _cryptoDomainService.CalculateEquivalentRates(
                cryptoPrice,
                exchangeRates.ToDictionary(rate => rate.Key, rate => new Price(rate.Value, rate.Key))
            );

            return new CryptoQuoteDto
            {
                Price = cryptoPrice.Value,
                Symbol = cryptoCode.ToUpper(),
                ConvertedPrices = convertedPrices
            };
        }
    }
}
