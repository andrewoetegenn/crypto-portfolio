using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace CryptoPortfolio
{
    public static class AppConfiguration
    {
        public static readonly IConfiguration Configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets(typeof(AppConfiguration).Assembly, true)
            .Build();

        public static class Finnhub
        {
            public static string ApiUri => AppConfiguration.Configuration["finnhub:apiUri"];
            public static string WebSocketUri => AppConfiguration.Configuration["finnhub:webSocketUri"];
            public static string ApiKey => AppConfiguration.Configuration["finnhub:apiKey"];
        }

        public static class Portfolio
        {
            public static Dictionary<string, Holding> Holdings
            {
                get
                {
                    var holdings = AppConfiguration.Configuration
                        .GetSection("portfolio")
                        .GetChildren()
                        .Select(x => new Holding(x["coin"], x["symbol"], x.GetValue<double>("quantity")))
                        .ToDictionary(x => x.Symbol);

                    return holdings;
                }
            }
        }
    }
}