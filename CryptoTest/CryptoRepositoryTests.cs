using CryptoTest.Stubs;
using Infrastructure;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using System.Text;

namespace CryptoTest
{
    public class CryptoRepositoryTests
    {
        private readonly Mock<IOptions<ThirdPartySettings>> _mockOptions;
        private readonly ThirdPartySettings _settings;
        private readonly string _baseCurrency = "USD";

        public CryptoRepositoryTests()
        {
            // Mock ThirdPartySettings
            _settings = new ThirdPartySettings
            {
                CoinMarketCap = new CoinMarketCapSettings
                {
                    ApiKey = "dummy_api_key",
                    Url = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest"
                },
                ExchangeRates = new ExchangeRatesSettings
                {
                    ApiKey = "dummy_exchange_api_key",
                    Url = "https://api.apilayer.com/exchangerates_data/latest?",
                    Currencies = "EUR,BRL,GBP,AUD"
                }
            };

            _mockOptions = new Mock<IOptions<ThirdPartySettings>>();
            _mockOptions.Setup(o => o.Value).Returns(_settings);
        }

        [Fact]
        public async Task GetCryptoPriceAsync_ValidCryptoCode_ReturnsCorrectPrice()
        {
            // Arrange
            var mockJsonResponse = new CryptoResponseBuilder()
                 .AddCryptoPrice("BTC", 20000m)
                 .Build();

            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(mockJsonResponse, Encoding.UTF8, "application/json")
            };

            var handler = new HttpMessageHandlerStub(mockResponse);
            var httpClient = new HttpClient(handler);
            var repository = new CryptoRepository(httpClient, _mockOptions.Object);

            // Act
            var price = await repository.GetCryptoPriceAsync("BTC", "USD");

            // Assert
            Assert.Equal(20000m, price);
        }

        [Fact]
        public async Task GetCryptoPriceAsync_InvalidCryptoCode_ThrowsException()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound);
            var handler = new HttpMessageHandlerStub(responseMessage);
            var httpClient = new HttpClient(handler);
            var repository = new CryptoRepository(httpClient, _mockOptions.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
                await repository.GetCryptoPriceAsync("INVALID_CURRENCY", _baseCurrency));
        }
    }
}
