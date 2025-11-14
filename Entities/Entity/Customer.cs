namespace Entities.Entity
{
    public class Customer : BaseEntity
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public ICollection<CustomerAddress> Addresses { get; set; } = new List<CustomerAddress>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
