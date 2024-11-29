using Applications.DTO;

namespace Applications.Contracts
{
    public interface ICryptoService
    {
        Task<CryptoQuoteDto> GetCryptoQuoteAsync(string cryptoCode);
    }
}
