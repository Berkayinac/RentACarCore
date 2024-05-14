using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Paging;

// gelen datayı sayfala haline getirecek extension
// IQueryable üzerinden yapmamızın sebebi -> Veritabanından tüm data gelmesin
// sadece istediğim sayfaya ait datalar gelsin'i sağlamak için
// örnek -> 0.indexe ait 10 tane ürün gelecek ya o 10 ürünü sadece getirsin diğer sayfalardaki ürünleri istek yapıldığında getirsini sağlamak ve performansı arttırmak için
public static class IQueryablePaginateExtensions
{
    public static async Task<Paginate<T>> ToPaginateAsync<T>(
        this IQueryable<T> source,
        int index,
        int size,
        CancellationToken cancellationToken = default)
    {
        // toplam veri sayısı
        int count = await source.CountAsync(cancellationToken).ConfigureAwait(false);

        // Skip -> datayı atla demek
        // Take -> Datayı al demek
        // örnek ilk index için;
        // source'dan gelen datayı 0*10 verilini vererek 0 geleceğinden dolayı atlama işlemi olmayacak ancak 1.index'deyken 0.index'teki dataları atlamasını ve sonraki SIZE boyutu kadar olan datayı Take etmesini söylüyoruz.
        List<T> items = await source.Skip(index * size).Take(size).ToListAsync(cancellationToken).ConfigureAwait(false);

        Paginate<T> list = new()
        {
            Index = index,
            Count = count,
            Items = items,
            Size = size,

            // Toplam data sayısını sayfanın alacağı max eleman sayısına yani size'a bölüp double hale getirip int değer dönmemesi için en sonda da int'e çevirerek toplam sayfa sayısına ulaştım
            Pages = (int)Math.Ceiling(count / (double)size)
        };

        return list;
    }

    public static Paginate<T> ToPaginate<T>(this IQueryable<T> source, int index, int size)
    {
        int count = source.Count();
        var items = source.Skip(index * size).Take(size).ToList();

        Paginate<T> list =
            new()
            {
                Index = index,
                Size = size,
                Count = count,
                Items = items,
                Pages = (int)Math.Ceiling(count / (double)size)
            };
        return list;
    }

}
