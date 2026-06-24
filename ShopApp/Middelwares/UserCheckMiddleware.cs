using Microsoft.AspNetCore.Http;
using ShopDomain.Models;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShopApp.Middlewares
{
    public class UserCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public UserCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method == "POST" && context.Request.Path == "/api/user/register")
            {
                await _next(context);
                return;
            }

            context.Request.EnableBuffering();

            var user = await JsonSerializer.DeserializeAsync<User>(context.Request.Body);
            context.Request.Body.Position = 0;

            if (user?.Id == 1 && user.Login == "admin")
            {
                await _next(context);
                return;
            }

            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new { message = "No authorization" });
        }
    }
}