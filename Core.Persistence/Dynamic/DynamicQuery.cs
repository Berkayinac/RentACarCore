using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Dynamic;

// DynamicQuery -> İçerisinde filter'lar ve sort'lar olan yapıdır.
// Kullanıcının seçtiği filter nesnelerini ve sort tipini alarak LINQ'ya bu seçilen kıstaslara göre listeleme işlemini yap sorgusunu göndericez.
public class DynamicQuery
{
    // Sort birden fazla olabilir çünkü Sort class'ı içerisinde iç içe geçmiş sort liste yapısı yok
    // bu yüzden birden fazla isteğe göre sort edilebilinmesi için IEnumerable<Sort> yapısı kullandık.
    public IEnumerable<Sort>? Sort { get; set; }

    // Filter nesnesini tek başına geçtik çünkü Filter class'ı içerisinde IEnumerable<Filter> olarak Filter'ları listeledik.
    // Filter'ların bu şekilde olmasının sebebi -> Bir filter'ın içerisinde birden fazla filter olabilir her bir filter'ın içinden birden fazla filter daha olabilir
    // Filter1 İçinde -> FilterN ->  FilterNN
    // örnek olarak parantezler oluşturup önce orası çalışsın şeklinde belirtmemizi sağlar.
    public Filter? Filter { get; set; }

    public DynamicQuery()
    {
        
    }

    public DynamicQuery(IEnumerable<Sort>? sort, Filter? filter)
    {
        Sort = sort;
        Filter = filter;
    }
}

// ADO.NET
// select * from Cars where UnitPrice < 100 and (transmission = 1 or fuel = 2.9km)

// EF
// p=>p.UnitPrice <= 100 && (transmission = 1 or fuel = 2.9km)

// LINQ -> EF yapısının string şeklinde oluşturulması gerekiyor. Sonra bu EF kodunu linq'ya çevir.
//  p=>p.UnitPrice <= 100