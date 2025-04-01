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

    public class ActivityMoodDto


    {
        // Activitymood id
        public int ActivityMoodId { get; set; }

        // Activity id
        public int ActivityId { get; set; }


        // mood id
        public int MoodId { get; set; }


        // Activity name comes from the Activitys table
        public string ActivityName { get; set; }


        // mood name comes from the moods table
        public string MoodName { get; set; }

        // date of Activity and mood log comes from the Activitys table
        public DateTime ActivityDate { get; set; }

        // mood intensity before the Activity
        public int MoodIntensityBefore { get; set; }


        // mood intensity after the Activity
        public int MoodIntensityAfter { get; set; }
    }
}
