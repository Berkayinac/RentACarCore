using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Persistence.Dynamic;

// veriyi sıralı bir şekilde dizmek için Sort class'ı kullanılır.
public class Sort
{
    public string Field { get; set; } // sıralanacak olan alan
    public string Dir { get; set; } // Direction -> // A-Z, Z-A, 1-9, 9-1 şeklinde sıralayabiliriz.

    public Sort()
    {
        Field = string.Empty;
        Dir = string.Empty;
    }

    public Sort(string field, string dir)
    {
        Field = field;
        Dir = dir;
    }
}