using Domins;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Infrastructure
{
    public class CryptoRepository : ICryptoRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CryptoRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<decimal> GetCryptoPriceAsync(string cryptoCode)
        {
            var apiKey = _configuration["CoinMarketCap:ApiKey"];
            var requestUrl = $"https://pro-api.coinmarketcap.com/v1/cryptocurrency/quotes/latest?symbol={cryptoCode}&convert=USD";
            _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);
            var response = await _httpClient.GetFromJsonAsync<List<CryptoQuote>>(requestUrl);

            return response.First().Price;
        }

        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync(decimal priceInUSD)
        {
            var apiKey = _configuration["ExchangeRates:ApiKey"];
            var requestUrl = $"https://api.exchangeratesapi.io/v1/latest?access_key={apiKey}&base=USD";
            var response = await _httpClient.GetFromJsonAsync<Dictionary<string, decimal>>(requestUrl);
            return response;
        }
    }
}

