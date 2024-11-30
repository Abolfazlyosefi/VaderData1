using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Core.DataAccess
{
    public class CsvLoader
    {
        // Ladda väderdata från CSV till en lista
        public static List<WeatherData> LoadWeatherDataFromCsv(string filePath, char delimiter = ',', bool logInvalidRows = false)
        {
            var weatherDataList = new List<WeatherData>();

            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("CSV-filen kunde inte hittas.", filePath);
                }

                var lines = File.ReadAllLines(filePath);

                if (lines.Length < 2)
                {
                    throw new InvalidDataException("CSV-filen innehåller inga data eller saknar rubrikrad.");
                }

                foreach (var line in lines.Skip(1)) // Hoppa över rubrikraden
                {
                    var columns = line.Split(delimiter);

                    if (columns.Length != 4)
                    {
                        if (logInvalidRows)
                            Console.WriteLine($"Ogiltig rad ignoreras: {line}");
                        continue; // Hoppa över rader med felaktigt antal kolumner
                    }

                    if (!DateTime.TryParse(columns[0], out var date) ||
                        !double.TryParse(columns[2], out var temperature) ||
                        !double.TryParse(columns[3], out var humidity))
                    {
                        if (logInvalidRows)
                            Console.WriteLine($"Ogiltig data på rad ignoreras: {line}");
                        continue; // Hoppa över rader med ogiltigt format
                    }

                    var weatherData = new WeatherData
                    {
                        Date = date,
                        Location = columns[1],
                        Temperature = temperature,
                        Humidity = humidity
                    };

                    weatherDataList.Add(weatherData);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid inläsning av CSV-filen: {ex.Message}");
            }

            return weatherDataList;
        }


        // Ladda data från CSV och spara i databasen
        public static void LoadData(string filePath, WeatherContext context, char delimiter = ',')
        {
            try
            {
                // Ladda data från CSV utan att logga ogiltiga rader
                var weatherDataList = LoadWeatherDataFromCsv(filePath, delimiter);

                if (!weatherDataList.Any())
                {
                    Console.WriteLine("Ingen giltig data att ladda från CSV-filen.");
                    return;
                }

                // Lägg till data i databasen
                foreach (var weatherData in weatherDataList)
                {
                    var exists = context.WeatherData.Any(w =>
                        w.Date == weatherData.Date &&
                        w.Location == weatherData.Location &&
                        Math.Abs(w.Temperature - weatherData.Temperature) < 0.01 &&
                        Math.Abs(w.Humidity - weatherData.Humidity) < 0.01);

                    if (!exists)
                    {
                        context.WeatherData.Add(weatherData);
                    }
                }

                context.SaveChanges();
                Console.WriteLine("Data från CSV har laddats och sparats i databasen.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ett fel inträffade vid sparande av data i databasen: {ex.Message}");
            }
        }

    }
}
