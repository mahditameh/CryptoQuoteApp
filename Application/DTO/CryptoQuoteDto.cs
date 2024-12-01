namespace Application.DTO
{
    public class CryptoQuoteDto
    {
        public string Symbol { get; set; }
        public decimal Price { get; set; }
        public Dictionary<string, decimal> ConvertedPrices { get; set; }
    }
}
