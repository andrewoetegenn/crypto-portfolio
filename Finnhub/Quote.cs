using Newtonsoft.Json;

namespace CryptoPortfolio.Finnhub
{
    public class Quote
    {
        [JsonProperty("c")]
        public double CurrentPrice { get; set; }
    }
}
