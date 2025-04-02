using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolterraActivities.Models
{
	public class ItemxType
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("Item")]
		public int ItemId	{ get; set; }

		[ForeignKey("ItemType")]
		public int TypeId {  get; set; }

		public virtual Item Item { get; set; }  //  Navigation back to Item
		public virtual ItemType ItemType { get; set; }  //  Connects to ItemType
	}
}
