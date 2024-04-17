using WeatherApi.Models;

namespace WeatherApi.Services.Interfaces;

public interface IDataService
{
    public RawData? ComputeRaw(string content);
    public ProcessedData ComputeProcessed(RawData rawData);
    public AverageData ComputeAverage(params ProcessedData[] processedData);
}
