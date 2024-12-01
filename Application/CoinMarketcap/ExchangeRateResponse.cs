using System.Text.Json.Serialization;

namespace Application.CoinMarketcap
{
    public class ExchangeRateResponse
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("timestamp")]
        public int Timestamp { get; set; }
        [JsonPropertyName("_base")]
        public string Base { get; set; }
        [JsonPropertyName("date")]
        public string Date { get; set; }
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public class Rates
    {
        public float EUR { get; set; }
        public float BRL { get; set; }
        public float GBP { get; set; }
        public float AUD { get; set; }
    }

}

