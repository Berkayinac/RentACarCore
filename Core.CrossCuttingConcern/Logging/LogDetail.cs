using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcern.Logging;

// Çalıştırılan class, metot bilgileri.
public class LogDetail
{
    public string FullName { get; set; } // Hangi class çalıştı
    public string MethodName { get; set; } // Hangi metot çalıştı
    public string User { get; set; } // kim çalıştırdı
    public List<LogParameter> Parameters { get; set; } // hangi parametreler ile çalıştırdı ?

    public LogDetail()
    {
        FullName = string.Empty;
        MethodName = string.Empty;
        User = string.Empty;
        Parameters = new List<LogParameter>();
    }

    public LogDetail(string fullName, string methodName, string user, List<LogParameter> parameters)
    {
        FullName = fullName;
        MethodName = methodName;
        User = user;
        Parameters = parameters;
    }
}
