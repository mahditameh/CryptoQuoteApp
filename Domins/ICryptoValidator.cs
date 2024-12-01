namespace Domins
{
    public interface ICryptoValidator
    {
        Task<bool> IsValidCryptoSymbolAsync(string symbol);
    }
}
