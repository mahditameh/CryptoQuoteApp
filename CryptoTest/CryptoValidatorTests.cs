using Domain;
using Infrastructure.Configurations;
using Infrastructure.Validators;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text.Json;

namespace CryptoTest
{
    public class CryptoValidatorTests
    {
        private readonly MockHttpMessageHandler _mockHttp;
        private readonly HttpClient _httpClient;
        private readonly CryptoValidator _validator;
        private readonly ThirdPartySettings _testSettings;
        private readonly IMemoryCache _cache;

        public CryptoValidatorTests()
        {
            // Set up mock HTTP handler and client
            _mockHttp = new MockHttpMessageHandler();
            _httpClient = new HttpClient(_mockHttp);

            // Initialize test settings
            _testSettings = new ThirdPartySettings
            {
                CoinMarketCap = new CoinMarketCapSettings
                {
                    ApiKey = "2f1bf525-fdaa-4879-919f-cfceee5c2757",
                    Url = "https://pro-api.coinmarketcap.com/v1/cryptocurrency/map"
                }
            };

            // Initialize in-memory cache
            _cache = new MemoryCache(new MemoryCacheOptions());

            // Wrap settings in IOptions<T>
            var options = Options.Create(_testSettings);

            // Initialize the CryptoValidator with dependencies
            _validator = new CryptoValidator(_httpClient, options, _cache);
        }

        [Fact]
        public async Task IsValidCryptoSymbolAsync_ValidSymbol_ReturnsTrue()
        {
            // Arrange
            const string validSymbol = "BTC";
            var responseJson = JsonSerializer.Serialize(new CryptoMapResponse
            {
                Data = new List<CryptoSymbol>
                {
                    new CryptoSymbol { Symbol = "BTC", Name = "Bitcoin" },
                    new CryptoSymbol { Symbol = "ETH", Name = "Ethereum" }
                }
            });

            _mockHttp.When(_testSettings.CoinMarketCap.Url)
                     .WithHeaders("X-CMC_PRO_API_KEY", _testSettings.CoinMarketCap.ApiKey)
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
            const string invalidSymbol = "INVALID";
            var responseJson = JsonSerializer.Serialize(new CryptoMapResponse
            {
                Data = new List<CryptoSymbol>
                {
                    new CryptoSymbol { Symbol = "BTC", Name = "Bitcoin" },
                    new CryptoSymbol { Symbol = "ETH", Name = "Ethereum" }
                }
            });

            _mockHttp.When(_testSettings.CoinMarketCap.Url)
                     .WithHeaders("X-CMC_PRO_API_KEY", _testSettings.CoinMarketCap.ApiKey)
                     .Respond("application/json", responseJson);

            // Act
            var result = await _validator.IsValidCryptoSymbolAsync(invalidSymbol);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsValidCryptoSymbolAsync_CachesData_ReturnsCachedResult()
        {
            // Arrange
            const string validSymbol = "BTC";

            // Preload the cache with mock data
            var cachedData = new List<CryptoSymbol>
            {
                new CryptoSymbol { Symbol = "BTC", Name = "Bitcoin" },
                new CryptoSymbol { Symbol = "ETH", Name = "Ethereum" }
            };
            _cache.Set("CryptoSymbolMap", cachedData);

            // Act 
            var result = await _validator.IsValidCryptoSymbolAsync(validSymbol);

            // Assert
            Assert.True(result);


            _mockHttp.VerifyNoOutstandingRequest();
        }

        [Fact]
        public async Task IsValidCryptoSymbolAsync_ApiError_ReturnsFalse()
        {
            // Arrange
            const string symbol = "BTC";

            _mockHttp.When(_testSettings.CoinMarketCap.Url)
                     .WithHeaders("X-CMC_PRO_API_KEY", _testSettings.CoinMarketCap.ApiKey)
                     .Respond(HttpStatusCode.BadRequest);

            // Act
            var result = await _validator.IsValidCryptoSymbolAsync(symbol);

            // Assert
            Assert.False(result); // Should return false due to fallback behavior
        }



    }
}
