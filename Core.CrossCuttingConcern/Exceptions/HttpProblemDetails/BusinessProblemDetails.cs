using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcern.Exceptions.HttpProblemDetails;

// .net'de bulunan ProblemDetails class'ını extent ederek exception içerisinde olan olayları açıklamak için yani details için 
//ProblemDetails yapısını kullanıyoruz. Ancak bu bize yetmiyor onu extension edicez.

// ProblemDetails -> RFC7807 kurallarını burada veriyor ÖĞREN!!
public class BusinessProblemDetails : ProblemDetails
{
    public BusinessProblemDetails(string detail)
    {
        // hata bilgisi başlığı => BusinessProblems içerisinde yazdığımız için Rule Violation olarak geçtik
        Title = "Rule Violation";
        Detail = detail;
        Status = StatusCodes.Status400BadRequest;
        Type = "https://example.com/probs/business"; // alınan hataları kullanıcı gidip neden aldığını kontrol etmesi için hazırlanan bir sayfa

    }
}
