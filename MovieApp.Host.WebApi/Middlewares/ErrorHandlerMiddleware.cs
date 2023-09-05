using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using MovieApp.Core.Abstractions.Exceptions;
using Serilog;

namespace MovieApp.Host.WebApi.Middlewares;

[ExcludeFromCodeCoverage]
public class ErrorHandlerMiddleware
{
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
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            switch (error)
            {
                case InputValidationException v:
                case BusinessException b:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case NotFoundException n:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    // unhandled error
                    Log.Error(error, error.Message);
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(new { message = error?.Message });
            await response.WriteAsync(result);
        }
    }
}