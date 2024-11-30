using System.Globalization;

namespace CryptoQuoteApp.Helpers
{
    public static class CurrencyHelper
    {
        public static string FormatCurrency(string currencyCode, decimal amount)
        {
            CultureInfo culture;

            switch (currencyCode)
            {
                case "EUR":
                    culture = new CultureInfo("fr-FR");
                    break;
                case "BRL":
                    culture = new CultureInfo("pt-BR");
                    break;
                case "GBP":
                    culture = new CultureInfo("en-GB");
                    break;
                case "AUD":
                    culture = new CultureInfo("en-AU");
                    break;
                default:
                    culture = CultureInfo.InvariantCulture; // Default fallback to InvariantCulture
                    break;
            }

            return string.Format(culture, "{0:C}", amount);
        }

    }
}