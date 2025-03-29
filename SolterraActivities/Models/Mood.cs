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
}
