using System.Text.Json.Serialization;

namespace Infrastructure.CoinMarketcap
{
    public class USD
    {
        [JsonPropertyName("price")]
        public float Price { get; set; }

        [JsonPropertyName("volume_24h")]
        public float Volume24h { get; set; }

        [JsonPropertyName("volume_change_24h")]
        public float VolumeChange24h { get; set; }

        [JsonPropertyName("percent_change_1h")]
        public float PercentChange1h { get; set; }

        [JsonPropertyName("percent_change_24h")]
        public float PercentChange24h { get; set; }

        [JsonPropertyName("percent_change_7d")]
        public float PercentChange7d { get; set; }

        [JsonPropertyName("percent_change_30d")]
        public float PercentChange30d { get; set; }

        [JsonPropertyName("percent_change_60d")]
        public float PercentChange60d { get; set; }

        [JsonPropertyName("percent_change_90d")]
        public float PercentChange90d { get; set; }

        [JsonPropertyName("market_cap")]
        public float MarketCap { get; set; }

        [JsonPropertyName("market_cap_dominance")]
        public float MarketCapDominance { get; set; }

        [JsonPropertyName("fully_diluted_market_cap")]
        public float FullyDilutedMarketCap { get; set; }

        [JsonPropertyName("tvl")]
        public object Tvl { get; set; }

        [JsonPropertyName("last_updated")]
        public DateTime LastUpdated { get; set; }
    }
}