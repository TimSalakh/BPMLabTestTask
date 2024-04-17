using Newtonsoft.Json;
using WeatherApi.Models;
using WeatherApi.Services.Interfaces;

namespace WeatherApi.Services.Implementations;

public class DataService : IDataService
{
    public RawData ComputeRaw(string content)
    {
        dynamic? deserialized = JsonConvert.DeserializeObject(content);

        return new RawData
        {
            City = deserialized?.name,
            Timezone = deserialized?.timezone,
            KelvinTemperature = deserialized?.main.temp,
            Pressure = deserialized?.main.pressure,
            Humidity = deserialized?.main.humidity,
            WindSpeed = deserialized?.wind.speed,
            Cloudy = deserialized?.clouds.all
        };
    }

    public ProcessedData ComputeProcessed(RawData rawData)
    {
        return new ProcessedData
        {
            City = rawData.City,
            CityCurrentTime = DateTime.UtcNow.AddSeconds(rawData.Timezone)
                .ToString("dd.MM.yy HH:mm"),
            ServerCurrentTime = DateTime.Now
                .ToString("dd.MM.yy HH:mm"),
            TimeDifference = (DateTime.UtcNow.AddSeconds(rawData.Timezone) - DateTime.Now)
                .ToString(@"dd\.hh\:mm"),
            CelsiumTemperature = Math.Round(rawData.KelvinTemperature - 273.15, 1),
            Pressure = rawData.Pressure,
            Humidity = rawData.Humidity,
            WindSpeed = rawData.WindSpeed,
            Cloudy = rawData.Cloudy
        };
    }

    public AverageData ComputeAverage(params ProcessedData[] processedData)
    {
        return new AverageData
        {
            AverageCelsiumTemperature = processedData
                .Select(pd => pd.CelsiumTemperature)
                .Average(),
            AveragePressure = processedData
                .Select(pd => pd.Pressure)
                .Average(),
            AverageHumidity = processedData
                .Select(pd => pd.Humidity)
                .Average(),
            AverageWindSpeed = processedData
                .Select(pd => pd.WindSpeed)
                .Average(),
            AverageCloudy = processedData
                .Select(pd => pd.Cloudy)
                .Average()
        };
    }
}
