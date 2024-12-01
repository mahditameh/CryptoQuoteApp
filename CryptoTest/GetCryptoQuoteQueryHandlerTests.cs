using Application.Handlers;
using Application.Queries;
using Domain;
using Domain.Services;
using Domain.ValueObjects;
using Moq;

namespace CryptoTest
{
    public class GetCryptoQuoteQueryHandlerTests
    {
        private readonly Mock<ICryptoRepository> _mockRepo;
        private readonly Mock<ICryptoValidator> _mockValidator;
        private readonly CryptoDomainService _domainService;
        private readonly GetCryptoQuoteQueryHandler _handler;

        public GetCryptoQuoteQueryHandlerTests()
        {
            _mockRepo = new Mock<ICryptoRepository>();
            _mockValidator = new Mock<ICryptoValidator>();
            _domainService = new CryptoDomainService();
            _handler = new GetCryptoQuoteQueryHandler(_mockRepo.Object, _mockValidator.Object, _domainService);
        }

        [Fact]
        public async Task Handle_ValidCryptoCode_ReturnsSuccessWithQuote()
        {
            // Arrange
            var cryptoCode = "BTC";
            var query = new GetCryptoQuoteQuery { CryptoCode = cryptoCode };

            var expectedPrice = new Price(20000m, "USD");
            var exchangeRates = new Dictionary<string, Price>
        {
            { "EUR", new Price(0.85m, "EUR") },
            { "GBP", new Price(0.75m, "GBP") }
        };

            var expectedConvertedPrices = new Dictionary<string, decimal>
        {
            { "EUR", 17000m },
            { "GBP", 15000m }
        };

            // Mock the validator and repository
            _mockValidator.Setup(v => v.IsValidCryptoSymbolAsync(cryptoCode))
                          .ReturnsAsync(true); // Validator returns valid
            _mockRepo.Setup(repo => repo.GetCryptoPriceAsync(cryptoCode, "USD"))
                     .ReturnsAsync(expectedPrice.Value);
            _mockRepo.Setup(repo => repo.GetExchangeRatesAsync())
                     .ReturnsAsync(exchangeRates.ToDictionary(k => k.Key, v => v.Value.Value));

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(cryptoCode.ToUpper(), result.Data.Symbol);
            Assert.Equal(expectedPrice.Value, result.Data.Price);
            Assert.Equal(expectedConvertedPrices, result.Data.ConvertedPrices);
        }

        [Fact]
        public async Task Handle_InvalidCryptoCode_ReturnsFailure()
        {
            // Arrange
            var cryptoCode = "INVALID";
            var query = new GetCryptoQuoteQuery { CryptoCode = cryptoCode };

            // Mock the validator
            _mockValidator.Setup(v => v.IsValidCryptoSymbolAsync(cryptoCode))
                          .ReturnsAsync(false); // Validator returns invalid

            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Equal($"The cryptocurrency code '{cryptoCode}' is invalid.", result.Error);
        }
    }

}
