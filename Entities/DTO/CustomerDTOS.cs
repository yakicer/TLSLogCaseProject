namespace Entities.DTO
{
    public class CustomerListDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class CustomerDetailDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; }
        public List<CustomerAddressDto> Addresses { get; set; } = new();
    }

    public class CustomerAddressDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AdresType { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Town { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PostalCode { get; set; }
    }

    public class CustomerCreateDto
    {
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }

    public class CustomerUpdateDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class CityOrderCountDto
    {
        public string City { get; set; } = null!;
        public int OrderCount { get; set; }
    }
}
