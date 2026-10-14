namespace KasabaKIAT.Core.Middleware;

public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext ctx)
    {
        try
        {
            await next(ctx);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception for {Method} {Path}", ctx.Request.Method, ctx.Request.Path);
            ctx.Response.ContentType = "application/json";
            ctx.Response.StatusCode = 500;
            await ctx.Response.WriteAsJsonAsync(new { error = "Internal server error" });
        }
    }
}
