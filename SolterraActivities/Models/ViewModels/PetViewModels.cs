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

	}
}
