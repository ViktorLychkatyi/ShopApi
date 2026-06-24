
using ShopApp.Interfaces;
using ShopApp.Middelwares;
using ShopApp.Middlewares;
using ShopApp.Services;

namespace ShopApp
{
    /*public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestTimer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestTimerMiddleware>();
        }
    }*/

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<ICategoryService, CategoryService>();

            var app = builder.Build();
 
            app.MapControllers();
            app.UseStaticFiles();
            app.UseMiddleware<UserCheckMiddleware>();

            app.Run();
        }
    }
}
