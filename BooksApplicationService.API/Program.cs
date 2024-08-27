
using System.Text;
using BooksApplicationService.API.Config;
using BooksApplicationService.API.Middleware;
using BooksApplicationService.API.Model.Data;
using BooksApplicationService.API.Model.Entities;
using BooksApplicationService.API.Model.Interfaces;
using BooksApplicationService.API.Model.Services;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BooksApplicationService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // 1. Configure the EF Core with your connection string
            string conStr = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<BookDbContext>(options =>
            {
                options.UseSqlServer(conStr);
            });

            // 2. Add Identity services and configure JWT authentication 
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                            .AddEntityFrameworkStores<BookDbContext>()
                            .AddDefaultTokenProviders();

            // 3. Add JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            });

            builder.Services.AddControllers()
                .AddXmlDataContractSerializerFormatters()
                .AddNewtonsoftJson();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            

            // 4. Add Authorization (this is required to protect your endpoints)
            builder.Services.AddAuthorization();

            // 5. Inject the OData and other necessary services
            builder.Services.AddOData();


            //Dependency injection
            builder.Services.AddTransient<ITokenService, TokenService>();
            builder.Services.AddTransient<IGreetingsService, GreetingsService>();


            //Configuration injection
            builder.Services.Configure<MySettings>(builder.Configuration.GetSection("MySettings"));


            var app = builder.Build();


            // Use middleware
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            // 6. Use Authentication and Authorization middlewares
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.EnableDependencyInjection();
                endpoints.Select().OrderBy().Filter().MaxTop(100).SkipToken().Count();
            });

            app.MapControllers();

            app.Run();
        }
    }
}
