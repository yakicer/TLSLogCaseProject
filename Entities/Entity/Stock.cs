namespace Entities.Entity
{
    public class Stock : BaseEntity
    {
        public int Id { get; set; }
        public string StockName { get; set; } = null!;
        public string? Unit { get; set; }
        public decimal Price { get; set; }
        public string Barcode { get; set; } = null!;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
