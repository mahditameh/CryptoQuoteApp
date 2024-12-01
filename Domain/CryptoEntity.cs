using Domain.ValueObjects;

namespace Domain
{
    public class CryptoEntity
    {
        public string Symbol { get; private set; }
        public Price Price { get; private set; }

        public CryptoEntity(string symbol, Price price)
        {
            if (string.IsNullOrWhiteSpace(symbol))
                throw new ArgumentException("Symbol cannot be empty");

            Symbol = symbol.ToUpper();
            Price = price;
        }

        public decimal ConvertTo(Price targetRate)
        {
            return Price.Value * targetRate.Value;
        }
    }
}
