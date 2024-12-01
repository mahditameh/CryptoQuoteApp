namespace Domain.ValueObjects
{
    public class Price
    {
        public decimal Value { get; private set; }
        public string Currency { get; private set; }

        public Price(decimal value, string currency)
        {
            if (value <= 0)
                throw new ArgumentException("Price value must be greater than zero");
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency cannot be empty");

            Value = value;
            Currency = currency.ToUpper();
        }

        /// <summary>
        /// Converts this price to another currency based on the given exchange rate.
        /// </summary>
        /// <param name="exchangeRate">The exchange rate for the target currency.</param>
        /// <returns>The converted price value.</returns>
        public decimal ConvertTo(decimal exchangeRate)
        {
            if (exchangeRate <= 0) throw new ArgumentException("Exchange rate must be greater than zero.");
            return Value * exchangeRate;
        }
    }
}
