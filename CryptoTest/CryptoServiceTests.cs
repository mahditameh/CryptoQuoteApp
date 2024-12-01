using Applications.Services;
using Domins;
using Moq;

namespace CryptoTest
{
    public class CryptoServiceTests
    {
        private readonly Mock<ICryptoRepository> _mockRepo;
        private readonly Mock<ICryptoValidator> _mockValidator; // Mock for Validator
        private readonly CryptoService _cryptoService;
        private readonly string _baseCurrency = "USD";

        public CryptoServiceTests()
        {
            _mockRepo = new Mock<ICryptoRepository>();
            _mockValidator = new Mock<ICryptoValidator>(); // Initialize Mock
            _cryptoService = new CryptoService(_mockRepo.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task GetCryptoQuoteAsync_ValidCryptoCode_ReturnsCorrectQuote()
        {
            // Arrange
            var cryptoCode = "BTC";
            var expectedPrice = 20000m;
            var expectedEquivalentPrices = new Dictionary<string, decimal>
        {
            { "EUR", 0.85m },
            { "GBP", 0.75m }
        };
            var expectedConvertedPrices = new Dictionary<string, decimal>
        {
            { "EUR", 17000m },
            { "GBP", 15000m }
        };

            _mockValidator.Setup(v => v.IsValidCryptoSymbolAsync(cryptoCode))
                          .ReturnsAsync(true); // Validator returns valid
            _mockRepo.Setup(repo => repo.GetCryptoPriceAsync(cryptoCode, _baseCurrency))
                     .ReturnsAsync(expectedPrice);
            _mockRepo.Setup(repo => repo.GetExchangeRatesAsync())
                     .ReturnsAsync(expectedEquivalentPrices);

            // Act
            var result = await _cryptoService.GetCryptoQuoteAsync(cryptoCode);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cryptoCode.ToUpper(), result.Symbol);
            Assert.Equal(expectedPrice, result.Price);
            Assert.Equal(expectedConvertedPrices, result.ConvertedPrices);
        }

        [Fact]
        public async Task GetCryptoQuoteAsync_InvalidCryptoCode_ThrowsArgumentException()
        {
            // Arrange
            var cryptoCode = "INVALID";
            _mockValidator.Setup(v => v.IsValidCryptoSymbolAsync(cryptoCode))
                          .ReturnsAsync(false); // Validator returns invalid

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _cryptoService.GetCryptoQuoteAsync(cryptoCode));
        }
    }

}
