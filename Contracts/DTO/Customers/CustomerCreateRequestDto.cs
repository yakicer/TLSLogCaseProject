namespace Contracts.DTO.Customers
{
    public class CustomerCreateRequestDto
    {
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }
}
