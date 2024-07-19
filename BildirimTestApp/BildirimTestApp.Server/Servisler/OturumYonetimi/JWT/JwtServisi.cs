using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BildirimTestApp.Server.Models;
using BildirimTestApp.Server.Servisler.OturumYonetimi.JWT.Token;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;

namespace BildirimTestApp.Server.Servisler.OturumYonetimi.JWT
{
    public class JwtServisi : IJwtServisi
    {
        private readonly IConfiguration _configuration;

        public JwtServisi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public JwtToken JwtTokenOlustur(SisKullanici kullanici)
        {
            var jwtToken = new JwtToken();
            var tokenhandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey key =
                new(Encoding.UTF8.GetBytes(_configuration["JWT:Key"] ?? string.Empty));

            jwtToken.AccessTokenTime = DateTime.UtcNow.AddHours(1);

            var claims = new List<Claim>
            {
                new("kullaniciAdi", kullanici.KullaniciAdi.ToString())
            };
            var tokendesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _configuration["Jwt:Audience"],
                Issuer = _configuration["Jwt:Issuer"],
                Expires = jwtToken.AccessTokenTime,
                SigningCredentials = new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenhandler.CreateToken(tokendesc);
            var finaltoken = tokenhandler.WriteToken(token);
            return new JwtToken()
            {
                AccessToken = finaltoken,
                AccessTokenTime = jwtToken.AccessTokenTime
            };
        }
    }
}
