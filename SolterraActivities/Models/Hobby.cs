using System.ComponentModel.DataAnnotations;

namespace SolterraActivities.Models
{
    public class Hobby
    {
        // hobby id
        [Key]
        public int HobbyId { get; set; }


        // hobby name
        public required string HobbyName { get; set; }

        // link to activities -- one hobby can be related to many activities
        public virtual ICollection<Activity> Activities { get; set; }

    }

    public class HobbyDto
    {
        // hobby id
        public int HobbyId { get; set; }

        // hobby name comes from the hobbies table
        public string? HobbyName { get; set; }

        // the number of Activities will come from all the Activities that have this hobby id in the Activities table
        public int NumberofActivities { get; set; }

        // the number of hours spent will be summed from all the Activities that have this hobby id in the Activities table
        public decimal HoursSpent { get; set; }

        // the typical moods will be a list of moods that that are felt during Activities that have this hobby id in the Activities table that appear 3 or more times
        public List<string>? TypicalMoods { get; set; }
    }
}
