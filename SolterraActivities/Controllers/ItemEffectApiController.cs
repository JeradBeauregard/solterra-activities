using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolterraActivities.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ItemEffectApiController : ControllerBase
	{
		private readonly IItemEffectService _itemEffectService;

		public ItemEffectApiController(IItemEffectService itemEffectService)
		{
			_itemEffectService = itemEffectService;
		}

		/// <summary>
		/// Get all item effects for a specific item.
		/// </summary>
		/// <param name="itemId">Item ID</param>
		/// <returns>A list of item effects for the specified item.</returns>
		[HttpGet("GetItemEffects/{itemId}")]
		public async Task<IActionResult> GetItemEffects(int itemId)
		{
			var itemEffects = await _itemEffectService.GetItemEffects(itemId);
			return Ok(itemEffects);
		}

		/// <summary>
		/// Add an effect to an item.
		/// </summary>
		/// <param name="itemId">Item ID</param>
		/// <param name="statToAffect">Stat to affect (e.g. hunger)</param>
		/// <param name="amount">Amount of effect</param>
		/// <returns>Status message</returns>
		[HttpPost("AddItemEffect/{itemId}")]
		public async Task<IActionResult> AddItemEffect(int itemId, string statToAffect, int amount)
		{
			var result = await _itemEffectService.AddItemEffect(itemId, statToAffect, amount);
			return Ok(result);
		}

		/// <summary>
		/// Update an item effect.
		/// </summary>
		/// <param name="itemEffectId">ItemEffect ID</param>
		/// <param name="itemId">Item ID</param>
		/// <param name="statToAffect">Stat to affect</param>
		/// <param name="amount">Amount</param>
		/// <returns>Status message</returns>
		[HttpPut("UpdateItemEffect/{itemEffectId}")]
		public async Task<IActionResult> UpdateItemEffect(int itemEffectId, int itemId, string statToAffect, int amount)
		{
			var result = await _itemEffectService.UpdateItemEffect(itemEffectId, itemId, statToAffect, amount);
			return Ok(result);
		}

		/// <summary>
		/// Delete an item effect.
		/// </summary>
		/// <param name="itemEffectId">ItemEffect ID</param>
		/// <returns>Status message</returns>
		[HttpDelete("DeleteItemEffect/{itemEffectId}")]
		public async Task<IActionResult> DeleteItemEffect(int itemEffectId)
		{
			var result = await _itemEffectService.DeleteItemEffect(itemEffectId);
			return Ok(result);
		}
	}
}
