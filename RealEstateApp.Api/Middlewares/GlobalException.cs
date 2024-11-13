using System.Text.Json;

namespace RealEstateApp.Api.Middlewares;

public class GlobalException
{
    private RequestDelegate _next;
    public GlobalException(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            await HandleExceptionAsync(context, exception);
        }
    }
    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)StatusCodes.Status500InternalServerError;

        var response = new
        {
            error = new
            {
                message = "An error occurred while processing your request.",
                details = exception.Message
            }
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}