using Domain.ValueObjects;

namespace Domain.Services
{
    public class CryptoDomainService
    {
        /// <summary>
        /// Calculates equivalent rates in various currencies based on the exchange rates.
        /// </summary>
        /// <param name="basePrice">The base price in USD or another reference currency.</param>
        /// <param name="exchangeRates">Dictionary of target currencies and their exchange rates.</param>
        /// <returns>A dictionary of converted prices by currency code.</returns>
        public Dictionary<string, decimal> CalculateEquivalentRates(Price basePrice, Dictionary<string, Price> exchangeRates)
        {
            var convertedRates = new Dictionary<string, decimal>();

            foreach (var rate in exchangeRates)
            {
                convertedRates[rate.Key] = basePrice.ConvertTo(rate.Value.Value);
            }

            return convertedRates;
        }
    }
}
