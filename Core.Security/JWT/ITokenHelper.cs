using Core.Security.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.JWT;

// JWT Token yani AccessToken'ı üretecek olan token
// RefreshToken olacak şekilde 2 adet tokenımız var. -> Bu token'ları veritabanında tutuyor olucaz. Ancak Access Token'ları Microsoft.Identity ile yönetileceğinden dolayı veritabanına eklenemez.


public interface ITokenHelper
{
    // Kullanıcının kendisi ve ona ait operation claim'leri neler bunlar lazım ki JWT tarafında token üretebilelim.
    AccessToken CreateToken(User user, IList<OperationClaim> operationClaims);

    RefreshToken CreateRefreshToken(User user, string ipAddress);
}
