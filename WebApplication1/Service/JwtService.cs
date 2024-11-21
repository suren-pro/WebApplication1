using App.Business.Dto;
using App.Data.Models;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Service
{
    public interface IJwtService
    {
        string GenerateJwtToken(UserDto userDto);
        void GetUserClaimsByToken(string token);
    }
    public class JwtService:IJwtService
    {
        private readonly IConfiguration configuration;
        public JwtService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        private ClaimsIdentity CreateClaimIdentity(UserDto userDto) {
            var ci = new ClaimsIdentity();
            ci.AddClaim(new Claim("id", userDto.UserId.ToString()));
            return ci;
        }
        private SigningCredentials GenereateCredentials()
        {
            var privateKey = Encoding.UTF8.GetBytes(configuration["PrivateKey"]);
            return new SigningCredentials(
                        new SymmetricSecurityKey(privateKey),
                        SecurityAlgorithms.HmacSha256);
        }
        public string GenerateJwtToken(UserDto userDto)
        {

            var handler = new JwtSecurityTokenHandler();
           
            ClaimsIdentity claimsIdentity = CreateClaimIdentity(userDto);
            SigningCredentials credentials = GenereateCredentials();
            //Token Descriptor 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                SigningCredentials = credentials,
                Expires = DateTime.UtcNow.AddHours(2),
                Subject = claimsIdentity
            };
            var token = handler.CreateJwtSecurityToken(tokenDescriptor);
            return handler.WriteToken(token);
        }
        public void GetUserClaimsByToken(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);
            var claims = token.Claims;
        }
    }
}
