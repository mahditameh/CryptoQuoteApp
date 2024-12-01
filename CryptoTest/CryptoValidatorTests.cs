using Infrastructure.Configurations;
using Infrastructure.Validators;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using System.Net;

namespace CryptoTest
{
    public class CryptoValidatorTests
    {
        private readonly MockHttpMessageHandler _mockHttp;
        private readonly HttpClient _httpClient;
        private readonly CryptoValidator _validator;
        private readonly ThirdPartySettings _testSettings;

        public CryptoValidatorTests()
        {
            _mockHttp = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHttp);


            _testSettings = new ThirdPartySettings
            {
                CoinMarketCap = new CoinMarketCapSettings
                {
                    ApiKey = "2f1bf525-fdaa-4879-919f-cfceee5c2757",
                    Url = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest"
                }
            };

            // Use Options.Create to pass settings as IOptions<T>
            var options = Options.Create(_testSettings);

            // Initialize the validator
            _validator = new CryptoValidator(_httpClient, options);
        }

        [Fact]
        public async Task IsValidCryptoSymbolAsync_ValidSymbol_ReturnsTrue()
        {
            // Arrange
            var validSymbol = "BTC";
            var responseJson = @"{
            ""data"": [
                { ""symbol"": ""BTC"" },
                { ""symbol"": ""ETH"" }
            ]
        }";

            _mockHttp.When("https://pro-api.coinmarketcap.com/v1/cryptocurrency/map")
                     .WithHeaders("X-CMC_PRO_API_KEY", _testSettings.CoinMarketCap.ApiKey) // Match the test API key
                     .Respond("application/json", responseJson);

            // Act
            var result = await _validator.IsValidCryptoSymbolAsync(validSymbol);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsValidCryptoSymbolAsync_InvalidSymbol_ReturnsFalse()
        {
            // Arrange
            var invalidSymbol = "INVALID";
            var responseJson = @"{
            ""data"": [
                { ""symbol"": ""BTC"" },
                { ""symbol"": ""ETH"" }
            ]
        }";

            _mockHttp.When("https://pro-api.coinmarketcap.com/v1/cryptocurrency/map")
                     .WithHeaders("X-CMC_PRO_API_KEY", _testSettings.CoinMarketCap.ApiKey) // Match the test API key
                     .Respond("application/json", responseJson);

            // Act
            var result = await _validator.IsValidCryptoSymbolAsync(invalidSymbol);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsValidCryptoSymbolAsync_ApiError_ReturnsFalse()
        {
            // Arrange
            var symbol = "BTC";

            _mockHttp.When("https://pro-api.coinmarketcap.com/v1/cryptocurrency/map")
                     .WithHeaders("X-CMC_PRO_API_KEY", _testSettings.CoinMarketCap.ApiKey)
                     .Respond(HttpStatusCode.BadRequest);

            // Act
            var result = await _validator.IsValidCryptoSymbolAsync(symbol);

            // Assert
            Assert.False(result);
        }
    }
}

