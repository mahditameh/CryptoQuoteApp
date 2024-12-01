using System.Text.Json.Serialization;

namespace Infrastructure.CoinMarketcap.GetCryptoDetails
{
    public class Quote
    {
        [JsonPropertyName("USD")]
        public USD USD { get; set; }
    }
}
