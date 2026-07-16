
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ShopApi.Services;
using ShopApplication.Interfaces;
using ShopApplication.Interfaces.Helpers;
using ShopApplication.Interfaces.Repository;
using ShopApplication.Interfaces.Services;
using ShopApplication.Mapping;
using ShopApplication.Services;
using ShopInfrastructure.Configuration;
using ShopInfrastructure.Data;
using ShopInfrastructure.Helpers;
using ShopInfrastructure.Repositories;
using ShopInfrastructure.Services;
using System.Reflection;
using System.Text;

namespace ShopApi
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

            builder.Services.AddDbContext<ShopDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

            });

            var configuration = builder.Configuration;

            //JWT Settings
            var jwtSettings = configuration
                .GetSection("Jwt")
                .Get<JwtSettings>()
                ?? throw new Exception("JWT settings not configured.");

            builder.Services.Configure<JwtSettings>
                (configuration.GetSection("Jwt"));

            // AutoMapper
            builder.Services.AddAutoMapper(
                _ => { },
                typeof(CategoryProfile).Assembly,
                typeof(UserProfile).Assembly
            );

            // CORS 
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //builder.Services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "Shop Api",
            //        Version = "v1"
            //    });
            //});

            //builder.Services.AddSwaggerGen(options =>
            //{
            //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            //    options.IncludeXmlComments(xmlPath);
            //});

            // Swagger + JWT
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT token"
                });

                options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });

            //builder.Services.AddSwaggerGen();

            // Authentication
            builder.Services.AddAuthentication(options =>
            {            
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })        
            .AddJwtBearer(options => 
            {   
                //Правила перевірки токена
                options.TokenValidationParameters = new TokenValidationParameters            
                {                
                    ValidateIssuer = true,                
                    ValidateAudience = true,                
                    ValidateLifetime = true,                
                    ValidateIssuerSigningKey = true,                
                    
                    ValidIssuer = jwtSettings.Issuer,                
                    ValidAudience = jwtSettings.Audience,                
                    IssuerSigningKey = new SymmetricSecurityKey(                    
                        Encoding.UTF8.GetBytes(jwtSettings.Key)                
                    ),                
                    
                    ClockSkew = TimeSpan.Zero            
                };        
            });        
            
            builder.Services.AddAuthorization();

            builder.Services.AddControllers();

            // SERVICES
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IJWTService, JWTService>();

            // REPOSITORIES
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

            // HELPERS
            builder.Services.AddSingleton<IHashHelper, HashHelper>();

            //----

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseCors("AllowAll");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShopApi v1");
                });
            }
            //app.ProductsEndpoints();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.UseStaticFiles();
            //app.UseMiddleware<UserCheckMiddleware>();


            app.Run();
        }
    }
}
