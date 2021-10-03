using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CryptoPortfolio.Finnhub;
using Spectre.Console;

namespace CryptoPortfolio
{
    public class Program
    {
        private static readonly Dictionary<string, Holding> _holdings = AppConfiguration.Portfolio.Holdings;

        public static async Task Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-GB");

            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, _) => cts.Cancel();

            var finnhub = new FinnhubClient();
            var finnhubWS = new FinnhubWebSocketClient();

            await finnhubWS.Connect(cts.Token);

            await AnsiConsole
                .Status()
                .StartAsync("Loading portfolio...", async context =>
                {
                    context.Status($"Loading portfolio...");

                    foreach (var (symbol, holding) in _holdings)
                    {
                        var quote = await finnhub.GetQuote(symbol, cts.Token);
                        holding.Price = quote.CurrentPrice;
                        holding.Value = quote.CurrentPrice * holding.Quantity;

                        await finnhubWS.Subscribe(symbol, cts.Token);
                    }
                });

            await AnsiConsole
                .Live(Text.Empty)
                .StartAsync(async context =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        var trades = await finnhubWS.Receive(cts.Token);

                        foreach (var data in trades.Data)
                        {
                            var holding = _holdings[data.Symbol];
                            holding.Price = data.CurrentPrice;
                            holding.Value = data.CurrentPrice * holding.Quantity;
                        }

                        var table = new Table()
                            .Centered()
                            .Expand()
                            .Title("My Crypto Portfolio", new Style(Color.Blue))
                            .Border(TableBorder.Simple)
                            .AddColumn("Coin")
                            .AddColumn("Symbol")
                            .AddColumn("Quantity")
                            .AddColumn("Price (per coin)")
                            .AddColumn("Value");

                        table.Columns[2].RightAligned();
                        table.Columns[3].RightAligned();
                        table.Columns[4].RightAligned();

                        foreach (var (symbol, holding) in _holdings)
                        {
                            table.AddRow(
                                holding.Coin,
                                holding.Symbol,
                                $"{holding.Quantity}",
                                $"{holding.Price:C}",
                                $"{holding.Value:C}"
                            );
                        }

                        table.Columns[4].Footer($"{_holdings.Values.Sum(x => x.Value):C}");

                        context.UpdateTarget(table);
                    }
                });
        }
    }
}
