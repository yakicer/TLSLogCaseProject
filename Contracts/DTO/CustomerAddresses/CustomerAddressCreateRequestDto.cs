namespace Contracts.DTO.CustomerAddresses
{
    public class CustomerAddressCreateRequestDto
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
}
