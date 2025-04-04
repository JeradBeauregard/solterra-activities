using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Controllers
{

	public class SpeciesPageController : Controller
	{

		private readonly ISpeciesService _speciesService;
		public SpeciesPageController(ISpeciesService speciesService)
		{
			_speciesService = speciesService;
		}
		
		// GET: SpeciesPage/List
		public async Task<IActionResult> List()
		{
			IEnumerable<Species> species = await _speciesService.ListSpecies();

			return View(species);
		}

		// GET: SpeciesPage/Details
		public async Task<IActionResult> Details(int id)
		{
			Species result = await _speciesService.ListSingleSpecies(id);
			return View(result);
		}
		// GET: SpeciesPage/New
		public IActionResult New()
		{
			return View();
		}

		// POST: SpeciesPage/Create
		[HttpPost]
		public async Task<IActionResult> Create(string SpeciesName)
		{
			if (ModelState.IsValid)
			{
				await _speciesService.CreateSpecies(SpeciesName); // wil update later with species skins
				return RedirectToAction("List");
			}
			return View();
		}

		// GET: SpeciesPage/Edit
		public async Task<IActionResult> Edit(int id)
		{
			Species result = await _speciesService.ListSingleSpecies(id);
			return View(result);
		}

		// POST: SpeciesPage/Update
		[HttpPost]
		public async Task<IActionResult> Update(int id, string name)
		{
			if (ModelState.IsValid)
			{
				await _speciesService.UpdateSpecies(id, name);
				return RedirectToAction("List");
			}
			return View();
		}
		// GET: SpeciesPage/ConfirmDelete
		public async Task<IActionResult> ConfirmDelete(int id)
		{
			Species result = await _speciesService.ListSingleSpecies(id);
			return View(result);
		}
		// POST: SpeciesPage/ConfirmDelete
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			if (ModelState.IsValid)
			{
				await _speciesService.DeleteSpecies(id);
				return RedirectToAction("List");
			}
			return View();
		}

	}
}
