using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolterraActivities.Models
{
    public class Pet
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }

        [ForeignKey("Species")]
        public int SpeciesId { get; set; }

        public int Level { get; set; }

        public int Health   { get; set; }

        public int Strength { get; set; }

        public int Agility { get; set; }

        public int Intelligence { get; set; }

        public int Defence { get; set; }

        public int Hunger { get; set; }

        public string Mood { get; set; }
	}
}
