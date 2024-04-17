using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Xml.Serialization;
using WeatherApi.Models;
using WeatherApi.Services.Interfaces;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/current-weather")]
public class WeatherController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IDataService _dataService;
    private readonly IWrapperService _wrapperService;
    private readonly ILogger<WeatherController> _logger;    

    public WeatherController(
        IConfiguration configuration,
        IDataService dataService,
        IWrapperService wrapperService,
        ILogger<WeatherController> logger)
    {
        _configuration = configuration;
        _dataService = dataService;
        _wrapperService = wrapperService;
        _logger = logger;
    }

    [HttpGet("json-type/city={cityName}")]
    public async Task<IActionResult> GetWeatherJson(string cityName)
    {
        var apiKey = _configuration.GetSection("OpenWeatherMap:ApiKey").Value;
        var result = await _wrapperService.GetProcessedDataAsync(cityName, apiKey!);

        if (result == null)
        {
            _logger.LogCritical($"Weather result was null.");
            return BadRequest("Invalid city name.");
        }

        _logger.LogInformation("Api key received: {ApiKey}", apiKey);
        _logger.LogInformation("Weather result received: {Result}", 
            JsonConvert.SerializeObject(result));

        return Ok(result);
    }

    [HttpGet("json-type/api-key={apiKey}&city={cityName}")]
    public async Task<IActionResult> GetWeatherByApiKeyJson(string apiKey, string cityName)
    {
        var result = await _wrapperService.GetProcessedDataAsync(cityName, apiKey);

        if (result == null)
        {
            _logger.LogCritical($"Weather result was null.");
            return BadRequest("Invalid api key or city name.");
        }

        _logger.LogInformation("Api key received: {ApiKey}", apiKey);
        _logger.LogInformation("Weather result received: {Result}", 
            JsonConvert.SerializeObject(result));

        return Ok(result);
    }

    [HttpGet("json-type/first-city={cityName1}&second-city={cityName2}")]
    public async Task<IActionResult> GetAverageWeatherJson(string cityName1, string cityName2)
    {
        var apiKey = _configuration.GetSection("OpenWeatherMap:ApiKey").Value;
        var firstResult = await _wrapperService.GetProcessedDataAsync(cityName1, apiKey!);
        var secondResult = await _wrapperService.GetProcessedDataAsync(cityName2, apiKey!);

        if (firstResult == null || secondResult == null)
        {
            _logger.LogCritical($"One of weather results was null.");
            return BadRequest("Invalid city name.");
        }

        _logger.LogInformation("Api key received: {ApiKey}", apiKey);
        _logger.LogInformation("First weather result received: {FirstResult}", 
            JsonConvert.SerializeObject(firstResult));
        _logger.LogInformation("Second weather result received: {SecondResult}",
            JsonConvert.SerializeObject(secondResult));

        var averageData = _dataService.ComputeAverage(firstResult, secondResult);
        _logger.LogInformation("Average weather result received: {AverageData}",
            JsonConvert.SerializeObject(averageData));

        return Ok(new { averageData, firstResult, secondResult });
    }

    [HttpGet("xml-type/city={cityName}")]
    public async Task<IActionResult> GetWeatherXml(string cityName)
    {
        var apiKey = _configuration.GetSection("OpenWeatherMap:ApiKey").Value;
        var result = await _wrapperService.GetProcessedDataAsync(cityName, apiKey!);

        if (result == null)
        {
            _logger.LogCritical($"Weather result was null.");
            return BadRequest("Invalid city name.");
        }

        _logger.LogInformation("Api key received: {ApiKey}", apiKey);
        _logger.LogInformation("Weather result received: {Result}",
            JsonConvert.SerializeObject(result));

        var serializer = new XmlSerializer(typeof(ProcessedData));
        using var writer = new StringWriter();
        serializer.Serialize(writer, result);
        string xmlString = writer.ToString();

        return new ContentResult
        {
            Content = xmlString,
            ContentType = "application/xml",
            StatusCode = 200
        };
    }
}
