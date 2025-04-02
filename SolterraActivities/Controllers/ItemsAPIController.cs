using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolterraActivities.Data;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemService _itemService;

        public ItemsAPIController(ApplicationDbContext context, IItemService itemService)
        {
            _context = context;
            _itemService = itemService;
        }

		/// <summary>
		/// Returns a list of all items
		/// </summary>
		/// <returns>
		/// [{Item},{Item},..]
		/// </returns>
		/// <example>
		/// GET: api/ItemsAPI -> 
		/*
	[
  {
    "id": 1,
    "name": "Blue Rasperberry Desert",
    "description": "A delicious sundae made with blue raspberries",
    "value": 450,
    "hasPic": false,
    "picPath": null,
    "itemxType": null
  },
  {
    "id": 2,
    "name": "Golden Hairbrush",
    "description": "A hairbrush made for on the finest of hair",
    "value": 1200,
    "hasPic": false,
    "picPath": null,
    "itemxType": null
  },
  {
    "id": 3,
    "name": "Turkey Leg",
    "description": "Yum!",
    "value": 250,
    "hasPic": false,
    "picPath": null,
    "itemxType": null
  }]
		*/
		/// </example>

		// get items
		// GET: api/ItemsAPI
		[HttpGet]
        public async Task<IEnumerable<Item>> GetItems()
        {
            return await _itemService.GetItems();
        }

		/// <summary>
		/// Returns a list of all items with their types
		/// </summary>
		/// <returns>
		/// [{ItemDto},{ItemDto},..]
		/// </returns>
		/// <example>
		/// GET: api/ItemsAPI/withtypes -> 
		/*
		         [
          {
            "id": 1,
            "name": "Blue Rasperberry Desert",
            "types": [
              "Food",
              "Desert"
            ]
          },
          {
            "id": 2,
            "name": "Golden Hairbrush",
            "types": [
              "Beauty",
              "Luxury"
            ]
          },
          {
            "id": 3,
            "name": "Turkey Leg",
            "types": [
              "Food"
            ]
          }
        ]
		*/
		/// </example>
		[HttpGet("WithTypes")]

        public async Task<IEnumerable<ItemWithTypesDto>> GetItemsWithTypes()
		{
            IEnumerable<ItemWithTypesDto> results = await _itemService.GetItemsWithTypes();

            return results;
        }

		/// <summary>
		/// Returns a list of items of a given type by typeId
		/// </summary>
		/// <returns>
		/// [{ItemsForTypeDto},{ItemsForTypeDto},..]
		/// </returns>
		/// <example>
		/// GET: api/ItemsAPI/GetItemsForType/1 -> 
		/*
		[
  {
    "id": 1,
    "name": "Blue Rasperberry Desert"
  },
  {
    "id": 3,
    "name": "Turkey Leg"
  }
]
		*/
		/// </example>

		// returns all items of a given type

		[HttpGet("ItemsAPI/GetItemsForType")]

		public async Task<IEnumerable<ItemsForTypeDto>> GetItemsForType(int typeId)
        {
			IEnumerable<ItemsForTypeDto> results = await _itemService.GetItemsForType(typeId);
			return results;
		}

		/// <summary>
		/// Returns an item by item id
		/// </summary>
		/// <returns>
		/// {ItemDto}
		/// </returns>
		/// <example>
		/// GET: api/ItemsAPI/5
		/*
		on success:

        {
  "id": 2,
  "name": "Golden Hairbrush",
  "description": "A hairbrush made for on the finest of hair",
  "value": 1200,
  "picPath": null,
  "itemxType": null
}

        on failure:

        {
  "id": 0,
  "name": "Item not found, this is an error message. NOT a real item",
  "description": null,
  "value": 0,
  "picPath": null,
  "itemxType": null
}

		*/
		/// </example>

		// GET: api/ItemsAPI/5
		[HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(int id)
        {
            ItemDto item = await _itemService.GetItem(id);
            return item;
        }

		/// <summary>
		/// Deletes an item by the given id
		/// </summary>
		/// <param name="id">The id of the item to delete</param>
		/// <returns>
		/// on success "item succesfully deleted."
		/// on failure "item to be deleted not found"
		/// </returns>
		/// <example>
		/// api/ItemsAPI/5 -> "item succesfully deleted."
		/// </example>

		// DELETE: api/ItemsAPI/5
		[HttpDelete("{id}")]
        public async Task<string> DeleteItem(int id)
        {
           string result = await _itemService.DeleteItem(id);
            return result;
        }

		/// <summary>
		/// links an item to a type (adds link to bridge table itemsxtypes)
		/// </summary>
		/// <param name="itemId">The id of the item to link</param></param>
		/// <param name="typeId">The id of the type to link to</param>"
		/// <returns>
		/// on success "item linked to type"
		/// on failure "item and item type not found" or "item not found" or "item type not found"
		/// </returns>
		/// <example>
		/// api/ItemsAPI/LinkItemToType/5/2 -> "item linked to type"
		/// </example>


		[HttpPost("ItemsAPI/LinkItemToType/{itemId}/{typeId}")]
        public async Task<string> LinkItemToType(int itemId, int typeId)
		{
			string result = await _itemService.LinkItemToType(itemId, typeId);
			return result;
		}

		/// <summary>
		/// unlinks an item from a type (deleted link on bridge table itemsxtypes)
		/// </summary>
		/// <param name="itemId">The id of the item to unlink</param></param>
		/// <param name="typeId">The id of the type to unlink to</param>"
		/// <returns>
		/// on success "item linked to type"
		/// on failure "item and item type not found" or "item not found" or "item type not found" or "item not linked to type"
		/// </returns>
		/// <example>
		/// api/ItemsAPI/UnLinkItemToType/5/2 -> "item unlinked from type"
		/// </example>

		[HttpPost("ItemsAPI/UnlinkItemToType/{itemId}/{typeId}")]
		public async Task<string> UnlinkItemToType(int itemId, int typeId)
		{
			string result = await _itemService.UnlinkItemToType(itemId, typeId);
			return result;
		}

		/// <summary>
		/// creates a new item
		/// </summary>
		/// <param name="name">The name of the new item</param>
		/// <param name="description">The description of the new item</param>
		/// <param name="value">The value of the new item</param>
		/// <returns>
		/// [{CreateItemDto}]
		/// </returns>
		/// <example>
		/// api/ItemsAPI/CreateItem/Raspberry/berry/200 ->{
		///"id": 14,
		///"name": "raspberry",
		/// "description": "berry",
		/// "value": 200
		///}
		/// 
		/// </example>
		/// 
		[HttpPost("ItemsAPI/CreateItem/{name}/{description}/{value}")]

		public async Task<CreateItemDto> CreateItem(string name, string description, int value)
        {

            CreateItemDto result = await _itemService.CreateItem(name, description, value);
            
			return result;
		}

		/// <summary>
		/// edit an existing item
		/// </summary>
		/// <param name="id">The id of the item to edit</param>
		/// <param name="name">The name of the new item</param>
		/// <param name="description">The description of the new item</param>
		/// <param name="value">The value of the new item</param>
		/// <returns>
		/// [{CreateItemDto}]
		/// </returns>
		/// <example>
		/// api/ItemsAPI/EditItem/Raspberry/berrygood/250 ->{
		///"id": 14,
		///"name": "raspberry",
		/// "description": "berrygood",
		/// "value": 250
		///}
		/// 
		/// </example>
		/// 
		[HttpPost("ItemsAPI/EditItem")]

		public async Task<CreateItemDto> EditItem(int id, string name, string description, int value)
		{
			CreateItemDto result = await _itemService.EditItem(id, name, description, value);
			return result;
		}


	}
}
