using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Entities;

// OTP -> One Time Password -> Two Factor Authentication için kullanılır.
// Google authenticator gibi yapıları kullanarak Two Factor Authentication'ı destekleyecek altyapıyı sağlar.
public class OtpAuthenticator : Entity<int>
{
    public int UserId { get; set; } // hangi kullanıcı için
    public byte[] SecretKey { get; set; } // Çözümleyecek olan anahtar 
    public bool IsVerified { get; set; } // onaylanma bilgisi

    public virtual User User { get; set; } = null!;

    public OtpAuthenticator()
    {
        SecretKey = Array.Empty<byte>();
    }

    public OtpAuthenticator(int userId, byte[] secretKey, bool isVerified)
    {
        UserId = userId;
        SecretKey = secretKey;
        IsVerified = isVerified;
    }

    public OtpAuthenticator(int id, int userId, byte[] secretKey, bool isVerified)
        : base(id)
    {
        UserId = userId;
        SecretKey = secretKey;
        IsVerified = isVerified;
    }
}
