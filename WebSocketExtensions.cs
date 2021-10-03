using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptoPortfolio
{
    public static class WebSocketExtensions
    {
        public static async Task SendAsJsonAsync(this ClientWebSocket client, object target, CancellationToken token)
        {
            var json = JsonConvert.SerializeObject(target);
            var bytes = Encoding.UTF8.GetBytes(json);

            await client.SendAsync(bytes, WebSocketMessageType.Text, true, token);
        }

        public static async Task<T> ReceiveAsAsync<T>(this ClientWebSocket client, CancellationToken token) where T : new()
        {
            var endOfMessage = false;
            var data = new StringBuilder();
            var bytes = new byte[256];

            while (!endOfMessage)
            {
                var result = await client.ReceiveAsync(new ArraySegment<byte>(bytes), token);
                bytes = bytes.Take(result.Count).ToArray();
                data.Append(Encoding.UTF8.GetString(bytes));

                endOfMessage = result.EndOfMessage;
            }

            var response = data.ToString();
            var target = JsonConvert.DeserializeObject<T>(response);

            return target;
        }
    }
}
