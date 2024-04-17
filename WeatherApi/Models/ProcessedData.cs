namespace WeatherApi.Models;

public class ProcessedData
{
    public string City { get; set; }
    public string CityCurrentTime { get; set; }
    public string ServerCurrentTime { get; set; }
    public string TimeDifference { get; set; }
    public double CelsiumTemperature { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public int Cloudy { get; set; }
}
