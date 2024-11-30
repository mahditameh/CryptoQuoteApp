using Applications.Services;
using Domins;
using Moq;

namespace CryptoTest
{
    public class CryptoServiceTests
    {
        private readonly Mock<ICryptoRepository> _mockRepo;
        private readonly CryptoService _cryptoService;
        private readonly string _baseCurrency = "USD";
        public CryptoServiceTests()
        {
            _mockRepo = new Mock<ICryptoRepository>();
            _cryptoService = new CryptoService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetCryptoQuoteAsync_ValidCryptoCode_ReturnsCorrectQuote()
        {
            // Arrange
            var cryptoCode = "BTC";
            var expectedPrice = 20000m;
            var expectedEquivalentPrices = new Dictionary<string, decimal>
            {
                { "EUR", 340000000 },
                { "GBP", 15000m }
            };
            var expectedConvertedPrices = new Dictionary<string, decimal>
            {
                { "EUR", 6800000000000 },
                { "GBP", 300000000 }
            };
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
        public async Task GetCryptoQuoteAsync_InvalidCryptoCode_ThrowsException()
        {
            // Arrange
            var cryptoCode = "INVALID_CURRENCY";
            _mockRepo.Setup(repo => repo.GetCryptoPriceAsync(cryptoCode, _baseCurrency))
                      .ThrowsAsync(new KeyNotFoundException());

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _cryptoService.GetCryptoQuoteAsync(cryptoCode));
        }
    }
}
