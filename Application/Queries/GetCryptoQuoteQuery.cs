using Application.Common;
using Application.DTO;
using MediatR;

namespace Application.Queries
{
    public record GetCryptoQuoteQuery : IRequest<ApiResult<CryptoQuoteDto>>
    {
        public string CryptoCode { get; set; }
    }

}
