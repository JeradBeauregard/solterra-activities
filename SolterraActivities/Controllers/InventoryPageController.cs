using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using SolterraActivities.Services;

namespace testapp.Controllers
{
	public class InventoryPageController : Controller
	{

		// Dependancy Injection

		private readonly IInventoryService _inventoryService;
		private readonly IUserService _UserService;
		private readonly IItemService _ItemService;

		public InventoryPageController(IInventoryService inventoryService, IUserService userService, IItemService itemService)
		{
			_inventoryService = inventoryService; // Dependancy Injection: Inventory Service
			_UserService = userService;
			_ItemService = itemService;
		}

		// List

		public async Task<IActionResult> List()
		{

			IEnumerable<InventoryDto> Inventories = await _inventoryService.ListInventories(); // Get Inventorys from Inventory Service

			return View(Inventories);
		}

		// New (Create)

		public async Task<IActionResult> New()
		{
			IEnumerable<User> Users = await _UserService.GetUsers(); // Get Users from Inventory Service
			IEnumerable<Item> Items = await _ItemService.GetItems(); // Get Items from Inventory Service

			InventoryNewViewModel inventoryNewViewModel = new InventoryNewViewModel
			{
				Users = Users,
				Items = Items
			};

			return View(inventoryNewViewModel);
		}


		public async Task<IActionResult> Create(int userId, int itemId, int quantity)
		{
			string result = await _inventoryService.AddToInventory(userId, itemId, quantity);
			return RedirectToAction("List");
		}
		// Details

		public async Task<IActionResult> Details(int id)
		{

			InventoryDto inventory = await _inventoryService.ListInventory(id); // Get Inventory from Inventory Service
			return View(inventory);
		}

		// Delete

		public async Task<IActionResult> ConfirmDelete(int Id)
		{

			InventoryDto inventory = await _inventoryService.ListInventory(Id); // Get Inventory from Inventory Service
			return View(inventory);
		}

		public async Task<IActionResult> Delete(int Id)
		{
			string result = await _inventoryService.DeleteInventory(Id);
			return RedirectToAction("List");
		}

		public async Task<IActionResult> DeleteInventoryFromUserDetails(int Id, int UserId)
		{
			await _inventoryService.DeleteInventory(Id);
			return RedirectToAction("Details","UserPage", new { id = UserId });
		}

		// Edit

		public IActionResult Edit()
		{
			return View();
		}

		public async Task<IActionResult> EditInventory(int id, int userId, int itemId, int quantity)
		{
			await _inventoryService.EditInventory(id, userId, itemId, quantity);
			return RedirectToAction("List");


		}

		public async Task<IActionResult> UpdateQuantity(int id, int quantity)
		{
			await _inventoryService.UpdateQuantity(id, quantity);
			return RedirectToAction("List");
		}
	}
}
