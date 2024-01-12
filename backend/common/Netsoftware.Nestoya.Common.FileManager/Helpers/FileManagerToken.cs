using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Netsoftware.Nestoya.Common.FileManager.Helpers
{
    internal class FileManagerToken
    {
        public string Token { get; }

        private const int TOKEN_EXPIRATION_TIME = 2;
        private readonly IConfiguration _configuration;
        public FileManagerToken(IConfiguration configuration)
        {
            _configuration = configuration;
            Token = "Bearer " + GetToken();         
        }

        public string GetToken()
        {
            try
            {
                JwtSecurityToken token = BuildToken(CreateSecurityKey(_configuration.GetSection("FileStorageJwtKey").Value));
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SymmetricSecurityKey CreateSecurityKey(string jwtKey)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtKey));
        }

        private JwtSecurityToken BuildToken(SecurityKey securityKey)
        {
            var token = new JwtSecurityToken(
                              expires: DateTime.UtcNow.AddMinutes(TOKEN_EXPIRATION_TIME),
                              signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));

            return token;
        }
    }
}
