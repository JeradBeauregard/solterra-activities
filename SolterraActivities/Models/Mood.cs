using System.ComponentModel.DataAnnotations;

namespace SolterraActivities.Models
{
    public class Mood
    {

        // mood id
        [Key]
        public int MoodId { get; set; }

        // mood name
        public required string MoodName { get; set; }
    }



    // mood dto

    public class MoodDto
    {
        // mood id
        public int MoodId { get; set; }

        // mood name
        public string? MoodName { get; set; }

        

        // count of activities that have this mood recorded
        public int MoodActivityCount { get; set; }
    }


}
