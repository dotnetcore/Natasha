using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Natasha.CSharp.HotExecutor.Component;

namespace Webapi2Sample
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //HE:Async
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            var modelMetadataProvider = app.Services.GetService(typeof(IModelMetadataProvider));
            var controllerActivatorProvider = app.Services.GetService(typeof(IControllerActivatorProvider));

            HEProxy.SetPreHotExecut(() => {

                var action = HEDelegateHelper.GetDelegate(modelMetadataProvider.GetType(), "ClearCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                action(modelMetadataProvider);

                var action2 = HEDelegateHelper.GetDelegate(controllerActivatorProvider.GetType(), "ClearCache", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                action2(modelMetadataProvider);

            });
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.AsyncToHotExecutor();
            app.UseAuthorization();

            var summaries = new[]
            {
                "Freezing1", "Bracing1", "Chilly1", "Cool1", "Mild1", "Warm1", "Balmy1", "Hot1", "Sweltering1", "Scorching1"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast");


            app.MapGet("/weatherforecast1", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast1");


            app.MapGet("/weatherforecast2", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast2");

            app.Run();
        }
    }
}
