using Microsoft.EntityFrameworkCore;
using Testing.Repositories.Implementations;
using Testing.Repositories.Interfaces;
using Testing.Services.Implementations;
using Testing.Services.Interfaces;
using Microsoft.OpenApi.Models;
using AutoMapper;
using Testing.Services.Mappings;
using Testing.Repositories;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container

        builder.Services.AddDbContext<DbPrintedBoardsContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Регистрация репозиториев

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUserSessionRepository, UserSessionRepository>();
        builder.Services.AddScoped<IMailRepository, MailRepository>();
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        // Регистрация сервисов

        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IProductService, ProductService>();

        // Настройки SMTP

        builder.Services.Configure<SmtpSettings>(      
            builder.Configuration.GetSection("SmtpSettings"));

        builder.Services.AddControllersWithViews();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                policy =>
                {
                    policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
        });

        // Swagger

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Testing API", Version = "v1" });
        });

        builder.Services.AddAutoMapper(typeof(MappingProfile));

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Testing API V1");
            });
        }

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();
        app.UseCors("AllowFrontend");
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}