using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolterraActivities.Data;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Services
{
	public class ItemEffectService : IItemEffectService
	{

		// dependencies

		private readonly ApplicationDbContext _context;
		public ItemEffectService(ApplicationDbContext context)
		{
			_context = context;
		}

		// get all item effects for a specific item

		public async Task<ICollection<ItemEffect>> GetItemEffects(int itemId)
		{
			var item = await _context.Items
				.Include(i => i.Effects)
				.FirstOrDefaultAsync(i => i.Id == itemId);
			if (item == null)
			{
				return null;
			}
			return item.Effects;
		}

		// add an effect to an item

		public async Task<string> AddItemEffect(int itemId, string statToAffect, int amount)
		{
			//  Validate stat first
			if (!PetStats.ValidStats.Contains(statToAffect))
			{
				return $"Invalid stat '{statToAffect}'. Must be one of: {string.Join(", ", PetStats.ValidStats)}";
			}
			// get item 
			var item = await _context.Items
				.Include(i => i.Effects)
				.FirstOrDefaultAsync(i => i.Id == itemId);
			if (item == null)
			{
				return "Item not found";
			}
			var effect = new ItemEffect
			{
				ItemId = itemId,
				StatToAffect = statToAffect,
				Amount = amount
			};
			item.Effects.Add(effect);
			await _context.SaveChangesAsync();
			return "success item effect added";
		}

		// update an effect for an item

		public async Task<string> UpdateItemEffect(int id, int itemId, string statToAffect, int amount)
		{
			var itemEffect = await _context.ItemEffects
				.FirstOrDefaultAsync(i => i.Id == id);
			if (itemEffect == null)
			{
				return "Item effect not found";
			}
			itemEffect.ItemId = itemId;
			itemEffect.StatToAffect = statToAffect;
			itemEffect.Amount = amount;
			await _context.SaveChangesAsync();
			return "success item effect updated";
		}

		// delete an effect for an item

		public async Task<string> DeleteItemEffect(int id)
		{
			var itemEffect = await _context.ItemEffects
				.FirstOrDefaultAsync(i => i.Id == id);
			if (itemEffect == null)
			{
				return "Item effect not found";
			}
			_context.ItemEffects.Remove(itemEffect);
			await _context.SaveChangesAsync();
			return "success item effect deleted";
		}


	}
}
