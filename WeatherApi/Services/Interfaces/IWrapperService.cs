using WeatherApi.Models;

namespace WeatherApi.Services.Interfaces;

public interface IWrapperService
{
    public Task<ProcessedData?> GetProcessedDataAsync(string cityName, string apiKey);
}
