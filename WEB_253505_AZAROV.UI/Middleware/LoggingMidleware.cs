using Microsoft.AspNetCore.Http.Extensions;
using Serilog;
namespace WEB_253505_AZAROV.UI.Middleware;
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext httpContext)
    {
        await _next(httpContext);
        if (httpContext.Response.StatusCode < 200 || httpContext.Response.StatusCode >= 300)
        {
            Log.Information($"---> request {httpContext.Request.GetDisplayUrl()} returns {httpContext.Response.StatusCode}");
        }
    }
}