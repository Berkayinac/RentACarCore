using Core.Persistence.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Responses;

public class GetListResponse<T> : BasePageableModel
{
    private IList<T> _items;
    
    // property yerine field olarak kullanmamızın sebebi 
    // defensive gitmek için eğer ki _items null ise yeni List oluştursun ama _items'ın içerisinde veri varsa onları döndürsün
    public IList<T> Items {
        get => _items??= new List<T>(); 
        set => _items = value; 
    }
}
