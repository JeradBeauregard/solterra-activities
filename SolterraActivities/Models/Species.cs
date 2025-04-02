using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolterraActivities.Models
{
	public class Species
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
	}
}
