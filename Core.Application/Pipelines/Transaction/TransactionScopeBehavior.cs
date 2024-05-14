using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Core.Application.Pipelines.Transaction;

// where TRequest : IRequest<TResponse>, ITransactionalRequest -> nesne hem Request olmalı, hemde ITransactionalRequest'in imzasını taşıyor olmalı.

public class TransactionScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ITransactionalRequest
{
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        // TransactionScope -> oluştur ve ASYNC'i enable et.
        using TransactionScope transactionScope = new (TransactionScopeAsyncFlowOption.Enabled);
        TResponse response;

        try
        {
            response = await next(); // metotları çalıştır
            transactionScope.Complete(); // işlemler başarılı ise complete et.
        }
        catch(Exception ex)
        {
            transactionScope.Dispose(); // işlemler başarısız ise dispose et yani işlemleri geri al.
            throw;
        }

        return response;
    }
}
