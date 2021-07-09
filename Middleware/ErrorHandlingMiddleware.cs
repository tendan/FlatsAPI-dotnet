using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlatsAPI.Exceptions;
using Microsoft.AspNetCore.Hosting;

namespace FlatsAPI.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(IWebHostEnvironment env)
        {
            _env = env;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (NotFoundException notFoundException)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(notFoundException.Message);
            }
            catch (BadRequestException badRequestException)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(badRequestException.Message);
            }
            catch (UnauthorizedException unauthorizedException)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(unauthorizedException.Message);
            }
            catch (Exception e)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync(e.ToString());
            }
        }
    }
}
