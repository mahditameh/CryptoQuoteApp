namespace Domain.ValueObjects
{
    namespace Domain.ValueObjects
    {
        public class Currency
        {
            public string Code { get; private set; }
            public string Symbol { get; private set; }
            public decimal ExchangeRate { get; private set; }

            public Currency(string code, decimal exchangeRate)
            {
                if (string.IsNullOrWhiteSpace(code))
                    throw new ArgumentException("Currency code cannot be empty.", nameof(code));

                if (exchangeRate <= 0)
                    throw new ArgumentException("Exchange rate must be positive.", nameof(exchangeRate));

                Code = code.ToUpper();
                Symbol = GetSymbolForCurrency(Code); // Set symbol during initialization
                ExchangeRate = exchangeRate;
            }

            private static string GetSymbolForCurrency(string code)
            {
                return code switch
                {
                    "USD" => "$",
                    "EUR" => "€",
                    "GBP" => "£",
                    "BTC" => "₿",
                    _ => code // Fallback to the code itself
                };
            }
        }
    }

}

