using System.Text.Json.Serialization;

namespace Application.CoinMarketcap.Mapp
{
    public class CryptoMapResponse
    {
        [JsonPropertyName("data")]
        public IEnumerable<CryptoCurrency> Data { get; set; }
    }
}
