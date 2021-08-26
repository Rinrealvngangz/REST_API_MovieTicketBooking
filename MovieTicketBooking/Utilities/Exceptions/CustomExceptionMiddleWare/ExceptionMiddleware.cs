using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Dtos;
namespace Utilities.Exceptions.CustomExceptionMiddleWare
{
    public  class ExceptionMiddleware
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
            catch(Exception ex)
            {
                await HandlerExceptionAsync(httpContext,ex);
            }
        }

        private Task HandlerExceptionAsync(HttpContext httpContext,Exception ex)
        {
          
            httpContext.Request.ContentType ="application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return httpContext.Response.WriteAsync(new ErrorDetail {
                   StatusCode =httpContext.Response.StatusCode,
                   Message ="Internal Server Error from the custom middleware"
            }.ToString());
        }
    }
}
