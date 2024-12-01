namespace Domain
{
    public interface ICryptoValidator
    {
        Task<bool> IsValidCryptoSymbolAsync(string symbol);
    }
}
