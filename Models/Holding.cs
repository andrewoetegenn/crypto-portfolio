namespace CryptoPortfolio
{
    public class Holding
    {
        public string Coin { get; }
        public string Symbol { get; }
        public double Quantity { get; }
        public double Price { get; set; }
        public double Value { get; set; }

        public Holding(string coin, string symbol, double quantity)
        {
            Coin = coin;
            Symbol = symbol;
            Quantity = quantity;
        }
    }
}