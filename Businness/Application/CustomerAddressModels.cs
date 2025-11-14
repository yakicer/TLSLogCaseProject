namespace Businness.Application
{
    public class CustomerAddressListItem
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int AdresType { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Town { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomerAddressDetailItem
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public int AdresType { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Town { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PostalCode { get; set; }
        public bool IsActive { get; set; }
    }

    public class CustomerAddressCreateRequest
    {
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

    public class CustomerAddressUpdateRequest
    {
        public int Id { get; set; }
        public int AdresType { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Town { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PostalCode { get; set; }
        public bool IsActive { get; set; }
    }

}
