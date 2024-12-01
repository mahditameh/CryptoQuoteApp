using CryptoTest.Stubs;
using Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace CryptoTest
{
    public class CryptoRepositoryTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly string _baseCurrency = "USD";

        public CryptoRepositoryTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(cfg => cfg["CoinMarketCap:ApiKey"]).Returns("dummy_api_key");
        }

        [Fact]
        public async Task GetCryptoPriceAsync_ValidCryptoCode_ReturnsCorrectPrice()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(new
                {
                    data = new
                    {
                        BTC = new { price = 20000 }
                    }
                })
            };

            var handler = new HttpMessageHandlerStub(mockResponse);
            var httpClient = new HttpClient(handler);
            var repository = new CryptoRepository(httpClient, _mockConfiguration.Object);

            // Act
            var price = await repository.GetCryptoPriceAsync("BTC", _baseCurrency);

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
            var repository = new CryptoRepository(httpClient, _mockConfiguration.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(async () =>
                await repository.GetCryptoPriceAsync("INVALID_CURRENCY", _baseCurrency));
        }
    }
}
