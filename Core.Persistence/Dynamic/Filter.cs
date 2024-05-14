using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Dynamic;

public class Filter
{
    public string Field { get; set; } // Filtrelencek alan // -> Vites tipi
    public string? Value { get; set; } // Alanın değeri // -> Otomatik
    public string Operator { get; set; } // büyük eşittir // -> İçinde geçen veya eşittir
    public string? Logic { get; set; } // and or logic // And or gibi bir değer

    public IEnumerable<Filter>? Filters { get; set; } // bir filtreye başka filtrelerde uygulamak için kullanılır. // filter içerisinde ayrı filtreler verilerek işlem tamamlanır.

    public Filter()
    {
        Field = string.Empty;
        Operator = string.Empty;
    }

    //  @operator -> isimlendirmesi c# içerisinde de operator keywordü olduğundan c#'ın değişken olarak anlaması için başına "@" işareti kondu.
    public Filter(string field, string @operator)
    {
        Field = field;
        Operator = @operator;
    }
}