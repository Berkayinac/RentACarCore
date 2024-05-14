using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcern.Exceptions.Extensions;

// Burada WebAPI'daki var builder = WebApplication.CreateBuilder(args); olan Application'ımızı extend ediyoruz.
// IApplicationBuilder -> implementini WebApplication class'ı gerçekleştirdiğinden onu bu şekilde extend edebiliyoruz.
public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionMiddleware>();
}
