using SolterraActivities.Data;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace SolterraActivities.Services
{
	public class ItemTypesService : IItemTypesService
	{
		//database and interface context

		private readonly ApplicationDbContext _context;


		public ItemTypesService(ApplicationDbContext context)
		{
			_context = context;

		}

		//list a single type

		public async Task<ItemType> GetItemType(int id)
		{
			var itemType = await _context.ItemTypes.FindAsync(id);
			if (itemType == null)
			{
				ItemType itemTypeError = new ItemType();
				itemTypeError.Type = "Item type not found";
				return itemTypeError;
			}
			return itemType;
		}

		// list all types
		public async Task<IEnumerable<ItemType>> GetItemTypes()
		{
			return await _context.ItemTypes.ToListAsync();
		}


		// types for an item
		public async Task<IEnumerable<ItemTypeDto>> GetTypesForItem(int itemId)
		{
			List<ItemType> ItemTypes = await _context.ItemTypes
				.Where(it => it.ItemXTypes.Any(ix => ix.ItemId == itemId))
				.ToListAsync();
			List<ItemTypeDto> ItemTypeDtos = new List<ItemTypeDto>();
			foreach (ItemType ItemType in ItemTypes)
			{
				ItemTypeDto itemTypeDto = new ItemTypeDto();
				itemTypeDto.Id = ItemType.Id;
				itemTypeDto.Type = ItemType.Type;
				ItemTypeDtos.Add(itemTypeDto);
			}
			return ItemTypeDtos;
		}

		// create new type

		public async Task<ItemType> CreateItemType(string type)
		{

			ItemType itemType = new ItemType();

			itemType.Type = type;
			_context.ItemTypes.Add(itemType);

			await _context.SaveChangesAsync();

			return itemType;

		}

		// edit existing type

		public async Task<ItemType> EditItemType(int id, string type)
		{
			ItemType itemType = await _context.ItemTypes.FindAsync(id);
			itemType.Type = type;
			await _context.SaveChangesAsync();
			return itemType;



		}

		// delete Item type

		public async Task<string> DeleteItemType(int id)
		{
			ItemType itemType = await _context.ItemTypes.FindAsync(id);
			if (itemType == null)
			{
				return "item type not found";
			}
			_context.ItemTypes.Remove(itemType);
			await _context.SaveChangesAsync();
			return "item type deleted";
		}
	}
}
