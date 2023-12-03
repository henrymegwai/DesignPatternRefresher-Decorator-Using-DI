using Microsoft.Extensions.Caching.Memory;
using System;

namespace DecoratorDesignPattern.WeatherInterface
{
    public class WeatherServiceCachingDecorator : IWeatherService
    {
        private IWeatherService _innerWeatherServices;
        private IMemoryCache _cache;

        public WeatherServiceCachingDecorator(IWeatherService innerWeatherServices, IMemoryCache cache)
        {
            _innerWeatherServices = innerWeatherServices;
            _cache = cache;
        }
        public CurrentWeather GetCurrentWeather(string location)
        {
            string cacheKey = $"WeatherConditions::{location}";
            if (_cache.TryGetValue<CurrentWeather>(cacheKey, out var currentWeather))
                return currentWeather;

            var currentConditions = _innerWeatherServices.GetCurrentWeather(location);
            _cache.Set(cacheKey, currentConditions, TimeSpan.FromMinutes(10));
            return currentConditions;
        }

        public LocationForecast GetForecast(string location)
        {
            string cacheKey = $"WeatherForecast::{location}";
            if (_cache.TryGetValue<LocationForecast>(cacheKey, out var forecast))
                return forecast;

            var locationForeCast = _innerWeatherServices.GetForecast(location);
            _cache.Set<LocationForecast>(cacheKey, locationForeCast, TimeSpan.FromMinutes(10));
            return locationForeCast;
        }
    }
}
