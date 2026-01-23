using SmartHome.BL;
using SmartHome.DL;
using SmartHome.Host.Validators;
using SmartHome.Host.HealthChecks; // Добавено: за да вижда папката с HealthChecks
using FluentValidation;
using Mapster;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace SmartHome.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Serilog 
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            builder.Host.UseSerilog();

            
            builder.Services
                .AddDataLayer(builder.Configuration)
                .AddBusinessLayer();

            //  Mapster/Automapper)
            builder.Services.AddMapster();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            //Swagger)
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Smart Home API", Version = "v1" });
            });

            
            builder.Services.AddHealthChecks()
                .AddCheck<MongoHealthCheck>("MongoDB");

           
            builder.Services.AddValidatorsFromAssemblyContaining<AddRoomRequestValidator>();

            var app = builder.Build();

            
            app.MapHealthChecks("/health");

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartHome V1");
                });
            }

            app.UseAuthorization();

            //контролер за CRUD и един за бизнес сървиса
            app.MapControllers();

            app.Run();
        }
    }
}