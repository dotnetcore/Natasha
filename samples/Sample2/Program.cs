var builder = WebApplication.CreateBuilder(args);

NatashaManagement.Preheating();

// Add services to the container.

var app = builder.Build();

//NatashaManagement.AddGlobalReference(typeof(int));
//NatashaManagement.AddGlobalReference(typeof(Console));

// Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{

    NDelegate.RandomDomain().ConfigClass(item => item.Using("System")).Action("Console.WriteLine(\"1111111111111111111111111111\");")();
    var forecast = Enumerable.Range(1, 5).Select(index =>
       new WeatherForecast
       (
           DateTime.Now.AddDays(index),
           Random.Shared.Next(-20, 55),
           summaries[Random.Shared.Next(summaries.Length)]
       ))
        .ToArray();
    return forecast;
});

app.Run();

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}