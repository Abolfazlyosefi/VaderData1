using System;
using Core;
using System.Linq;

namespace Core.UI
{
    public class Menu
    {
        private readonly WeatherService _weatherService;

        public Menu(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        public void DisplayMenu()
        {
            while (true)
            {
                Console.Clear(); // Rensa skärmen varje gång
                Console.WriteLine("Välj ett alternativ:");
                Console.WriteLine("1. Visa medeltemperatur för ett datum (Ute/Inne)");
                Console.WriteLine("2. Sortera dagar efter medeltemperatur (Ute/Inne)");
                Console.WriteLine("3. Sortera dagar efter luftfuktighet (Ute/Inne)");
                Console.WriteLine("4. Visa meteorologisk höst och vinter");
                Console.WriteLine("5. Visa mögelrisk för inomhusmiljöer");
                Console.WriteLine("6. Avsluta");

                var choice = Console.ReadLine();

                Console.Clear(); // Rensa skärmen innan resultatet visas
                HandleUserChoice(choice);

                Console.WriteLine("\nTryck på en tangent för att återgå till menyn...");
                Console.ReadKey();
            }
        }


        private void HandleUserChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    // Medeltemperatur för ett datum (Ute/Inne)
                    Console.WriteLine("Ange datum (yyyy-MM-dd):");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime date))
                    {
                        Console.WriteLine("Ange plats (Ute/Inne):");
                        var location = Console.ReadLine();

                        var avgTemp = _weatherService.GetAverageTemperatureForDate(date, location);
                        if (avgTemp != 0)
                        {
                            Console.WriteLine($"Medeltemperatur för {date.ToShortDateString()} ({location}): {avgTemp:F1}°C");
                        }
                        else
                        {
                            Console.WriteLine("Ingen data tillgänglig för det angivna datumet och platsen.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Felaktigt datumformat. Försök igen.");
                    }
                    break;

                case "2":
                    Console.WriteLine("Ange plats (Ute/Inne):");
                    var tempLocation = Console.ReadLine();

                    try
                    {
                        var sortedTemp = _weatherService.GetDaysSortedByTemperature(tempLocation);
                        if (!sortedTemp.Any())
                        {
                            Console.WriteLine("Ingen data tillgänglig för att sortera efter medeltemperatur.");
                        }
                        else
                        {
                            Console.WriteLine($"Dagar sorterade efter medeltemperatur ({tempLocation}):");
                            foreach (var data in sortedTemp)
                            {
                                Console.WriteLine($"{data.Date.ToShortDateString()}: {data.AverageTemp:F1}°C");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ett fel inträffade: {ex.Message}");
                    }
                    break;


                case "3":
                    // Sortera dagar efter luftfuktighet (Ute/Inne)
                    Console.WriteLine("Ange plats (Ute/Inne):");
                    var humidityLocation = Console.ReadLine();

                    var sortedHumidity = _weatherService.GetDaysSortedByHumidity(humidityLocation);
                    if (!sortedHumidity.Any())
                    {
                        Console.WriteLine("Ingen data tillgänglig för att sortera efter luftfuktighet.");
                    }
                    else
                    {
                        Console.WriteLine("Sorterade dagar efter luftfuktighet:");
                        foreach (var data in sortedHumidity)
                        {
                            Console.WriteLine($"{data.Date.ToShortDateString()}: {data.AverageHumidity:F1}%");
                        }
                    }
                    break;

                case "4":
                    // Visa meteorologisk höst och vinter
                    Console.WriteLine("Ange plats (Ute/Inne):");
                    var seasonLocation = Console.ReadLine();

                    var autumn = _weatherService.GetMeteorologicalAutumnStart(seasonLocation);
                    var winter = _weatherService.GetMeteorologicalWinterStart(seasonLocation);

                    Console.WriteLine($"Meteorologisk höst startade: {autumn?.ToShortDateString() ?? "Ej funnen"}");
                    Console.WriteLine($"Meteorologisk vinter startade: {winter?.ToShortDateString() ?? "Ej funnen"}");
                    break;

                case "5":
                    // Visa mögelrisk för inomhusmiljöer
                    Console.WriteLine("Sorterade dagar baserat på mögelrisk (från lägst till högst):");
                    var sortedMoldRisk = _weatherService.GetSortedMoldRisk();
                    if (!sortedMoldRisk.Any())
                    {
                        Console.WriteLine("Ingen data tillgänglig för att beräkna mögelrisk.");
                    }
                    else
                    {
                        foreach (var item in sortedMoldRisk)
                        {
                            Console.WriteLine($"{item.Date.ToShortDateString()} - Risk: {item.MoldRisk:F2}");
                        }
                    }
                    break;

                case "6":
                    // Avsluta programmet
                    Console.WriteLine("Avslutar programmet...");
                    Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Ogiltigt val. Försök igen.");
                    break;
            }

            Console.WriteLine("Tryck på en tangent för att fortsätta...");
            Console.ReadKey(); // Vänta på användaren innan menyn visas igen
        }
    }
}
