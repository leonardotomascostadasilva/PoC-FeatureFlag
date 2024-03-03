using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFeatureManagement(builder.Configuration.GetSection("FeatureManagement"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", async (IFeatureManager featureManager) =>
    {
        List<WeatherForecast> forecast;
        if (await featureManager.IsEnabledAsync("FeatureC"))
        {
            forecast = [new WeatherForecast(DateOnly.MinValue, 10, "old")];
        }
        else
        {
            forecast = [new WeatherForecast(DateOnly.MinValue, 10, "new")];
        }

        return forecast;
    })
    .WithName("GetWeatherForecast")
    .WithOpenApi();

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary);