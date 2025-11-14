namespace Web.Blazor.Services.Interface
{
    public interface ITokenService
    {
        Task<string?> GetAccessTokenAsync();
        Task SetAccessTokenAsync(string token);
        Task RemoveAccessTokenAsync();
    }
}
