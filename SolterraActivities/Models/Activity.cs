using System.ComponentModel.DataAnnotations;


namespace SolterraActivities.Models
{
    public class Activity
    {
        // activity id
        [Key]
        public int ActivityId { get; set; }

        // activity name
        public int ActivityName { get; set; }

        // hobby id (FK)
        public int HobbyId { get; set; }

        // activity cost in sol shards
        public int ActivityCost { get; set; }

        // activity duration in hours
        public decimal DurationInHours { get; set; }

        // date of activity
        public DateTime ActivityDate { get; set; }

        public virtual Hobby Hobby { get; set; }

        public virtual ICollection<ActivityMood> ActivityMoods { get; set; } = new List<ActivityMood>();

        // images of the activity
        public bool HasImage { get; set; } = false;
        public string ImagePath { get; set; }


    }
}
