using System.Text.Json.Serialization;
using NewsScope.Api.Services;
using NewsScope.Api.Strategies;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<INewsScoreService, NewsScoreService>();
builder.Services.AddScoped<IMeasurementScoreStrategy, TemperatureScoreStrategy>();
builder.Services.AddScoped<IMeasurementScoreStrategy, HeartRateScoreStrategy>();
builder.Services.AddScoped<IMeasurementScoreStrategy, RespiratoryRateScoreStrategy>();


var app = builder.Build();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.MapControllers();

app.Run();
