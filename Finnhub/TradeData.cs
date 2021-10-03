using Newtonsoft.Json;

namespace CryptoPortfolio.Finnhub
{
    public class TradeData
    {
        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("p")]
        public double CurrentPrice { get; set; }
    }
}
