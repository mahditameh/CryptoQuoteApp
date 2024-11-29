namespace Domins
{
    public interface ICryptoRepository
    {
        Task<decimal> GetCryptoPriceAsync(string cryptoCode);
        Task<Dictionary<string, decimal>> GetExchangeRatesAsync(decimal priceInUSD);
    }
}
