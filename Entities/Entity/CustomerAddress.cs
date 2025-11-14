using Entities.Enums;

namespace Entities.Entity
{
    public class CustomerAddress : BaseEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public AddressType AdresType { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Town { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PostalCode { get; set; }
        public Customer Customer { get; set; } = null!;
        public ICollection<Order> DeliveryOrders { get; set; } = new List<Order>();
        public ICollection<Order> InvoiceOrders { get; set; } = new List<Order>();
    }


}
