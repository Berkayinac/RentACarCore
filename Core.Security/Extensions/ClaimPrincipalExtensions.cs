using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Extensions;

//  ClaimsPrincipal -> O an login olmuş token'ı göndermiş olan nesne
// O anki kullanıcının kendisine ait bilgileri aldığımız yer.
// onun claim'lerini bir liste haline almış olucaz.

public static class ClaimsPrincipalExtensions
{
    // İlgili kullanıcıya ait tüm claim'leri aldığımız metot
    public static List<string>? Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
    {
        var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
        return result;
    }

    // İlgili kullanıcının rollerini aldığımız metot
    public static List<string>? ClaimRoles(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal?.Claims(ClaimTypes.Role);

    // O kullanıcıya ait UserId'yi aldığımız metot
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal) =>
        Convert.ToInt32(claimsPrincipal?.Claims(ClaimTypes.NameIdentifier)?.FirstOrDefault());
}