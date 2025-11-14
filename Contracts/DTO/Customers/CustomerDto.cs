namespace Contracts.DTO.Customers
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
