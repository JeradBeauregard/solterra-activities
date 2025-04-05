using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SolterraActivities.Models.ViewModels
{
	public class PetViewModels
	{
		public class PetNew
		{
			


			public int UserId { get; set; }

			public string UserName { get; set; }

			public List<Species> Species { get; set; }

			

			public List<MoodDto> Moods { get; set; }
		}

		public class PetEdit
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public int UserId { get; set; }
			public int SpeciesId { get; set; }
			public int Level { get; set; }
			public int Health { get; set; }
			public int Strength { get; set; }
			public int Agility { get; set; }
			public int Intelligence { get; set; }
			public int Defence { get; set; }
			public int Hunger { get; set; }
			public string Mood { get; set; }

			public List<Species> Species { get; set; }
			public List<MoodDto> Moods { get; set; }
		}

		public class  PetDetails
		{
			public int Id { get; set; }
			public string Name { get; set; }
			public int UserId { get; set; }
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
}
