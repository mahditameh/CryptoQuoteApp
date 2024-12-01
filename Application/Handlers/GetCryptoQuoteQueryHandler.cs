using Application.Common;
using Application.DTO;
using Application.Queries;
using Domain;
using MediatR;

namespace Application.Handlers
{
    public class GetCryptoQuoteQueryHandler : IRequestHandler<GetCryptoQuoteQuery, ApiResult<CryptoQuoteDto>>
    {
        private readonly ICryptoRepository _cryptoRepository;
        private readonly ICryptoValidator _cryptoValidator;

        public GetCryptoQuoteQueryHandler(ICryptoRepository cryptoRepository, ICryptoValidator cryptoValidator)
        {
            _cryptoRepository = cryptoRepository;
            _cryptoValidator = cryptoValidator;
        }

        public async Task<ApiResult<CryptoQuoteDto>> Handle(GetCryptoQuoteQuery request, CancellationToken cancellationToken)
        {
            if (!await _cryptoValidator.IsValidCryptoSymbolAsync(request.CryptoCode))
            {
                return ApiResult<CryptoQuoteDto>.Failure($"The cryptocurrency code '{request.CryptoCode}' is invalid.");
            }

            var price = await _cryptoRepository.GetCryptoPriceAsync(request.CryptoCode, "USD");
            var exchangeRates = await _cryptoRepository.GetExchangeRatesAsync();

            var convertedPrices = exchangeRates.ToDictionary(
                rate => rate.Key,
                rate => price * rate.Value
            );

            return ApiResult<CryptoQuoteDto>.Success(new CryptoQuoteDto
            {
                Price = price,
                Symbol = request.CryptoCode.ToUpper(),
                ConvertedPrices = convertedPrices
            });
        }
    }

}
