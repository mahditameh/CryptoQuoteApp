﻿using System.Text.Json.Serialization;

namespace Application.CoinMarketcap
{
    public class Quote
    {
        [JsonPropertyName("USD")]
        public USD USD { get; set; }
    }
}
