using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Caching;

public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ICachableRequest
{
    private readonly CacheSettings _cacheSettings;

    // distributed cache mimarisi .net core'un altyapısından gelecektir. tüm cache yapımızı buna dayatıcaz
    // farklı cache ortamlarını inMemory veya Redis gibi tek bir configrasyon ile istediğimiz zaman geçiş yapabilmeyi sağlıyoruz.
    private readonly IDistributedCache _cache;

    // Projelerde kontrol etmek için logger ekleyebilirsin. loglamayı Console'a yapar .
    //private readonly ILogger<CachingBehavior<TRequest, TResponse>> _logger;

    public CachingBehavior(IDistributedCache cache, IConfiguration configuration //, ILogger<CachingBehavior<TRequest, TResponse>> logger
    )
    {
        _cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? throw new InvalidOperationException();
        _cache = cache;
        //_logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // eğerki bypass açık ise cache işlemini yapma.
        if (request.BypassCache)
        {
            return await next();
        }

        TResponse response;

        // cache'ler byte array şeklinde tutulur.
        byte[]? cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);

        // eğer cache'de byte array olarak tutulmuş veriyi al Deserialize ederek json'a çevir 
        if (cachedResponse != null)
        {
            response = JsonSerializer.Deserialize<TResponse>(Encoding.Default.GetString(cachedResponse));
            //_logger.LogInformation($"Fetched from Cache -> {request.CacheKey}"); -> Cache'den çekilme logu
        }
        else
        {
            response = await getResponseAndToCache(request, next, cancellationToken);
        }

        return response;
    }

    private async Task<TResponse?> getResponseAndToCache(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // veritabanına giderek metodu çalıştır veriyi bana getir
        TResponse response = await next();

        // request.SlidingExpiration varsa onu set et yoksa(??) CacheSettings'de expiration ne olarak ayarlanmışsa onu set et.
        TimeSpan slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromDays(_cacheSettings.SlidingExpiration);

        // Kaydedilecek olan cache'i ayarlamak için bu nesneyi kullandık.
        DistributedCacheEntryOptions cacheOptions = new() { SlidingExpiration = slidingExpiration };

        // response datayı byte array haline getirerek cache ediyoruz.
        byte[] serializeData = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(response));

        // tüm işlemler bittikten sonra IDistributedCache olan cache'i set et.
        await _cache.SetAsync(request.CacheKey, serializeData, cacheOptions, cancellationToken);

        //_logger.LogInformation($"Add to Cache -> {request.CacheKey}");  -> cache'e eklenme logger'ı

        if (request.CacheGroupKey != null)
            await addCacheKeyToGroup(request, slidingExpiration, cancellationToken);


        return response;
    }

    // Cachelenen datayı CacheGroupKey ile birleştirdiğimizde ilgili class'a ait nesne Insert, Update, Delete edildiğinde GroupKey kullanılarak tüm cache'lere ulaşmamızı sağlar.
    // Cachelenen data bu yüzden ilgili CacheGroupKey'e eklenmeli ki biz cacheKeys[] listesi şeklinde tutabilelim.
    // Bu metot bu işi yapıyor özetle, örnek olarak GetBrands'e ait CacheGroupKey'i alıyor ve tüm bunun alakalı cache'leri kendi listesine ekliyor. Bu metodu kullanarak onlarla işlem yapabilmemizi sağlıyıor.
    private async Task addCacheKeyToGroup(TRequest request, TimeSpan slidingExpiration, CancellationToken cancellationToken)
    {
        byte[]? cacheGroupCache = await _cache.GetAsync(key: request.CacheGroupKey!, cancellationToken);

        HashSet<string> cacheKeysInGroup;

        if (cacheGroupCache != null)
        {
            cacheKeysInGroup = JsonSerializer.Deserialize<HashSet<string>>(Encoding.Default.GetString(cacheGroupCache))!;
            if (!cacheKeysInGroup.Contains(request.CacheKey))
                cacheKeysInGroup.Add(request.CacheKey);
        }

        else
            cacheKeysInGroup = new HashSet<string>(new[] { request.CacheKey });

        byte[] newCacheGroupCache = JsonSerializer.SerializeToUtf8Bytes(cacheKeysInGroup);

        byte[]? cacheGroupCacheSlidingExpirationCache = await _cache.GetAsync(
            key: $"{request.CacheGroupKey}SlidingExpiration",
            cancellationToken
        );

        int? cacheGroupCacheSlidingExpirationValue = null;
        if (cacheGroupCacheSlidingExpirationCache != null)
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(Encoding.Default.GetString(cacheGroupCacheSlidingExpirationCache));

        if (cacheGroupCacheSlidingExpirationValue == null || slidingExpiration.TotalSeconds > cacheGroupCacheSlidingExpirationValue)
            cacheGroupCacheSlidingExpirationValue = Convert.ToInt32(slidingExpiration.TotalSeconds);

        byte[] serializeCachedGroupSlidingExpirationData = JsonSerializer.SerializeToUtf8Bytes(cacheGroupCacheSlidingExpirationValue);

        DistributedCacheEntryOptions cacheOptions =
            new() { SlidingExpiration = TimeSpan.FromSeconds(Convert.ToDouble(cacheGroupCacheSlidingExpirationValue)) };

        await _cache.SetAsync(key: request.CacheGroupKey!, newCacheGroupCache, cacheOptions, cancellationToken);
        //_logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}");

        await _cache.SetAsync(
            key: $"{request.CacheGroupKey}SlidingExpiration",
            serializeCachedGroupSlidingExpirationData,
            cacheOptions,
            cancellationToken
        );
        //_logger.LogInformation($"Added to Cache -> {request.CacheGroupKey}SlidingExpiration");
    }
}