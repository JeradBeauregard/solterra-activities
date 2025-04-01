using System.ComponentModel.DataAnnotations;

namespace SolterraActivities.Models
{
    public class UserActivity
    {
        // an instance of an activity that a user has participated in
        [Key] public int UserActivityId { get; set; }

        // the user who participated in the activity
        public required virtual User? User { get; set; }
        public int UserId { get; set; }

        // the activity that the user participated in
        public required virtual Activity? Activity { get; set; }
        public int ActivityId { get; set; }

        // the pet that the user brought with them to the activity
        public required virtual Pet? Pet { get; set; }
        public int PetId { get; set; }

        // participating in activities will allow users to gain an item
        // this is the item that the user gained from the activity
        public required virtual Item? Item { get; set; }
        public int ItemId { get; set; }


    }



    public class UserActivityDto
    {
        // Primary Key
        public int UserActivityId { get; set; }

        // Foreign Keys
        public int UserId { get; set; }
        public int ActivityId { get; set; }
        public int PetId { get; set; }
        public int ItemId { get; set; }

        // Display Information
        public string? Username { get; set; }

        public string? ActivityName { get; set; }
        public DateTime? ActivityDate { get; set; }
        public decimal? ActivityCost { get; set; }
        public decimal? ActivityDurationInHours { get; set; }

        public string? PetName { get; set; }

        // Moods tied to the activity (shown as pet's mood after activity)
        public string? PetMoodAfterActivity { get; set; }

        public string? ItemGained { get; set; }
    }
}
