﻿using Domins;
using Infrastructure.CoinMarketcap;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Web;

namespace Infrastructure
{
    public class CryptoRepository : ICryptoRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private const string CoinMarketCapApiKeyHeader = "X-CMC_PRO_API_KEY";
        private const string ExchangeApiKeyHeader = "apikey";
        private const string AcceptHeader = "Accepts";
        private const string AcceptHeaderValue = "application/json";

        public CryptoRepository(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<decimal> GetCryptoPriceAsync(string cryptoCode, string symbol)
        {
            var requestUrl = ConstructCoinMarketCapUrl(cryptoCode, symbol);
            var response = await SendRequestAsync(requestUrl, new Dictionary<string, string>
            {
                { CoinMarketCapApiKeyHeader, _configuration["CoinMarketCap:ApiKey"] },
                { AcceptHeader, AcceptHeaderValue }
            });

            var quotes = JsonSerializer.Deserialize<QuoteJsonModel>(response);
            return (decimal)quotes.Data.BTC.Quote.USD.Price;
        }

        public async Task<Dictionary<string, decimal>> GetExchangeRatesAsync()
        {
            var apiKey = _configuration["ExchangeRates:ApiKey"];
            var currencies = _configuration["ExchangeRates:Currencies"];
            var requestUrl = $"{_configuration["ExchangeRates:Url"]}access_key={apiKey}&symbols={currencies}&base=USD";

            // Ensure the complete URL is formed correctly
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(currencies))
            {
                throw new ArgumentException("API Key or currencies are not configured correctly.");
            }

            var response = await SendRequestAsync(requestUrl, new Dictionary<string, string>
            {
                { ExchangeApiKeyHeader, apiKey }
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
            var url = _configuration["CoinMarketCap:Url"];
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