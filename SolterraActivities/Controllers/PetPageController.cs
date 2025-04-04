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
		public PetPageController(IPetService petService, ISpeciesService speciesService, IMoodService moodService)
		{
			_petService = petService;
			_speciesService = speciesService;
			_moodService = moodService;
		}
		// GET: PetPage/List
		public async Task<IActionResult> List()
		{
			IEnumerable<Pet> pets = await _petService.ListPets();

			return View(pets);
		}

		// GET: PetPage/Details
		public async Task<IActionResult> Details(int id)
		{
			Pet result = await _petService.ListPet(id);

			return View(result);
		}

		// GET: PetPage/New
		public async Task<IActionResult> New()
		{
			PetViewModels.PetNew petNew = new PetViewModels.PetNew();

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

		public async Task<IActionResult> Create( string name, int userId, int species_id, int level, int health, int strength, int agility, int intelligence, int defence, int hunger, string mood)
		{
			Pet pet = await _petService.CreatePetAdmin(name,userId,species_id, level, health, strength, agility,  intelligence, defence,  hunger,  mood);
			return RedirectToAction("List");
		}

		// GET: PetPage/Edit
		public async Task<IActionResult> Edit(int id)
		{
			Pet result = await _petService.ListPet(id);
			return View(result);
		}

		// GET: PetPage/Delete
		public async Task<IActionResult> ConfirmDelete(int id) {

			string result = await _petService.DeletePet(id);
			return RedirectToAction("List");
		}
	}
}
