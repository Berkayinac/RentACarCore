using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching;

// Cache'deki datayı belirli bir zamanda temizlememiz gerekiyor. O zamanda veri manipülasyonları gerçekleştiğinde yani Insert, Update, Delete Command'leri devreye girdiğinde Cache'deki verinin temizlenmesi gerekiyor.
public interface ICacheRemoverRequest
{
    string CacheKey { get; }
    bool BypassCache { get; }
    string? CacheGroupKey { get; }
}
