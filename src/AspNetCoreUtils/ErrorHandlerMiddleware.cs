using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
// ReSharper disable ConsiderUsingConfigureAwait

namespace AspNetCoreUtils
{
    public class ErrorHandlerMiddleware
    {
        public class HttpStatusCodeException : Exception
        {
            public HttpStatusCode StatusCode { get; set; }

            public HttpStatusCodeException(HttpStatusCode statusCode)
            {
                StatusCode = statusCode;
            }
        }

        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HttpStatusCodeException exception)
            {
                context.Response.StatusCode = (int)exception.StatusCode;
                context.Response.Headers.Clear();
            }
        }
    }
}