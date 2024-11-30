using Core.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Core
{
    public class WeatherContext : DbContext
    {
        public DbSet<WeatherData> WeatherData { get; set; }

        
        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {
        }

        // Konfiguration för SQLite-databasen
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlite("Data Source=VaderData.db") // Använd anslutningssträng från konfig
                    .EnableSensitiveDataLogging()          // För att felsöka SQL-frågor
                    .LogTo(Console.WriteLine, LogLevel.Information); // Loggning av SQL-frågor
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Exempel på seed data
            modelBuilder.Entity<WeatherData>().HasData(
                new WeatherData
                {
                    Id = 1,
                    Date = new DateTime(2024, 1, 1),
                    Location = "Ute",
                    Temperature = -5,
                    Humidity = 80
                },
                new WeatherData
                {
                    Id = 2,
                    Date = new DateTime(2024, 1, 2),
                    Location = "Inne",
                    Temperature = 22,
                    Humidity = 40
                }
            );
        }
    }
}
