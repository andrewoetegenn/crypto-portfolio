using System.Collections.Generic;
using Newtonsoft.Json;

namespace CryptoPortfolio.Finnhub
{
    public class Trades
    {
        [JsonProperty("data")]
        public List<TradeData> Data { get; set; } = new();
    }
}
