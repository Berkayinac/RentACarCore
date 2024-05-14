using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Paging;

// frontend geliştirici için sayfalama işlemi yapıyoruz.
// hangi index'te olduğumuzu yani kaçıncı sayfada olduğumuzu
// bir sayfada kaç data var, Toplamda kaç data var
// elemanlarımız neler <TEntity>
// Bilgilerini veren bir yapıdır.
public class Paginate<T>
{
    public Paginate()
    {
        Items = Array.Empty<T>();
    }

    public int Size { get; set; }
    public int Index { get; set; } // default 0 olcak
    public int Count { get; set; } // toplam data sayısı, veri sayının fazlalığına göre long olarakta oluşturabilirsin.
    public int Pages { get; set; } // toplam kaç sayfa
    public IList<T> Items { get; set; } // Tüm datalar
    public bool HasPrevious => Index > 0; // bir önceki sayfa var mı -> Index 0'dan büyükteyse bir önceki sayfa var anlamına gelir.
    public bool HasNext => Index + 1 < Pages; // bir sonraki sayfa var mı -> Index+1 Pages sayısından küçükse var
}
