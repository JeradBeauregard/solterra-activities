using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SolterraActivities.Models;

namespace SolterraActivities.Interfaces
{
	public interface IItemEffectService
	{

		/// <summary>Get all item effects for a specific item.</summary>
		/// <param name="itemId">Item Id</param>
		/// <returns>All item effects for the item with the specified id.</returns>
		Task<ICollection<ItemEffect>> GetItemEffects(int itemId);
		/// <summary>Add and effect to an item</summary>
		/// <param name="itemId">Item Id</param>
		/// <param name="statToAffect">Stat to affect</param>
		/// <param name="amount">Amount to affect</param>
		/// <returns>String "success item effect added" if the effect was added successfully.</returns>
		Task<string> AddItemEffect(int itemId, string statToAffect, int amount);
		/// <summary>Update an effect for an item</summary>
		Task<string> UpdateItemEffect(int id, int itemId, string statToAffect, int amount);
		Task<string> DeleteItemEffect(int id);
	}
}
