namespace CryptoTest.Stubs
{
    public class HttpMessageHandlerStub : HttpMessageHandler
    {
        private readonly HttpResponseMessage _response;

        public HttpMessageHandlerStub(HttpResponseMessage response)
        {
            _response = response;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_response);
        }
    }
}
