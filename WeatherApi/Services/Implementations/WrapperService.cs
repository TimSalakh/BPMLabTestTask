using WeatherApi.Models;
using WeatherApi.Services.Interfaces;

namespace WeatherApi.Services.Implementations;

public class WrapperService : IWrapperService
{
    private readonly IGeocodingService _geocodingService;
    private readonly IDataService _dataService;

    public WrapperService(
        IGeocodingService geocodingService,
        IDataService dataService)
    {
        _geocodingService = geocodingService;
        _dataService = dataService;
    }

    public async Task<ProcessedData?> GetProcessedDataAsync(string cityName, string apiKey)
    {
        var content = await _geocodingService.GetCityDataAsync(cityName, apiKey);

        if (string.IsNullOrEmpty(content))
            return null;

        var rawData = _dataService.ComputeRaw(content);
        var processedData = _dataService.ComputeProcessed(rawData!);

        return processedData;
    }
}
