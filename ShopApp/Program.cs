
using ShopApp.Interfaces;
using ShopApp.Middelwares;
using ShopApp.Middlewares;
using ShopApp.Services;
using Microsoft.OpenApi;
using System.Reflection;

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

            builder.Services.AddEndpointsApiExplorer();

            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "Shop Api",
            //        Version = "v1"
            //    });
            //});

            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                options.IncludeXmlComments(xmlPath);
            });

            builder.Services.AddControllers();
            builder.Services.AddSingleton<IProductService, ProductService>();
            builder.Services.AddSingleton<ICategoryService, CategoryService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopApi v1");
                });
            }
            //app.ProductsEndpoints();

            app.MapControllers();
            app.UseStaticFiles();
            //app.UseMiddleware<UserCheckMiddleware>();

            app.Run();
        }
    }
}
