using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Models;

namespace SolterraActivities.Interfaces
{
	public interface IInventoryService
	{
		// Read

		// list user inventory
		//Task<IEnumerable<CategoryDto>> ListCategories();
		Task<IEnumerable<InventoryDto>> ListUserInventory(int userId);
		Task<IEnumerable<InventoryDto>> ListInventories();
		Task<InventoryDto> ListInventory(int id);
		Task<int> TotalAmountofItem(int itemId);

		// Create
		// add item to inventory
		Task<string> AddToInventory(int userId, int itemId, int quantity);

		//Update
		Task<InventoryDto>EditInventory(int id, int userId, int itemId, int quantity);

		Task<string> UpdateQuantity(int id, int quantity);


			//Delete
			// Delete inventory row/entry
			Task<string> DeleteInventory(int id);

	}
}
