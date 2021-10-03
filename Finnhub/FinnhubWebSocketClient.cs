using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoPortfolio.Finnhub
{
    public class FinnhubWebSocketClient
    {
        private static readonly ClientWebSocket client = new();

        public async Task Connect(CancellationToken cancellationToken)
        {
            await client.ConnectAsync(new Uri($"{AppConfiguration.Finnhub.WebSocketUri}?token={AppConfiguration.Finnhub.ApiKey}"), cancellationToken);
        }

        public async Task Subscribe(string symbol, CancellationToken cancellationToken)
        {
            await client.SendAsJsonAsync(new { type = "subscribe", symbol }, cancellationToken);
        }

        public async Task<Trades> Receive(CancellationToken cancellationToken)
        {
            var trades = await client.ReceiveAsAsync<Trades>(cancellationToken);
            return trades;
        }
    }
}