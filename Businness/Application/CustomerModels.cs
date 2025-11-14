namespace Businness.Application
{
    public class CustomerListItem
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class CustomerDetailItem
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; }
        public List<CustomerAddressItem> Addresses { get; set; } = new();
    }

    public class CustomerAddressItem
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
        public bool IsActive { get; set; } = true;

    }

    public class CustomerCreateRequest
    {
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; } = true;
    }

    public class CustomerUpdateRequest
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class CityOrderCountModel
    {
        public string City { get; set; } = null!;
        public int OrderCount { get; set; }
    }
}
