using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.JWT;

// AccessToken -> Sisteme giriş için kullanılan veya kullanıcı giriş yaptığında verebileceğimiz token'dır.

public class AccessToken
{
    public string Token { get; set; } // Tokenın kendisi
    public DateTime Expiration { get; set; } // Tokenın bitiş süresi

    public AccessToken()
    {
        Token = string.Empty;
    }

    public AccessToken(string token, DateTime expiration)
    {
        Token = token;
        Expiration = expiration;
    }
}
