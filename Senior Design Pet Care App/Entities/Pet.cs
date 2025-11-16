using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Senior_Design_Pet_Care_App.Entities
{
    public enum ActivityLevel
    {
        Low,
        Medium,
        High
    }

    public class Pet
    {
        [Key]
        public int Id { get; set; }

        // foreign key to User.Id
        public int UserId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Breed { get; set; } = string.Empty;

        [Required]
        public int Age { get; set; } // in years (you can change to decimal for months)

        [Required]
        public decimal Height { get; set; } // use whatever units your app expects (e.g., inches)

        [Required]
        public decimal Weight { get; set; } // pounds (or kg) — be consistent in UI

        [Required]
        public ActivityLevel ActivityLevel { get; set; } = ActivityLevel.Medium;

        // Foods and Medications are stored as CSV in DB for simplicity
        public string? FoodsCsv { get; set; }
        public string? MedicationsCsv { get; set; }

        public DateTime? MostRecentVetAppointment { get; set; }

        // store image bytes in DB (nullable)
        public byte[]? PictureData { get; set; }

        public string? Notes { get; set; }

        //generated advice from OpenAI
        [MaxLength(4000)]
        public string? Advice { get; set; }

        // convenience properties not mapped are handled at app-level (component will parse CSV)
    }
}