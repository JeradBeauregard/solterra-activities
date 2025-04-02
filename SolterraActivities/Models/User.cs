using System.ComponentModel.DataAnnotations;

namespace SolterraActivities.Models
{
	public class User
	{
		[Key]
		public int Id { get; set; }

		public string Username { get; set; }

		public string Password { get; set; }

		public int InventorySpace { get; set; }

		public int SolShards { get; set; }
	}

	public class CreateUserDto
	{
		public int UserId { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public int InventorySpace { get; set; }
		public int SolShards { get; set; }
	}



	public class UserDetailsViewModel
	{
		public User User { get; set; }
		public IEnumerable<InventoryDto> Inventory { get; set; }

		public IEnumerable<Item> AllItems { get; set; }
	}
}

