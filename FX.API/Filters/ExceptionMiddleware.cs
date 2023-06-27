using FX.API.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Threading.Tasks;
using FX.Application.Contracts;
//using FX.Infrastructure.Contracts;

namespace FX.API.Filters
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                var sql = httpContext.RequestServices.GetService<ISqlDBObjects>();
                await sql.StoredProcedures.LogSystemErrorAsync(e.Message, httpContext.Request.GetEncodedPathAndQuery());

                httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await httpContext.Response.WriteAsJsonAsync(new JsonMessage<string>()
                {
                    Status = false,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    ErrorMessage = ResponseMessages.InternalServerError
                });
            }
        }
    }
}
