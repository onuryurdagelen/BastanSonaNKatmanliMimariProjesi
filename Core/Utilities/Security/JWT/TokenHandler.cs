using Core.Extensions;
using Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Core.Utilities.Security.JWT
{
    public class TokenHandler : ITokenHandler
    {
        private IConfiguration Configuration { get; }
        private readonly TokenOptions _tokenOptions;

        public TokenHandler(IConfiguration configuration)
        {
            Configuration = configuration;
            _tokenOptions = Configuration.GetSection("Token").Get<TokenOptions>();
        }


        public Token CreateToken(User user, List<OperationClaim> operationClaims)
        {
            Token token = new Token();
            //Security Key'in simetriği alınır.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));

            //Şifrelenmiş kimliği oluşturulur.
            //SigninCredentials class'ı securityKey ve oluşturacağımız security algoritması parametreleri alır.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //Token ayarları yapılır.
            token.ExpirationTime = DateTime.Now.AddMinutes(60); //Token'ın bitiş süresini 1 dk olarak ayarladık.

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer:_tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration),
                claims: SetClaims(user,operationClaims),
                notBefore: DateTime.Now, //"Token üretildikten hemen sonra devreye girsin" demektir.
                signingCredentials: signingCredentials
                );
            //Token oluşturucu sınıfından bir örnek alınır.
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            //Token üretilir.
            token.AccessToken = jwtSecurityTokenHandler.WriteToken(securityToken);

            //Refresh token üretilir.
            token.RefreshToken = CreateRefreshToken();

            return token;

        }
        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }
        private IEnumerable<Claim> SetClaims(User user,List<OperationClaim> operationClaims)
        {
            var claims = new List<Claim>();
            claims.AddName(user.Name);
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddRoles(operationClaims.Select(oc => oc.Name).ToArray());

            return claims;

        }
    }
}
