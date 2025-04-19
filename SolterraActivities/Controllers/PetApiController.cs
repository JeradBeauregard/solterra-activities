using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Models;
using SolterraActivities.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SolterraActivities.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PetApiController : Controller
	{
		
		// dependencies

		private readonly IPetService _petService;
		public PetApiController(IPetService petService)
		{
			_petService = petService;
		}

		/// <summary>
		/// Returns a list of all Pets
		/// </summary>
		/// <example>
		/// GET: Api/PetApi/List -> [{Pet},{Pet},..]
		/// </example>
		/// <returns>A List of pet objects</returns>
		[HttpGet("List")]
        [Authorize]
        public async Task<IEnumerable<Pet>> ListPets()
		{
			return await _petService.ListPets();
		}
		public async Task<Pet> ListPet(int id)
		{
			return await _petService.ListPet(id);
		}

		/// <summary>
		/// Retuns a list of all pets for a user
		/// </summary>
		/// <param name="userId">The id of a user</param>
		/// <example>
		/// GET: Api/PetApi/ListUserPets -> [{Pet},{Pet},..]
		/// </example>
		/// <returns>A list of pet objects</returns>
		/// 
		[HttpGet("ListUserPets")]
        [Authorize]
        public async Task<IEnumerable<PetDto>> ListUserPets(int userId)
		{
			return await _petService.ListUserPets(userId);
		}

		//create

		/// <summary>
		/// Create a pet Object for admin
		/// <example>
		/// POST: Api/PetApi/CreatePetAdmin -> {Pet}
		/// </example>
		/// Creates a new pet object with the given parameters.
		/// Returns created pet object.
		/// for admin use, can manipulate all stats
		/// </summary>
		[HttpPost("CreatePetAdmin")]
        [Authorize]
        public async Task<Pet> CreatePetAdmin(string name, int userId, int species_id, int level, int health, int strength, int agility, int intelligence, int defence, int hunger, string mood)
		{
			return await _petService.CreatePetAdmin(name, userId, species_id, level, health, strength, agility, intelligence, defence, hunger, mood);

		}


		/// <summary>
		/// Create a pet object for user
		/// </summary>
		/// <example>
		/// POST: Api/PetApi/CreatePetUser -> {Pet}
		/// </example>
		/// <param name="name"></param>
		/// <param name="userId"></param>
		/// <param name="species_id"></param>
		/// <returns>The created pet object</returns>
		[HttpPost("CreatePetUser")]
        [Authorize]
        public async Task<Pet> CreatePetUser(string name, int userId, int species_id)
		{
			return await _petService.CreatePetUser(name, userId, species_id);
		}
		//update

		/// <summary>
		/// Update a pet object for admin
		/// <example>
		/// POST: Api/PetApi/UpdatePetAdmin -> {Pet}
		/// </example>
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="userId"></param>
		/// <param name="species_id"></param>
		/// <param name="level"></param>
		/// <param name="health"></param>
		/// <param name="strength"></param>
		/// <param name="agility"></param>
		/// <param name="intelligence"></param>
		/// <param name="defence"></param>
		/// <param name="hunger"></param>
		/// <param name="mood"></param>
		/// <returns>Returns updated pet object</returns>
		[HttpPut("UpdatePetAdmin")]
        [Authorize]
        public async Task<Pet> UpdatePetAdmin(int id, string name, int userId, int species_id, int level, int health, int strength, int agility, int intelligence, int defence, int hunger, string mood)
		{
			return await _petService.UpdatePetAdmin(id, name, userId, species_id, level, health, strength, agility, intelligence, defence, hunger, mood);
		}

		//delete

		/// <summary>
		/// Delete a pet object
		/// </summary>
		/// <example>
		/// DELETE: Api/PetApi/DeletePet -> {string}
		/// </example>
		/// <param name="id"></param>
		/// <returns>Returns deletion status message</returns>
		[HttpDelete("DeletePet")]
        [Authorize]
        public async Task<string> DeletePet(int id)
		{
			var pet = await _petService.ListPet(id);
			if (pet == null)
			{
				return "Pet not found";
			}
			else
			{
				return await _petService.DeletePet(id);
			}
		}
	}
}
