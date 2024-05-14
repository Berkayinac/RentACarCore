using Core.CrossCuttingConcern.Exceptions.Types;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = Core.CrossCuttingConcern.Exceptions.Types.ValidationException; // ValidationException -> Hem fluentvalidaiton'da hem de bizde var -> bizde o yüzden bu şekilde using'te belirtmeliyiz ki ne kullnacağını bilsin.

namespace Core.Application.Pipelines.Validation;

public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{

    private readonly IEnumerable<IValidator<TRequest>> _validators;

    // buradaki constructor bloğunu Fluentvalidation'ına ait olan DependencyInjection paketi sağlayacak ama ana projede olmalı o paket.
    public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }



    // Her request ve response için bir validator varsa o zaman bu handle metodunu çalıştır diyoruz.
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        ValidationContext<object> context = new(request);

        IEnumerable<ValidationExceptionModel> errors = _validators
            .Select(validator => validator.Validate(context)) // context'in içeriğini yani gelen request'i validate et.
            .SelectMany(result => result.Errors) // eğerki birden fazla validator varsa bir request için, onlara ait error'lerin hepsini topla
            .Where(failure => failure != null) // eğer ki bir failure varsa 
            .GroupBy( 
                keySelector: p => p.PropertyName, // onları keySelector kullanarak PropertyName'lerini al
                resultSelector: (propertyName, errors) => // resultSelector kullanarak içerisindeki propertyName ve errors'ları benim ValidationExceptionModel'ime göre uyarla son olarak listeleyip gönder
                    new ValidationExceptionModel { Property = propertyName, Errors = errors.Select(e => e.ErrorMessage) }
            ).ToList();

        if (errors.Any())
            throw new ValidationException(errors); 
        TResponse response = await next(); // eğer ki hata yoksa sistemde sonraki metodu çalıştır.
        return response;
    }
}

// Pipeline -> Command ve query'lere uygulanan middleware'ler diyebiliriz.
// Query'lerde bana bir değer gönderiyorsan sunlara uymak zorundasın gibi gibi yapılar verebilir.
// örnek olarak tc'yi 11 karakterli bir şekilde kullanıcıdan istiyoruz gibi