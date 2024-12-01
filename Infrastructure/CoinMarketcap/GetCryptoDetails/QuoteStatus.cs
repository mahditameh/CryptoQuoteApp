using System.Text.Json.Serialization;

namespace Infrastructure.CoinMarketcap.GetCryptoDetails
{
    public class QuoteStatus
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("error_message")]
        public object ErrorMessage { get; set; }

        [JsonPropertyName("elapsed")]
        public int Elapsed { get; set; }

        [JsonPropertyName("credit_count")]
        public int CreditCount { get; set; }

        [JsonPropertyName("notice")]
        public object Notice { get; set; }
    }

}
