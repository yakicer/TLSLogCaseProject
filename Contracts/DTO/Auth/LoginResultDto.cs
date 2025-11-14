namespace Contracts.DTO.Auth
{
    public class LoginResultDto
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string? RefreshToken { get; set; }
    }
}
