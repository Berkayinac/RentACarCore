using Core.Persistence.Dynamic;
using Core.Persistence.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Repositories;

// IQuery -> Raporlama işlemleri için kullanılıyor linq ile yazmak yerine direkt olarak SQL Query'si ile yapıyoruz.
public interface IAsyncRepository<TEntity, TEntityId> : IQuery<TEntity>
    where TEntity : Entity<TEntityId>
{
    Task<TEntity?> GetASync(
        Expression<Func<TEntity, bool>> predicate, // lambda ile gösterme yöntemi -> p=>p.Id == id  veya p=>p.Name == name  gibi yapıları almamızı sağlar
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,  // Bir başka tablo ile join etmemizi sağlayan include yapı. -> IIncludableQueryable => EF.Core.Query'den gelen bir yapı.
        bool withDeleted = false, // --> silinenleri getirme --> eğer ki silinen verileri yani pasif verileri de getirmesini istiyorsan bunu true yap.
        bool enableTracking = true, // -> EF'nin Tracking(izleme) desteğinin enable edilip edilmeyeceğini gösteren bir yapıdır.
        CancellationToken cancellationToken = default); // Async metotlar için iptal edilmesini sağlayan gerekli değer.

    // Paginate -> Sayfalı dönmesini sağlayan bir class
    Task<Paginate<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, // Order By yapabilmeyi sağlıyor
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0, // kaçıncı sayfa => 0. yani ilk sayfa
        int size = 10, // her sayfada kaçar tane bu nesneden olsun => 10000000 veri diyip tüm verileride tek sayfada gösterebilirsin.
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default);

    // DynamicQuery -> Kullanıcı birden fazla olan filtreleme sorgusunda örnek olarak araba parçaları diyelim
    // vites tipi, motoru, yakıt tipi vs vs  gibi yapıda sadece marka ve vites tipini seçiyor kullanıcı ve sorguyu bu kıstaslara bağlı kalarak atmak zorunda oluyoruz. bunu sağlayan yapı dynamic query yapısı
    // DynamicQuery -> Dinamik bir şekilde listeleme, sorgulama, search işlemi yapmamızı sağlayan yapı
    Task<Paginate<TEntity>> GetListByDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );

    // Bizim aradığımız veri var mı yok mu => örnek olarak böyle bir TcNo var mı yok mu
    Task<bool> AnyAsync(
       Expression<Func<TEntity, bool>>? predicate = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default);


    // Tekli eleman ekleme
    Task<TEntity> AddAsync(TEntity entity);

    // Çoklu eleman ekleme
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities);

    // Tekli eleman güncelleme
    Task<TEntity> UpdateAsync(TEntity entity);

    // Çoklu eleman güncelleme
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities);


    // permanent => soft delete mi yoksa kalıcı mı sileyim kısmını belirtmek için kullanıyoruz.
    // Yani bir eleman silindiğinde onu direkt sistemden uçurmak yerine soft delete yaparak sadece inactive olmasını sağlayabiliriz. Bunun için permanent => false olmalıdır.
    // // Eğer ki bir eleman silindiğinde onu direkt sistemden uçurmak istiyorsan permanent = true yap.

    // Tekli eleman silme 
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false);

    // Çoklu eleman silme
    Task<ICollection<TEntity>> DeleteRangeAsync(ICollection<TEntity> entities, bool permanent = false);
}