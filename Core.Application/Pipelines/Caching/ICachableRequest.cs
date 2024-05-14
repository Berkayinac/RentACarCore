using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching;

// Cache -> Bir Query için request geldiğinde tüm veriler veritabanından gelerek kullanıcıya gösterilir ve veriler Cachelenir.
// Bir başka kullanıcı da aynı istekte bulunduğunda veriler hali hazırda cache içerisinde bulunduğu için tekrardan veritabanından veriler gelmez.
// Cache'den direkt olarak kullanıcıya gönderilir. Ancak sistemde yeni bir command request'i yapıldığında cache'deki veriler remove edilerek kaldırlır ve bir sonraki istek yapıldığında veriler tekrardan veritabanından getirilerek tekrardan cachelenir.

// Burası Cache'leme yapılması için kurulan sistemdir. QUERY'ler için bu request kullanılacaktır. Command'e ait cache remove farklı bir class'ta yapılacak.
public interface ICachableRequest
{
    // CacheKey vasıtasıyla bir cache anahtarı oluşturucaz ve her request'i bir cache anahtarına bağlayıp o request ile cache oluşturmuş olucaz.
    string CacheKey { get; }

    // Cache'i bypass ederek, development çalışmaları ve test işlemleri için cache'i istediğim zaman bypass ediyorum.
    bool BypassCache { get; }

    // Cache'lerimizi gruplandıracağımız anahtar. örnek olarak GetBrands markaları çekme ile ilgili yapı.
    string? CacheGroupKey { get; }

    // İlgili cache ne kadar süre sistemde duracak. 
    TimeSpan? SlidingExpiration { get; }
}
