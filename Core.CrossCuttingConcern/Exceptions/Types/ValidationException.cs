using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcern.Exceptions.Types;

public class ValidationException : Exception
{
    // toplamda birden fazla da hata olabilir
    // yani hem name'de hem de age'de hata olabilir.
    public IEnumerable<ValidationExceptionModel> Errors { get; }

    public ValidationException()
        : base()
    {
        Errors = Array.Empty<ValidationExceptionModel>();
    }

    public ValidationException(string? message)
        : base(message)
    {
        Errors = Array.Empty<ValidationExceptionModel>();
    }

    public ValidationException(string? message, Exception? innerException)
        : base(message, innerException)
    {
        Errors = Array.Empty<ValidationExceptionModel>();
    }

    public ValidationException(IEnumerable<ValidationExceptionModel> errors)
        : base(BuildErrorMessage(errors))
    {
        Errors = errors;
    }

    // Kullanıcıya hata verirken bilgiye yer vermeden sadece işte tcno 11 haneli değil gibi bilgileri vermek için bu metot kullanılıyor.
    private static string BuildErrorMessage(IEnumerable<ValidationExceptionModel> errors)
    {
        IEnumerable<string> arr = errors.Select(
            x => $"{Environment.NewLine} -- {x.Property}: {string.Join(Environment.NewLine, values: x.Errors ?? Array.Empty<string>())}"
        );
        return $"Validation failed: {string.Join(string.Empty, arr)}";
    }
}

// bir listeye validasyon hatalarını koyup göndermekte istiyoruz. sadece hata var diyip bırakmamalıyız.
public class ValidationExceptionModel
{
    // Örnek olarak 3 tane class'ımızda alan olsun
    // name, city, age alanları olsun
    // her bir alanda birden fazla hata olabilir.
    public string Property { get; set; } // hangi alanda örnek olarak Name
    public IEnumerable<string>? Errors { get; set; } // tüm hatalı olan alanlar.
}



// ValidationExceptionModel -> içerisinde 1 alanın birden çok hatası olabilir o yüzden IEnumerable yazıldı

// ValidationException içerisinde birden çok alanın birden çok yine hatası olabilir yine o yüzden IEnumerable yazıldı