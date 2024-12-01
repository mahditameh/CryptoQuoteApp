using Application.Common;
using Application.Queries;
using MediatR;

namespace Application
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TResponse : ApiResult<object>
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is GetCryptoQuoteQuery query && string.IsNullOrEmpty(query.CryptoCode))
            {
                return ApiResult<object>.Failure("Crypto code cannot be empty.") as TResponse;
            }

            return await next();
        }
    }

}
