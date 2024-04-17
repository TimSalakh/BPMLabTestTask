using System.Net;
using WeatherApi.Services.Interfaces;

namespace WeatherApi.Services.Implementations;

public class GeocodingService : IGeocodingService
{
    private const string _baseApiUrl = "https://api.openweathermap.org/data/2.5/weather?q=";

    public async Task<string> GetCityDataAsync(string cityName, string apiKey)
    {
        var requestUrl = $"{_baseApiUrl}{cityName}&appid={apiKey}";

        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(requestUrl);

        if (response.StatusCode != HttpStatusCode.OK)
            return string.Empty;

        return await response.Content.ReadAsStringAsync();
    }
}
