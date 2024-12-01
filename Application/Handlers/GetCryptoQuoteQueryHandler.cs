using Application.Common;
using Application.DTO;
using Application.Queries;
using Domain;
using Domain.Services;
using Domain.ValueObjects;
using MediatR;

namespace Application.Handlers
{
    public class GetCryptoQuoteQueryHandler : IRequestHandler<GetCryptoQuoteQuery, ApiResult<CryptoQuoteDto>>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly ICryptoValidator _cryptoValidator;
        private readonly CryptoDomainService _cryptoDomainService;

        public GetCryptoQuoteQueryHandler(
            ICryptoRepository cryptoRepository,
            ICryptoValidator cryptoValidator,
            CryptoDomainService cryptoDomainService)
        {
            _cryptoRepository = cryptoRepository;
            _cryptoValidator = cryptoValidator;
            _cryptoDomainService = cryptoDomainService;
        }

        public async Task<ApiResult<CryptoQuoteDto>> Handle(GetCryptoQuoteQuery request, CancellationToken cancellationToken)
        {
            // Validate the cryptocurrency symbol
            if (!await _cryptoValidator.IsValidCryptoSymbolAsync(request.CryptoCode))
            {
                return ApiResult<CryptoQuoteDto>.Failure($"The cryptocurrency code '{request.CryptoCode}' is invalid.");
            }

            var basePriceValue = await _cryptoRepository.GetCryptoPriceAsync(request.CryptoCode, "USD");
            var basePrice = new Price(basePriceValue, "USD");

            var exchangeRates = await _cryptoRepository.GetExchangeRatesAsync();

            var ratesAsPrices = exchangeRates.ToDictionary(
                rate => rate.Key,
                rate => new Price(rate.Value, rate.Key)
            );
            var convertedPrices = _cryptoDomainService.CalculateEquivalentRates(basePrice, ratesAsPrices);


            return ApiResult<CryptoQuoteDto>.Success(new CryptoQuoteDto
            {
                Price = basePrice.Value,
                Symbol = request.CryptoCode.ToUpper(),
                ConvertedPrices = convertedPrices
            });
        }
    }
}
