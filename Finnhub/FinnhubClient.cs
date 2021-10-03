using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoPortfolio.Finnhub
{
    public class FinnhubClient
    {
        private static readonly HttpClient client = new();

        public async Task<Quote> GetQuote(string symbol, CancellationToken cancellationToken)
        {
            var request = $"{AppConfiguration.Finnhub.ApiUri}/quote?symbol={symbol}&token={AppConfiguration.Finnhub.ApiKey}";
            var json = await client.GetStringAsync(request, cancellationToken);

            var quote = JsonConvert.DeserializeObject<Quote>(json);

            return quote;
        }
    }
}