
using Entities.Enums;

namespace Entities.Entity
{
    public class Order : BaseEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNo { get; set; } = null!;
        public decimal TotalPrice { get; set; }
        public decimal Tax { get; set; }
        public OrderStatus Status { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int? InvoiceAddressId { get; set; }
        public Customer Customer { get; set; } = null!;
        public CustomerAddress? DeliveryAddress { get; set; }
        public CustomerAddress? InvoiceAddress { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
