﻿using Core.CrossCuttingConcern.Logging;
using Core.CrossCuttingConcern.Serilog;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Application.Pipelines.Logging;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ILoggableRequest
{

    private readonly IHttpContextAccessor _httpContextAccessor; // İlgili HttpContext'indeki bilgilere erişmek için kullanılır. Bu sayede Logu kimin gerçekleştirdiğini öğrenebiliriz.
    private readonly LoggerServiceBase _loggerServiceBase; // loglanacak yeri belirmek için

    public LoggingBehavior(IHttpContextAccessor httpContextAccessor, LoggerServiceBase loggerServiceBase)
    {
        _httpContextAccessor = httpContextAccessor;
        _loggerServiceBase = loggerServiceBase;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        List<LogParameter> logParameters =
            new()
            {
                new LogParameter{Type = request.GetType().Name, Value = request}, // Parametre adlarına ve değerlerine eriş.
            };

        LogDetail logDetail
            = new()
            {
                MethodName = next.Method.Name, // Metodun adına al
                Parameters = logParameters, // parametreleri al
                User = _httpContextAccessor.HttpContext.User.Identity?.Name ?? "?" // ilgili HttpContext'indeki user'a ait bilgileri ver, User yoksa ? olarak bilinmiyor de.
            };

        _loggerServiceBase.Info(JsonSerializer.Serialize(logDetail));
        return await next();
    }
}