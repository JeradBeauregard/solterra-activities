using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolterraActivities.Models
{
	public class Inventory
	{
		[Key]
		public int Id { get; set; }

		public int Quantity { get; set; }

		[ForeignKey("Users")]
		public int UserId { get; set; }

		[ForeignKey("Items")]
		public int ItemId { get; set; }

		public virtual User User { get; set; }

		public virtual Item Item { get; set; }

	}

	public class InventoryDto
	{

		public int Id { get; set; }

		public int Quantity { get; set; }

		public int UserId { get; set; }

		public string Username	{ get; set; }

		public int ItemId { get; set; }

		public string ItemName { get; set; }


	}

	public class InventoryNewViewModel
	{
		public IEnumerable<User> Users { get; set; }

		public IEnumerable<Item> Items { get; set; }
	}

}
