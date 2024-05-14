namespace Core.Application.Pipelines.Caching;

public class CacheSettings
{
    public int SlidingExpiration { get; set; } // Cache'de verilerin kalma süresini ayarladığımız yapı. -> Bu bilgi genellikle WebAPI'daki app settings'de tutulur.
}