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

		public int Health { get; set; }

		public int Strength { get; set; }

		public int Agility { get; set; }

		public int Intelligence { get; set; }

		public int Defence { get; set; }

		public int Hunger { get; set; }

		public string Mood { get; set; }
	}

	// valid pet stats

	public static class PetStats
	{
		public static readonly HashSet<string> ValidStats = new HashSet<string>
	{
		"Health", "Strength", "Agility", "Intelligence", "Defence", "Hunger"
        // Mood could be handled differently since it's a string
    };
	}




	// pet dto for user page

	public class PetDto
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public string UserName { get; set; }
		public string Name { get; set; }
		public int SpeciesId { get; set; }

		public string SpeciesName { get; set; }
		public int Level { get; set; }
		public int Health { get; set; }
		public int Strength { get; set; }
		public int Agility { get; set; }
		public int Intelligence { get; set; }
		public int Defence { get; set; }
		public int Hunger { get; set; }
		public string Mood { get; set; }
	}

}
