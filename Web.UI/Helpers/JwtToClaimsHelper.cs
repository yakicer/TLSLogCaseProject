using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Web.UI.Helpers
{
    public static class JwtToClaimsHelper
    {
        public static ClaimsPrincipal CreatePrincipalFromJwt(string jwt)
        {
            var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

            var claims = new List<Claim>();

            var name = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                       ?? token.Claims.FirstOrDefault(c => c.Type == "name")?.Value
                       ?? token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value
                       ?? "User";

            claims.Add(new Claim(ClaimTypes.Name, name));

            var email = token.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value
                        ?? token.Claims.FirstOrDefault(c => c.Type == "email")?.Value;
            if (!string.IsNullOrEmpty(email))
                claims.Add(new Claim(ClaimTypes.Email, email));

            var surName = token.Claims.FirstOrDefault(c => c.Type == "SurName")?.Value;
            if (!string.IsNullOrEmpty(surName))
                claims.Add(new Claim("SurName", surName));

            foreach (var r in token.Claims.Where(c => c.Type == ClaimTypes.Role || c.Type == "role"))
                claims.Add(new Claim(ClaimTypes.Role, r.Value));

            // (İstersen sub -> NameIdentifier)
            var sub = token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!string.IsNullOrEmpty(sub))
                claims.Add(new Claim(ClaimTypes.NameIdentifier, sub));

            var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            return new ClaimsPrincipal(id);
        }
    }
}
