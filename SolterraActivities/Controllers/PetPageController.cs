using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using SolterraActivities.Models.ViewModels;

namespace SolterraActivities.Controllers
{
	public class PetPageController : Controller
	{
		// Dependency Injection

		private readonly IPetService _petService;
		private readonly ISpeciesService _speciesService;
		private readonly IMoodService _moodService;
		private readonly IUserService _userService;
		public PetPageController(IPetService petService, ISpeciesService speciesService, IMoodService moodService, IUserService userService)
		{
			_petService = petService;
			_speciesService = speciesService;
			_moodService = moodService;
			_userService = userService;
		}
        // GET: PetPage/List
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> List()
		{
			IEnumerable<Pet> pets = await _petService.ListPets();

			return View(pets);
		}

        // GET: PetPage/Details
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
		{
			Pet pet = await _petService.ListPet(id);

			PetViewModels.PetDetails petDetails = new PetViewModels.PetDetails
			{
				Id = pet.Id,
				Name = pet.Name,
				UserId = pet.UserId,
				SpeciesId = pet.SpeciesId,
				Level = pet.Level,
				Health = pet.Health,
				Strength = pet.Strength,
				Agility = pet.Agility,
				Intelligence = pet.Intelligence,
				Defence = pet.Defence,
				Hunger = pet.Hunger,
				Mood = pet.Mood
			};

			// Get the species name from the database
			Species species = await _speciesService.ListSingleSpecies(pet.SpeciesId);
			petDetails.SpeciesName = species.Name;





			return View(petDetails);
		}

        // GET: PetPage/New
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> New()
		{
			PetViewModels.PetNew petNew = new PetViewModels.PetNew();

			// get user from database
			IEnumerable<User> users = await _userService.GetUsers();

			petNew.Users = users.ToList();


			// Get the list of species from the database
			IEnumerable<Species> species = await _speciesService.ListSpecies();
			// Populate the PetNew view model with the species
			petNew.Species = species.ToList();

			// Get the list of moods from the database
			IEnumerable<MoodDto> moods = await _moodService.ListMoods();
		

			// Populate the PetNew view model with the moods
			petNew.Moods = moods.ToList();


			return View(petNew);
		}

        // POST: PetPage/Create
        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Create( string name, int userId, int species_id, int level, int health, int strength, int agility, int intelligence, int defence, int hunger, string mood)
		{
			Pet pet = await _petService.CreatePetAdmin(name,userId,species_id, level, health, strength, agility,  intelligence, defence,  hunger,  mood);
			return RedirectToAction("List");
		}

        // GET: PetPage/Edit
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
		{
			PetViewModels.PetEdit petUpdate = new PetViewModels.PetEdit();

			// populate the PetEdit view model with the pet data
			Pet pet = await _petService.ListPet(id);
			petUpdate.Id = pet.Id;
			petUpdate.Name = pet.Name;
			petUpdate.UserId = pet.UserId;
			petUpdate.SpeciesId = pet.SpeciesId;
			petUpdate.Level = pet.Level;
			petUpdate.Health = pet.Health;
			petUpdate.Strength = pet.Strength;
			petUpdate.Agility = pet.Agility;
			petUpdate.Intelligence = pet.Intelligence;
			petUpdate.Defence = pet.Defence;
			petUpdate.Hunger = pet.Hunger;
			petUpdate.Mood = pet.Mood;


			// Get the list of species from the database
			IEnumerable<Species> species = await _speciesService.ListSpecies();
			// Populate the PetNew view model with the species
			petUpdate.Species = species.ToList();

			// Get the list of moods from the database
			IEnumerable<MoodDto> moods = await _moodService.ListMoods();


			// Populate the PetNew view model with the moods
			petUpdate.Moods = moods.ToList();

			return View(petUpdate);
		}

        // POST: PetPage/Update	
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int id, string name, int userId, int species_id, int level, int health, int strength, int agility, int intelligence, int defence, int hunger, string mood)
		{
			Pet pet = await _petService.UpdatePetAdmin(id, name, userId, species_id, level, health, strength, agility, intelligence, defence, hunger, mood);
			return RedirectToAction("List");
		}

        // GET: PetPage/ConfirmDelete
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(int id)
		{
			Pet pet = await _petService.ListPet(id);
			
			return View(pet);
		}


        // DELETE: PetPage/Delete
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id) {

			string result = await _petService.DeletePet(id);
			return RedirectToAction("List");
		}
	}
}
