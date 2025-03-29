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
}
