namespace Entities.Entity
{
    public class OrderDetail : BaseEntity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int StockId { get; set; }
        public decimal Amount { get; set; }
        public Order Order { get; set; } = null!;
        public Stock Stock { get; set; } = null!;
    }
}
