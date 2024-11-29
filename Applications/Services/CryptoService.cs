using Applications.Contracts;
using Applications.DTO;
using Domins;

namespace Applications.Services
{
    public class CryptoService : ICryptoService
    {
        private readonly ICryptoRepository _cryptoRepository;
        public CryptoService(ICryptoRepository cryptoRepository)
        {
            _cryptoRepository = cryptoRepository;
        }

        public async Task<CryptoQuoteDto> GetCryptoQuoteAsync(string cryptoCode)
        {
            var cryptoPrice = await _cryptoRepository.GetCryptoPriceAsync(cryptoCode);

            return new CryptoQuoteDto()
            {
                Price = cryptoPrice,
                Symbol = cryptoCode.ToUpper(),
                ConvertedPrices = await _cryptoRepository.GetExchangeRatesAsync(cryptoPrice)
            };
        }
    }
}
