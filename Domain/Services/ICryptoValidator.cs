using Domain.Services.Models;

namespace Domain.Services
{
    public interface ICryptoValidator
    {
        Task<bool> IsValidCryptoSymbolAsync(string symbol);
        Task<List<CryptoSymbol>> GetCryptoMapAsync();
    }
}
