using FlatsAPI.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace FlatsAPI.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        /*catch (NotFoundException notFoundException)
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
        }*/
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case NotFoundException:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case BadRequestException:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case ForbiddenException:
                    response.StatusCode = (int)HttpStatusCode.Forbidden;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var result = JsonSerializer.Serialize(new
            {
                TimeStamp = DateTime.Now.ToString(),
                StatusCode = response.StatusCode,
                Method = context.Request.Method,
                Path = (string)context.Request.Path,
                Message = error?.Message
            },
            jsonSerializerOptions
            );

            await response.WriteAsync(result);
        }
    }
}
