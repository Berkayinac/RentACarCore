using Core.CrossCuttingConcern.Exceptions.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcern.Exceptions.Handlers;

public abstract class ExceptionHandler
{
    // Switch için yeni bir syntax yöntemi bu
    // Eğerki gelen exception bir BusinessException ise -> Bunu HandleException'de businessException olarak except et.
    public Task HandleExceptionAsync(Exception exception) =>
        exception switch
        {
            BusinessException businessException => HandleException(businessException),

            ValidationException validationException => HandleException(validationException),

            // eğer ki gelen Yukarıdaki exception'lar dışında gelen bir exception ise onu direkt ana parametre olan exception'ı throwla
            _ => HandleException(exception)
        };

    // ExceptionHandler class'ını inherit eden sınıf HandleException metodunu doldurmak zorunda olsun.
    // HTTP kendisine göre burayı implemente edicek.

    //gelen exception -> BusinessException businessException => HandleException(businessException),
    // ise bu metot çalışcak.
    protected abstract Task HandleException(BusinessException businessException);

    protected abstract Task HandleException(ValidationException validationException);

    // _ => HandleException(exception) -> normal exception ise bu metot çalışacak.
    protected abstract Task HandleException(Exception exception);
}