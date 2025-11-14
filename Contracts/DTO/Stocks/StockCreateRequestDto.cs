namespace Contracts.DTO.Stocks
{
    public class StockCreateDto
    {
        public string StockName { get; set; } = null!;
        public string? Unit { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; } = null!;
    }
}
