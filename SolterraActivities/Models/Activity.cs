using System.ComponentModel.DataAnnotations;


namespace SolterraActivities.Models
{
    public class Activity
    {
        // activity id
        [Key]
        public int ActivityId { get; set; }

        // activity name
        public string ActivityName { get; set; }

        // hobby id (FK)
        public int HobbyId { get; set; }

        // activity cost in sol shards
        public decimal ActivityCost { get; set; }

        // activity duration in hours
        public decimal DurationInHours { get; set; }

        // date of activity
        public DateTime ActivityDate { get; set; }


        // Activity location
        public string ActivityLocation { get; set; }


        public virtual Hobby Hobby { get; set; }

        public virtual ICollection<ActivityMood> ActivityMoods { get; set; } = new List<ActivityMood>();

        // for images:
        public bool HasImage { get; set; } = false;
        public string? ActivityImagePath { get; set; }

    }


    public class ActivityDto
    {

        // Activity id
        public int ActivityId { get; set; }


        // Activity name 
        public string? ActivityName { get; set; }


        // the hobby name comes from the hobbies table
        public string? HobbyName { get; set; }

        // the hobby id

        public int HobbyId { get; set; }

        // the Activity cost 
        public decimal ActivityCost { get; set; }


        // duration in hours 
        public decimal DurationinHours { get; set; }

        // Activity date

        public DateTime ActivityDate { get; set; }


        // Activity location
        public string ActivityLocation { get; set; }


        // the moods felt during this Activity
        public ICollection<ActivityMood> ActivityMoods { get; set; } = new List<ActivityMood>();


        // for images:
        public bool HasImage { get; set; } = false;
        public string? ActivityImagePath { get; set; }


    }
}
