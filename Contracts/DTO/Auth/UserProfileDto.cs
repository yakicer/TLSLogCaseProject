namespace Contracts.DTO.Auth
{
    public sealed class UserProfileDto
    {
        public int Id { get; set; }
        public string DisplayName { get; set; } = default!;
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
