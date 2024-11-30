using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using Core.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class DataSeeder
    {
        public static void Initialize(WeatherContext context)
        {
            context.Database.EnsureCreated();

            // Kolla om databasen redan har data, annars seed den
            if (!context.WeatherData.Any())
            {
                var weatherData = LoadWeatherDataFromCsv("TempFuktData.csv");
                context.WeatherData.AddRange(weatherData);
                context.SaveChanges();
            }
        }

        // Laddar väderdata från CSV
        private static List<WeatherData> LoadWeatherDataFromCsv(string filePath)
        {
            var data = new List<WeatherData>();

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines.Skip(1))  
            {
                var values = line.Split(',');

                var weather = new WeatherData
                {
                    Date = DateTime.Parse(values[0]),
                    Location = values[1],
                    Temperature = double.Parse(values[2]),
                    Humidity = double.Parse(values[3])
                };

                data.Add(weather);
            }

            return data;
        }
    }
}
