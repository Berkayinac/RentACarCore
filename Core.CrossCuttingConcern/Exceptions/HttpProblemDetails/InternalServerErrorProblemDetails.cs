using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.CrossCuttingConcern.Exceptions.HttpProblemDetails;

// geriye kalan hatalar sistem kaynaklı, bizim ön görmediğimiz hatalar olabilir.
// bunlara internal server error deniyor.

public class InternalServerErrorProblemDetails : ProblemDetails
{
    public InternalServerErrorProblemDetails(string detail)
    {
        // hata bilgisi başlığı => BusinessProblems içerisinde yazdığımız için Rule Violation olarak geçtik
        Title = "Internal Server Error";
        Detail = "Internal Server Error";
        Status = StatusCodes.Status500InternalServerError;
        Type = "https://example.com/probs/internal"; // alınan hataları kullanıcı gidip neden aldığını kontrol etmesi için hazırlanan bir sayfa

    }
}