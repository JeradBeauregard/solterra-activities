using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Models;

namespace SolterraActivities.Interfaces
{
	public interface IItemTypesService
	{

		// Create
		Task<ItemType> CreateItemType(string type);

		// Read
		Task<IEnumerable<ItemType>> GetItemTypes();
		// get all types for an item
		Task<IEnumerable<ItemTypeDto>> GetTypesForItem(int itemId);
		// get a single type
		Task<ItemType> GetItemType(int id);

		// Update
		Task<ItemType> EditItemType(int id, string type);


		// Delete
		Task<string> DeleteItemType(int id);
	}
}
