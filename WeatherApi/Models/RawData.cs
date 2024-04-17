namespace WeatherApi.Models;

public class RawData
{
    public string City { get; set; }
    public int Timezone { get; set; }
    public double KelvinTemperature { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
    public double WindSpeed { get; set; }
    public int Cloudy { get; set; }
}
