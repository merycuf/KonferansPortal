using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace KonferansPortal
{

    public class JwtTokenHelper
    {
        public string GenerateToken(Models.Uye user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kEy231!+zQjrtS uzun bi sey lazimmis"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(ClaimTypes.Role, user.Discriminator)
            };

            var token = new JwtSecurityToken(
                issuer: "https://localhost:7128/", // Replace with your issuer
                audience: "https://localhost:7128/", // Replace with your audience
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
