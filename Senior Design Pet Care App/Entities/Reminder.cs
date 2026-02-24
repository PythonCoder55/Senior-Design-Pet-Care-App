using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Senior_Design_Pet_Care_App.Entities
{
    public enum ReminderType
    {
        VetAppointment,
        GroomingAppointment,
        MedicationTime,
        FeedingTime,
        ExerciseTime,
        Other
    }

    public enum RecurrenceType
    {
        None,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public class Reminder
    {
        [Key]
        public int Id { get; set; }

        // foreign key to User.Id
        public int UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Date and time for the reminder
        public DateTime RemindAt { get; set; }

        public ReminderType Type { get; set; } = ReminderType.Other;

        // Links recurring events together so they can be edited/deleted as a group
        public Guid? SeriesId { get; set; }

        // --- NEW: Pet Link ---
        public int? PetId { get; set; }

        [ForeignKey("PetId")]
        public Pet? Pet { get; set; }
    }
}