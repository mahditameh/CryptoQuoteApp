using System.Text.Json;

namespace CryptoTest.Stubs
{
    public class CryptoResponseBuilder
    {
        private readonly Dictionary<string, object> _data;

        public CryptoResponseBuilder()
        {
            _data = new Dictionary<string, object>();
        }

        public CryptoResponseBuilder AddCryptoPrice(string cryptoCode, decimal price)
        {
            _data[cryptoCode] = new
            {
                quote = new
                {
                    USD = new
                    {
                        price = price
                    }
                }
            };
            return this;
        }

        public string Build()
        {
            var response = new
            {
                data = _data
            };

            return JsonSerializer.Serialize(response);
        }
    }


}
