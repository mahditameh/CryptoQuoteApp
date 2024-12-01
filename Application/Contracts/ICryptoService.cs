using Application.DTO;

namespace Application.Contracts
{
    public interface ICryptoService
    {
        Task<CryptoQuoteDto> GetCryptoQuoteAsync(string cryptoCode);
    }
}
