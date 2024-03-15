using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingApp.Server.Utils
{
    public class GetUserByJwt
    {
        private readonly IConfiguration _config;
        public GetUserByJwt(IConfiguration configuration)
        {
            _config = configuration;
        }
        public IEnumerable<Claim>? GetClaims(string? tokenFromHeader) 
        {
            string[] tokenSplit = tokenFromHeader!.Split(' ');
            string token = tokenSplit[1];
            var tokenHandler = new JwtSecurityTokenHandler();
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
            (_config["Jwt:Key"]!);
            SecurityToken validatedToken;
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var claims = ((JwtSecurityToken)validatedToken).Claims;

                return claims;
            }
            catch (SecurityTokenException)
            {
                return null;
            }
        }

        public DateTime? GetExpirationDateFromJwtToken(string jwtToken)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.CanReadToken(jwtToken))
            {
                JwtSecurityToken jwtSecurityToken = tokenHandler.ReadJwtToken(jwtToken);

                if (jwtSecurityToken.Payload.ContainsKey(JwtRegisteredClaimNames.Exp))
                {
                    var expirationUnix = long.Parse(jwtSecurityToken.Payload[JwtRegisteredClaimNames.Exp].ToString());
                    var expirationDateTime = DateTimeOffset.FromUnixTimeSeconds(expirationUnix).LocalDateTime;

                    return expirationDateTime;
                }
            }

            return null;
        }

    }
}
