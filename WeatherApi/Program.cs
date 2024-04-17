using WeatherApi.Services.Implementations;
using WeatherApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGeocodingService, GeocodingService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IWrapperService, WrapperService>();  

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
