using System.Text.Json.Serialization;

namespace Application.CoinMarketcap.Mapp
{
    public class CryptoCurrency
    {
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
