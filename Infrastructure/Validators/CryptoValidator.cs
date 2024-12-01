using Applications.CoinMarketcap.Mapp;
using Domins;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Infrastructure.Validators
{

    public class CryptoValidator : ICryptoValidator
    {
        private readonly HttpClient _httpClient;
        private readonly ThirdPartySettings _apiSettings;
        public CryptoValidator(HttpClient httpClient, IOptions<ThirdPartySettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }

        public async Task<bool> IsValidCryptoSymbolAsync(string symbol)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://pro-api.coinmarketcap.com/v1/cryptocurrency/map");
            request.Headers.Add("X-CMC_PRO_API_KEY", _apiSettings.CoinMarketCap.ApiKey);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode) return false;

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var cryptoData = JsonSerializer.Deserialize<CryptoMapResponse>(jsonResponse);

            return cryptoData.Data.Any(c => c.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
        }
    }

}
