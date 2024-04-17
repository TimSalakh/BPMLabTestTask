namespace WeatherApi.Services.Interfaces;

public interface IGeocodingService
{
    public Task<string> GetCityDataAsync(string cityName, string apiKey);
}
