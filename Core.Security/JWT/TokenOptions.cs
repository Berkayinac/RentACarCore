using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.JWT;


public class TokenOptions
{
    public string Audience { get; set; } // Tokeni alan taraf
    public string Issuer { get; set; } // Tokeni veren taraf
    public int AccessTokenExpiration { get; set; } // Token sona erme süresi
    public string SecurityKey { get; set; } // Güvenlik anahtarı
    public int RefreshTokenTTL { get; set; } // RefreshToken için TTL değeri

    public TokenOptions()
    {
        Audience = string.Empty;
        Issuer = string.Empty;
        SecurityKey = string.Empty;
    }

    public TokenOptions(string audience, string issuer, int accessTokenExpiration, string securityKey, int refreshTokenTtl)
    {
        Audience = audience;
        Issuer = issuer;
        AccessTokenExpiration = accessTokenExpiration;
        SecurityKey = securityKey;
        RefreshTokenTTL = refreshTokenTtl;
    }
}
