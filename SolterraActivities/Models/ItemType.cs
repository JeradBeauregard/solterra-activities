using System.ComponentModel.DataAnnotations;

namespace SolterraActivities.Models
{
	public class ItemType
	{
		[Key]
		public int Id { get; set; }

		public string Type {  get; set; }

		// Back-reference for Many-to-Many Relationship
		public virtual List<ItemxType> ItemXTypes { get; set; }
	}

	public class ItemTypeDto
	{
		public int Id { get; set; }
		public string Type { get; set; }
	}

	public class ItemTypeDetailsViewModel
	{
		public ItemType ItemType { get; set; }
		public IEnumerable<ItemsForTypeDto> Items { get; set; }
	}


}
