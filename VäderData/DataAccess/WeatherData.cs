using System;
using System.ComponentModel.DataAnnotations;

namespace Core.DataAccess
{
    public class WeatherData
    {
        [Key]
        public int Id { get; set; }  // Primärnyckel för WeatherData

        [Required]
        public DateTime Date { get; set; }  // Datum för mätningen

        [Required]
        [MaxLength(50)]
        public string Location { get; set; }  // "Ute" eller "Inne"

        [Required]
        public double Temperature { get; set; }  // Temperatur vid mätningen

        [Required]
        public double Humidity { get; set; }  // Luftfuktighet vid mätningen
    }
}
