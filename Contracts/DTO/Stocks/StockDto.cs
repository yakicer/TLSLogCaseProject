namespace Contracts.DTO.Stocks
{
    public class StockDto
    {
        public int Id { get; set; }
        public string StockName { get; set; } = null!;
        public string? Unit { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
