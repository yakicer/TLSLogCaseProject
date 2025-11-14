namespace Businness.Interface.Base
{
    public interface ITokenService
    {
        string GetToken();
        void SetToken(string token);
        void ClearToken();
    }
}
