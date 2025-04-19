using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class ItemTypesAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IItemTypesService _itemTypesService;

        public ItemTypesAPIController(ApplicationDbContext context, IItemTypesService itemTypesService)
        {
            _context = context;
            _itemTypesService = itemTypesService;
        }

		/// <summary>
		/// Returns a list of all item types
		/// </summary>
		/// <returns>
		/// [{ItemType},{ItemType},..]
		/// </returns>
		/// <example>
		/// GET: api/ItemTypesAPI -> 
		/*
        [
  {
    "id": 1,
    "type": "Food",
    "itemXTypes": null
  },
  {
    "id": 2,
    "type": "Beauty",
    "itemXTypes": null
  },
  {
    "id": 3,
    "type": "EditedType",
    "itemXTypes": null
  }
]
		*/
		/// </example>

		// GET: api/ItemTypesAPI

		[HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ItemType>>> GetItemTypes()
        {
            return await _context.ItemTypes.ToListAsync();
        }

		/// <summary>
		/// Returns a single item type
		/// </summary>
		/// <returns>
		/// {ItemType}
		/// </returns>
		/// <example>
		/// GET: api/ItemTypesAPI/1 -> 
		/*
        
  {
    "id": 1,
    "type": "Food",
    "itemXTypes": null
  }
		*/
		/// </example>

		// GET: api/ItemTypesAPI/5
		[HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ItemType>> GetItemType(int id)
        {
            ItemType itemType = await _itemTypesService.GetItemType(id);
            return itemType;
        }




		/// <summary>
		/// Returns a list of types of a given item by id
		/// </summary>
		/// <returns>
		/// [{ItemsTypeDto},{ItemsTypeDto},..]
		/// </returns>
		/// <example>
		/// GET: api/ItemTypesAPI/GetTypesForItem/14 -> 
		/*
        [
  {
    "id": 1,
    "type": "Food"
  },
  {
    "id": 2,
    "type": "Beauty"
  },
  {
    "id": 4,
    "type": "Desert"
  }
]
		*/
		/// </example>
		/// 
		// get types for an item
		[HttpGet("ItemTypesAPI/GetTypesForItem")]
        [Authorize]

        public async Task<IEnumerable<ItemTypeDto>> GetTypesForItem(int itemId)
        {
            IEnumerable<ItemTypeDto> Result = await _itemTypesService.GetTypesForItem(itemId);
            return Result;
        }

		/// <summary>
		/// Create a new Item Type
		/// </summary>
		/// <param name="type">The name of the new item type/param>
		/// <returns>
		/// {ItemType}
		/// </returns>
		/// <example>
		/// api/ItemTypesAPI/CreateItemType/fruit ->
		/*{
			  "id": 11,
			  "type": "fruit",
			  "itemXTypes": null
			}
		 */
		/// </example>
		/// 

		// create new type

		[HttpPost("ItemTypesAPI/CreateItemType/{type}")]
        [Authorize]

        public async Task<ItemType> CreateItemType(string type)
        {
            ItemType Result = await _itemTypesService.CreateItemType(type);
            return Result;


        }

		/// <summary>
		/// Deletes an item type by the given id
		/// </summary>
		/// <param name="id">The id of the item type to delete</param>
		/// <returns>
		/// on success "item type deleted"
		/// on failure "item type not found"
		/// </returns>
		/// <example>
		/// api/ItemTypesAPI/DeleteItemType/5 -> "item succesfully deleted."
		/// </example>
		// delete Item type

		[HttpDelete("ItemTypesAPI/DeleteItemType/{id}")]
        [Authorize]

        public async Task<string> DeleteItemType(int id)
		{
			string Result = await _itemTypesService.DeleteItemType(id);
			return Result;
		}
	}
}