﻿using Domain;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Validators
{
    public class CryptoValidator : ICryptoValidator
    {
        private readonly HttpClient _httpClient;
        private readonly ThirdPartySettings _apiSettings;
        private readonly IMemoryCache _cache;

        private const string CacheKey = "CryptoSymbolMap";
        private const int CacheDurationInMinutes = 60; // Adjust as needed

        public CryptoValidator(HttpClient httpClient, IOptions<ThirdPartySettings> apiSettings, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
            _cache = cache;
        }

        public async Task<bool> IsValidCryptoSymbolAsync(string symbol)
        {
            var cryptoMap = await GetCryptoMapAsync();
            return cryptoMap.Any(c => c.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<List<CryptoSymbol>> GetCryptoMapAsync()
        {
            if (_cache.TryGetValue(CacheKey, out List<CryptoSymbol> cachedMap))
            {
                return cachedMap;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, "https://pro-api.coinmarketcap.com/v1/cryptocurrency/map");
            request.Headers.Add("X-CMC_PRO_API_KEY", _apiSettings.CoinMarketCap.ApiKey);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Failed to fetch cryptocurrency data.");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var cryptoData = JsonSerializer.Deserialize<CryptoMapResponse>(jsonResponse);

            var map = cryptoData.Data.ToList();
            _cache.Set(CacheKey, map, TimeSpan.FromMinutes(CacheDurationInMinutes));

            return map;
        }
    }

    public class CryptoMapResponse
    {
        public List<CryptoSymbol> Data { get; set; }
    }

    public class CryptoSymbol
    {
        public string Symbol { get; set; }
        public string Name { get; set; }
    }
}
