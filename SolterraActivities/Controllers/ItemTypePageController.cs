using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Controllers
{
	public class ItemTypePageController : Controller
	{

		// Dependancy Injection

		private readonly IItemTypesService _itemTypeService;
		private readonly IItemService _itemService;

		public ItemTypePageController(IItemTypesService itemTypeService, IItemService itemService)
		{
			_itemTypeService = itemTypeService; // Dependancy Injection: ItemType Service
			_itemService = itemService;
		}


        // List
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> List()
		{

			IEnumerable<ItemType> ItemTypes = await _itemTypeService.GetItemTypes(); // Get ItemTypes from ItemType Service

			return View(ItemTypes);
		}

        // New (Create)
        [HttpGet]
        [Authorize]

        public IActionResult New()
		{
			return View();
		}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string type)
		{
			await _itemTypeService.CreateItemType(type);
			return RedirectToAction("List");
		}

        // Details
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> Details(int Id)
		{

			ItemType itemType = await _itemTypeService.GetItemType(Id);
			IEnumerable<ItemsForTypeDto> items = await _itemService.GetItemsForType(Id);

			// fake data

			IEnumerable<ItemsForTypeDto> itemTest = new List<ItemsForTypeDto>
				{
				new ItemsForTypeDto
				{
					Id = 1,
					Name = "Item 1",
					
				},
				new ItemsForTypeDto
				{
					Id = 2,
					Name = "Item 2",
					
				},
				new ItemsForTypeDto
				{
					Id = 3,
					Name = "Item 3",
					
				}
			};


			ItemTypeDetailsViewModel itemTypes = new ItemTypeDetailsViewModel
			{
				ItemType = itemType,
				Items = items
			};

			return View(itemTypes);
		}

        // Delete
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> ConfirmDelete(int Id)
		{

			ItemType itemType = await _itemTypeService.GetItemType(Id);

			return View(itemType);
		}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int Id)
		{
			await _itemTypeService.DeleteItemType(Id);
			return RedirectToAction("List");
		}
        // Edit
        [HttpGet]
        [Authorize]

        public IActionResult Edit()
		{
			return View();
		}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditItemType(int id, string type)
		{
			await _itemTypeService.EditItemType(id, type);
			return RedirectToAction("List");
		}
	}
}
