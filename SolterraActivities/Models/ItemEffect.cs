using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SolterraActivities.Models
{
	public class ItemEffect
	{

		[Key]
		public int Id { get; set; }

		[Required]
		[ForeignKey("Item")]
		public int ItemId { get; set; }

		[Required]
		public string StatToAffect { get; set; }  // "hunger", "intelligence", "mood" etc. cms loads from pet stats

		[Required]
		public int Amount { get; set; }           // Positive or negative integer to affect stat^

		// Navigation property for EF framework LINQ
		public Item Item { get; set; }
	}
}
