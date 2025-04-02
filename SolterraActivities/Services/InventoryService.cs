using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolterraActivities.Data;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Services
{
	public class InventoryService : IInventoryService
	{
		private readonly ApplicationDbContext _context;
		private readonly IUserService _userService;

		// Inject ApplicationDbContext
		public InventoryService(ApplicationDbContext context, IUserService userService)
		{
			_context = context;
			_userService = userService;
		}

		// total in existance of an item, in user inventories

		public async Task<int> TotalAmountofItem(int itemId)
		{
			int total = await _context.Inventory
				.Where(i => i.ItemId == itemId)
				.SumAsync(i => i.Quantity);
			return total;
		}

		// list inventory items for a user

		public async Task<IEnumerable<InventoryDto>> ListUserInventory(int userId)
		{


			List<Inventory> Inventories = await _context.Inventory
			.Where(i => i.UserId == userId)
			.Include(i => i.User)
			.Include(i => i.Item)
			.ToListAsync();

			List<InventoryDto> InventoryDtos = new List<InventoryDto>();
			foreach (Inventory Inventory in Inventories)
			{
				InventoryDto inventoryDto = new InventoryDto();
				inventoryDto.Id = Inventory.Id;
				inventoryDto.Quantity = Inventory.Quantity;
				inventoryDto.UserId = Inventory.UserId;
				inventoryDto.Username = Inventory.User.Username;
				inventoryDto.ItemId = Inventory.ItemId;
				inventoryDto.ItemName = Inventory.Item.Name;
				InventoryDtos.Add(inventoryDto);
			}

			return InventoryDtos;

		}

		// list singele inventory entry

		public async Task<InventoryDto> ListInventory(int id)
		{
			Inventory inventory = await _context.Inventory
				.Include(i => i.User)
				.Include(i => i.Item)
				.FirstOrDefaultAsync(i => i.Id == id);

			if (inventory == null)
			{
				InventoryDto inventoryError = new InventoryDto();
				inventoryError.ItemName = "Inventory entry not found";
				return inventoryError;
			}

			InventoryDto inventoryDto = new InventoryDto();
			inventoryDto.Id = inventory.Id;
			inventoryDto.Quantity = inventory.Quantity;
			inventoryDto.UserId = inventory.UserId;
			inventoryDto.Username = inventory.User.Username;
			inventoryDto.ItemId = inventory.ItemId;
			inventoryDto.ItemName = inventory.Item.Name;

			return inventoryDto;

		}

		// list all inventory entries with item information
		public async Task<IEnumerable<InventoryDto>> ListInventories()
		{
			List<Inventory> Inventories = await _context.Inventory
				.Include(i => i.User)
				.Include(i => i.Item)
				.ToListAsync();

			List<InventoryDto> InventoryDtos = new List<InventoryDto>();
			foreach (Inventory Inventory in Inventories)
			{
				InventoryDto inventoryDto = new InventoryDto();
				inventoryDto.Id = Inventory.Id;
				inventoryDto.Quantity = Inventory.Quantity;
				inventoryDto.UserId = Inventory.UserId;
				inventoryDto.Username = Inventory.User.Username;
				inventoryDto.ItemId = Inventory.ItemId;
				inventoryDto.ItemName = Inventory.Item.Name;
				InventoryDtos.Add(inventoryDto);
			}

			// add code to order by user id

			return InventoryDtos.OrderBy(i => i.UserId);
		}


		// add item and update user space(calling user service)

		public async Task<string> AddToInventory(int userId, int itemId, int quantity)
		{


			//Check if the item already exists in the user's inventory
			var inventoryItem = await _context.Inventory
				.FirstOrDefaultAsync(i => i.UserId == userId && i.ItemId == itemId);

			if (inventoryItem != null)
			{
				// If item exists, increase its quantity
				inventoryItem.Quantity += quantity;


				await _context.SaveChangesAsync();
				string userSpaceReturn = await _userService.updateUserSpace(userId, quantity);
				Console.WriteLine(userSpaceReturn);


				// Return the created inventory entry
				return "success, queantity increased";
			}

			else
			{

				// Create new inventory entry
				var NewItem = new Inventory
				{
					UserId = userId,
					ItemId = itemId,
					Quantity = quantity
				};

				// Add to the database
				_context.Inventory.Add(NewItem);
				await _context.SaveChangesAsync();
				string userSpaceReturn = await _userService.updateUserSpace(userId, quantity);
				Console.WriteLine(userSpaceReturn);

				// Return the created inventory entry
				return "success... inventory item added";

				// update to return a service response

			}
		}

		

	

		// edit inventory entry

		public async Task<InventoryDto> EditInventory(int id, int userid, int itemid, int quantity)
		{


			// find inventory entry
			Inventory inventory = await _context.Inventory.FindAsync(id);

			int currentQuantity = inventory.Quantity;


			inventory.UserId = userid;
			inventory.ItemId = itemid;
			inventory.Quantity = quantity;
			await _context.SaveChangesAsync();

			// update user space based on difference in quantity before and after edit

			int quantityChange = quantity - currentQuantity;
			await _userService.updateUserSpace(userid, quantityChange);


			InventoryDto inventoryDto = new InventoryDto();
			inventoryDto.Id = inventory.Id;
			inventoryDto.UserId = inventory.UserId;
			inventoryDto.ItemId = inventory.ItemId;
			inventoryDto.Quantity = inventory.Quantity;


			return inventoryDto;


		}

		// edit quantity of an inventory entry

		public async Task<string> UpdateQuantity(int id, int quantity)
		{
			// find inventory entry
			Inventory inventory = await _context.Inventory.FindAsync(id);
			int currentQuantity = inventory.Quantity;
			inventory.Quantity = quantity;
			await _context.SaveChangesAsync();
			// update user space based on difference in quantity before and after edit
			int quantityChange = quantity - currentQuantity;
			await _userService.updateUserSpace(inventory.UserId, quantityChange);
			// Delete entry if quantity is less than 1
			if(inventory.Quantity < 1)
			{
				await DeleteInventory(id);
			}
			return "quantity updated";
		}


		// delete inventory entry


		// delete an inventory entry by id
		// DELETE: api/InventoriesAPI/5

		public async Task<string> DeleteInventory(int id)
			{
				var inventory = await _context.Inventory.FindAsync(id);
				if (inventory == null)
				{
					return "inventory entry not found";
				}

				await _userService.updateUserSpace(inventory.UserId, -inventory.Quantity);
			_context.Inventory.Remove(inventory);
				await _context.SaveChangesAsync();

				return "entry deleted";
			}
		}


	}

