namespace Entities.DTO
{
    public class CustomerAddressListDto
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

    public class CustomerAddressDetailDto
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
        public bool IsActive { get; set; }
    }

    public class CustomerAddressCreateDto
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

    public class CustomerAddressUpdateDto
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
