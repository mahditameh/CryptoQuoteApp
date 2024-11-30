namespace Domins
{
    public interface ICryptoRepository
    {
        Task<decimal> GetCryptoPriceAsync(string cryptoCode, string symbol);
        Task<Dictionary<string, decimal>> GetExchangeRatesAsync();
    }
}
