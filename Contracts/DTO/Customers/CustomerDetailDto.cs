using Contracts.DTO.CustomerAddresses;

namespace Contracts.DTO.Customers
{
    public class CustomerDetailDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; }
        public List<CustomerAddressDto> Addresses { get; set; } = new();
    }
}
