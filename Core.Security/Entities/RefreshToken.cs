using Core.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Entities;

// AccessToken'ın süresi dolmuşsa veya yeniden alınması gerekiyorsa burada kullanılan anahtar RefleshToken'dır.
// AccessToken -> JWT yapısında kullanıcının sisteme girebilmesini sağlayan ve sistem içerisinde bir işlem yapabilmesini sağlayan tokendır., BU token'ların bir süresi var, süre dolduğunda kullanıcı tekrar sisteme giriş yapma ihtiyacı duymadan reflesh token kullanarak süreç hızlanır.
public class RefreshToken : Entity<int>
{
    public int UserId { get; set; }
    public string Token { get; set; } // refresh tokenın kendisi
    public DateTime Expires { get; set; } // ne zaman sonlanacak bu refresh token.
    public string CreatedByIp { get; set; } // Oluşturan IP.
    public DateTime? Revoked { get; set; } // İptal etme işlemi. Güvenlik ihlali durumlarında kullanıcıya ait tokenı iptal etmek amacıyla kullanırız, Başka bir örnek kullanıcının süresi dolmuş olabilir isteyebilir.
    public string? RevokedByIp { get; set; } // İptal eden IP
    public string? ReplacedByToken { get; set; } // hangi tokenın yerine geçti bu token
    public string? ReasonRevoked { get; set; } // İptal sebebi nedir

    public virtual User User { get; set; } = null!;

    public RefreshToken()
    {
        Token = string.Empty;
        CreatedByIp = string.Empty;
    }

    public RefreshToken(int userId, string token, DateTime expires, string createdByIp)
    {
        UserId = userId;
        Token = token;
        Expires = expires;
        CreatedByIp = createdByIp;
    }

    public RefreshToken(int id, int userId, string token, DateTime expires, string createdByIp)
        : base(id)
    {
        UserId = userId;
        Token = token;
        Expires = expires;
        CreatedByIp = createdByIp;
    }
}
