using Domins;
using Infrastructure.CoinMarketcap;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Net;
using System.Text.Json;
using System.Web;

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

        public async Task<decimal> GetCryptoPriceAsync(string cryptoCode, string symbol)
        {
            var apiKey = _configuration["CoinMarketCap:ApiKey"];
            var url = _configuration["CoinMarketCap:Url"];
            var uRLBuilder = new UriBuilder($"{url}");
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString["symbol"] = cryptoCode;
            queryString["convert"] = symbol;
            uRLBuilder.Query = queryString.ToString();

            var client = new WebClient();
            client.Headers.Add("X-CMC_PRO_API_KEY", apiKey);
            client.Headers.Add("Accepts", "application/json");
            var response = client.DownloadString(uRLBuilder.ToString());
            var quotes = JsonSerializer.Deserialize<QuoteJsonModel>(response);
            return (decimal)quotes.Data.BTC.Quote.USD.Price;

        }

        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync()
        {
            var apiKey = _configuration["ExchangeRates:ApiKey"];
            var url = _configuration["ExchangeRates:Url"];
            var currencies = _configuration["ExchangeRates:Currencies"];
            var requestUrl = $"{url}symbols={currencies}&base=USD";

            var client = new RestClient();
            var request = new RestRequest(requestUrl, Method.Get);

            request.AddHeader("apikey", apiKey);
            RestResponse restResponse = await client.ExecuteAsync(request);
            if (!restResponse.IsSuccessful)
            {
                throw new Exception($"Error fetching exchange rates: {restResponse.ErrorMessage}");
            }

            var responseContent = restResponse.Content;


            ExchangeRateResponse? exchangeRates = JsonSerializer.Deserialize<ExchangeRateResponse>(responseContent);


            return exchangeRates.Rates;
        }
    }
}

