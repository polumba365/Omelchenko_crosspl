using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Omelchenko_crosspl.Models
{
    public static class AuthOptions
    {
        public const string Issuer = "OmelchenkoApi";
        public const string Audience = "OmelchenkoClient";
        // Длинный ключ (минимум 32 символа)
        public const string Key = "SuperSecretKeyForDiplomaProject2024ArcticOil!";

        public static object GenerateToken(bool isAdmin)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, isAdmin ? "admin" : "user"),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, isAdmin ? "admin" : "user") // Важно для [Authorize(Roles="admin")]
            };

            var keyBytes = Encoding.ASCII.GetBytes(Key);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // <-- ТРЕБОВАНИЕ: 1 ЧАС
                signingCredentials: credentials);

            return new { token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}