
using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Models;
using SolterraActivities.Services;

namespace SolterraActivities.Interfaces
{
	public interface IPetService
	{
		// read
		Task<IEnumerable<Pet>> ListPets();
		Task<Pet> ListPet(int id);

		Task<IEnumerable<PetDto>> ListUserPets(int userId);
		//create

		Task<Pet> CreatePetAdmin(string name, int userId, int species_id, int level, int health, int strength, int agility, int intelligence, int defence, int hunger, string mood);
		Task<Pet> CreatePetUser(string name, int userId, int species_id);
		//update
		Task<Pet> UpdatePetAdmin(int id, string name, int userId, int species_id, int level, int health, int strength, int agility, int intelligence, int defence, int hunger, string mood);

		//delete

		Task<string> DeletePet(int id);
	}
}
