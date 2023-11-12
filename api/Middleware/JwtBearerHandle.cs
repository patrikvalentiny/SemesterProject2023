using Serilog;
using service;
using service.Services;

namespace api.Middleware;

public class JwtBearerHandler
{
    private readonly RequestDelegate _next;

    public JwtBearerHandler( RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext http)
    {
        var jwtHelper = http.RequestServices.GetRequiredService<JwtService>();

        try
        {
            var authHeader = http.Request.Headers.Authorization.FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Split(" ")[1];
                var data = jwtHelper.ValidateAndDecodeToken(token);
                http.SetSessionData(data);
            }
        }
        catch (Exception e)
        {
            Log.Error(e, "Error extracting user from bearer token in Authorization header");
        }

        await _next.Invoke(http);
    }
}