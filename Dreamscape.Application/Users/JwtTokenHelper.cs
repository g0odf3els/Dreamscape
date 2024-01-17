using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Dreamscape.Application.Users
{
    public static class JwtTokenHelper
    {
        public static string GenerateJwtToken(IEnumerable<Claim> claims, IConfiguration config)
        {
            var jwt = new JwtSecurityToken(
                issuer: config["AuthOptions:Issuer"],
                audience: config["AuthOptions:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(2)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AuthOptions:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static string GenerateRefreshToken(IEnumerable<Claim> claims, IConfiguration config)
        {
            var jwt = new JwtSecurityToken(
                issuer: config["AuthOptions:Issuer"],
                audience: config["AuthOptions:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromDays(7)),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["AuthOptions:Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
