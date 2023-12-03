using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace DecoratorDesignPattern.WeatherInterface
{
    public class WeatherServiceLoggingDecorator : IWeatherService
    {
        private IWeatherService _innerWeatherService;
        private ILogger<WeatherServiceLoggingDecorator> _logger;

        public WeatherServiceLoggingDecorator(IWeatherService innerWeatherService, ILogger<WeatherServiceLoggingDecorator> logger)
        {
            _innerWeatherService = innerWeatherService;
            _logger = logger;
        }
        public CurrentWeather GetCurrentWeather(string location)
        {
            Stopwatch sw = Stopwatch.StartNew();
            CurrentWeather currentWeather = _innerWeatherService.GetCurrentWeather(location);
            sw.Stop();
            long elapsedMillis = sw.ElapsedMilliseconds;
            _logger.LogWarning($"Retrieved weather data for {location} - Elapsed ms {elapsedMillis}");
            return currentWeather;
        }

        public LocationForecast GetForecast(string location)
        {
            Stopwatch sw = Stopwatch.StartNew();
            LocationForecast forecast = _innerWeatherService.GetForecast(location);
            sw.Stop();
            long elapsedMillis = sw.ElapsedMilliseconds;
            _logger.LogWarning($"Retrieved forecast data for {location} - Elapsed ms {elapsedMillis}");
            return forecast;
        }
    }
}