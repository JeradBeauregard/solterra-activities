using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Controllers
{
	public class UserPageController : Controller
	{

		// Dependancy Injection	
		private readonly IUserService _userService;
		private readonly IInventoryService _inventoryService;
		private readonly IItemService _itemService;

		public UserPageController(IUserService userService, IInventoryService inventoryService, IItemService itemService)
		{
			_userService = userService; // Dependancy Injection: User Service
			_inventoryService = inventoryService;
			_itemService = itemService;
		}

		// List

		public async Task<IActionResult> List()
		{

			IEnumerable<User> Users = await _userService.GetUsers(); // Get Users from User Service
			return View(Users);
		}

		// New (Create)

		public IActionResult New()
		{
			return View();
		}

		public async Task<IActionResult> Create(string username, string password)
		{
			await _userService.PostUser(username, password);
			return RedirectToAction("List");
		}

		// Add to user inventory

		public async Task<IActionResult> AddToInventory(int userId, int itemId, int quantity)
		{
			string result = await _inventoryService.AddToInventory(userId, itemId, quantity);
			return RedirectToAction("Details", new { id = userId });
		}

		// Details

		public async  Task<IActionResult> Details(int id)
		{
			User user = await _userService.GetUser(id);
			IEnumerable<InventoryDto> inventory = await _inventoryService.ListUserInventory(id);
			IEnumerable<Item> items = await  _itemService.GetItems();

			// Order items alphabetically by name
			items = items.OrderBy(i => i.Name);

			UserDetailsViewModel userDetails = new UserDetailsViewModel
			{
				User = user,
				Inventory = inventory,
				AllItems = items
			};

			return View(userDetails);
		}

		// Delete

		public async Task<IActionResult> ConfirmDelete(int Id)
		{
			
			User user = await _userService.GetUser(Id);

			return View(user);
		}

		public async Task<IActionResult> Delete(int Id)
		{
			await _userService.DeleteUser(Id);
			return RedirectToAction("List");
		}

		// Edit

		public async Task<IActionResult> Edit(int id)
		{
			User user = await _userService.GetUser(id);
			return View(user);
		}

		public async Task<IActionResult> EditUser(int id, string username, string password,int solshards)
		{
			await _userService.EditUser(id, username, password, solshards);
			return RedirectToAction("Details", new { id = id });
		}

		public async Task<IActionResult> UpdateQuantity(int id, int quantity)
		{
			await _inventoryService.UpdateQuantity(id, quantity);
			InventoryDto UserId = await _inventoryService.ListInventory(id);

			return RedirectToAction("Details", new { id = UserId.UserId});
		}
	}
}
