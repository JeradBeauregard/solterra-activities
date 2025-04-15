using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Controllers
{
	public class ItemPageController : Controller
	{
		// Dependancy Injection
		private readonly IItemService _itemService;
		private readonly IItemTypesService _itemTypesService;
		private readonly IInventoryService _inventoryService;
		private readonly IItemEffectService _itemEffectService;
		public ItemPageController(IItemService itemService, IItemTypesService itemTypesService, IInventoryService inventoryService, 
			IItemEffectService itemEffectService)
		{
			_itemService = itemService; // Dependancy Injection: Item Service
			_itemTypesService = itemTypesService;
			_inventoryService = inventoryService;
			_itemEffectService = itemEffectService;
		}



		// List

		public async Task<IActionResult> List()
		{

			IEnumerable<Item> Items = await _itemService.GetItems(); // Get Items from Item Service

			return View(Items);
		}

		// New (Create)

		public IActionResult New()
		{
			return View();
		}

		public async Task<IActionResult> Create(string name, string description, int value)
		{
			await _itemService.CreateItem(name, description, value);
			return RedirectToAction("List");

		}


		// Details

		public async Task<IActionResult> Details(int Id)
		{
			ItemDto item = await _itemService.GetItem(Id);

			IEnumerable<ItemTypeDto> itemTypes = await _itemTypesService.GetTypesForItem(Id);

			IEnumerable<ItemType> allTypes = await _itemTypesService.GetItemTypes();

			IEnumerable<UserByItemDto> users = await _itemService.ListUsersByItem(Id);

			IEnumerable<ItemEffect> effects = await _itemEffectService.GetItemEffects(Id);

			HashSet<string> validStats = new HashSet<string>(PetStats.ValidStats);


			int TotalItems = await _inventoryService.TotalAmountofItem(Id);

			ItemDetailsViewModel itemDetails = new ItemDetailsViewModel
			{
				Item = item,
				ItemTypes = itemTypes,
				AllItemTypes = allTypes,
				UserByItem = users,
				Effects = effects,
				ValidStats = validStats,
				TotalAmount = TotalItems
			};

			return View(itemDetails);
		}

		// switch isConsumable to true or false

		public async Task<IActionResult> SwitchIsConsumable(int itemId)
		{
			await _itemService.SwitchIsConsumable(itemId);
			return RedirectToAction("Details", new { id = itemId });
		}

		// item effects

		public async Task<IActionResult> AddItemEffect(int itemId, string statToAffect, int amount)
		{
			await _itemEffectService.AddItemEffect(itemId,statToAffect,amount);
			return RedirectToAction("Details", new { id = itemId });
		}
		public async Task<IActionResult> UpdateItemEffect(int id, int itemId, string statToAffect, int amount)
		{
			await _itemEffectService.UpdateItemEffect(id, itemId, statToAffect, amount);
			return RedirectToAction("Details", new { id = itemId });
		}
		public async Task<IActionResult> DeleteItemEffect(int effectId ,int itemId)
		{
			await _itemEffectService.DeleteItemEffect(effectId);
			return RedirectToAction("Details", new { id = itemId });
		}
		//<a href="/ItemPage/DeleteItemEffect?effectId=@effect.Id&itemId=@item.Id">Delete</a>




		// link and unlink

		public async Task<IActionResult> LinkItemToType(int itemId, int typeId)
		{
			await _itemService.LinkItemToType(itemId, typeId);
			return RedirectToAction("Details", new { id = itemId });
		}

		public async Task<IActionResult> UnlinkItemFromType(int itemId, int typeId)
		{
			await _itemService.UnlinkItemToType(itemId, typeId);
			return RedirectToAction("Details", new { id = itemId });
		}

		// Delete

		public async Task<IActionResult> ConfirmDelete(int Id)
		{
			ItemDto item = await _itemService.GetItem(Id);
			return View(item);
			
		}

		public async Task<IActionResult> Delete(int Id)
		{
			await _itemService.DeleteItem(Id);
			return RedirectToAction("List");
		}
		// Edit

		

		public async Task<IActionResult> Edit(int id)
		{

			ItemDto item = await _itemService.GetItem(id);
			return View(item);
		}

		public async Task<IActionResult> EditItem(int id, string name, string description, int value,IFormFile image )
		{
			await _itemService.EditItem(id, name, description, value);
			await _itemService.AddItemImage(id, image);
			return RedirectToAction("List");
		}
	}
}
