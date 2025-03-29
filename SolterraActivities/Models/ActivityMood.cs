using System.ComponentModel.DataAnnotations;

namespace SolterraActivities.Models
{
    public class ActivityMood
    {
        // an individual occurence of a mood felt during an activity
        // multiple moods can be felt during an activity
        [Key] public int ActivityMoodId { get; set; }


        // an activity where a mood is felt
        public required virtual Activity? Activity { get; set; }
        public int ActivityId { get; set; }


        // a mood felt during an activity
        public required virtual Mood? Mood { get; set; }
        public int MoodId { get; set; }


        // intensity of this mood before the activity
        public int MoodIntensityBefore { get; set; }

        // intensity of this mood after the activity
        public int MoodIntensityAfter { get; set; }
    }
}
