using Applications.CoinMarketcap;
using Domins;
using Infrastructure.Configurations;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Web;

[assembly: InternalsVisibleTo("CryptoTest")]
namespace Infrastructure
{
    internal class CryptoRepository : ICryptoRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ThirdPartySettings _apiSettings;

        private const string CoinMarketCapApiKeyHeader = "X-CMC_PRO_API_KEY";
        private const string ExchangeApiKeyHeader = "apikey";
        private const string AcceptHeader = "Accepts";
        private const string AcceptHeaderValue = "application/json";

        public CryptoRepository(HttpClient httpClient, IOptions<ThirdPartySettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }

        public async Task<decimal> GetCryptoPriceAsync(string cryptoCode, string symbol)
        {
            var requestUrl = ConstructCoinMarketCapUrl(cryptoCode, symbol);
            var response = await SendRequestAsync(requestUrl, new Dictionary<string, string>
            {
                { CoinMarketCapApiKeyHeader, _apiSettings.CoinMarketCap.ApiKey },
                { AcceptHeader, AcceptHeaderValue }
            });

            var quotes = JsonSerializer.Deserialize<QuoteJsonModel>(response);
            return (decimal)quotes.Data.BTC.Quote.USD.Price;
        }

        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync()
        {
            var requestUrl = $"{_apiSettings.ExchangeRates.Url}access_key={_apiSettings.ExchangeRates.ApiKey}&symbols={_apiSettings.ExchangeRates.Currencies}&base=USD";

            // Ensure the complete URL is formed correctly
            if (string.IsNullOrEmpty(_apiSettings.ExchangeRates.ApiKey) ||
                string.IsNullOrEmpty(_apiSettings.ExchangeRates.Currencies))
            {
                throw new ArgumentException("API Key or currencies are not configured correctly.");
            }

            var response = await SendRequestAsync(requestUrl, new Dictionary<string, string>
            {
                { ExchangeApiKeyHeader, _apiSettings.ExchangeRates.ApiKey }
            });

            // Deserialize the response
            var exchangeRates = JsonSerializer.Deserialize<ExchangeRateResponse>(response);

            // Check for rates being returned
            if (exchangeRates == null || exchangeRates.Rates == null)
            {
                throw new Exception("No exchange rates found in the response.");
            }

            return exchangeRates.Rates;
        }


        private string ConstructCoinMarketCapUrl(string cryptoCode, string symbol)
        {
            var url = _apiSettings.CoinMarketCap.Url;
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["symbol"] = cryptoCode;
            queryString["convert"] = symbol;

            // Construct the full URL with the query string
            return new UriBuilder(url) { Query = queryString.ToString() }.ToString();
        }

        private async Task<string> SendRequestAsync(string requestUrl, Dictionary<string, string> headers = null)
        {
            using var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestUrl);

            // Add any provided headers
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await _httpClient.SendAsync(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error fetching data: {response.ReasonPhrase}");
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}