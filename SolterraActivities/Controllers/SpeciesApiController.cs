using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Models;
using SolterraActivities.Interfaces;


namespace SolterraActivities.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SpeciesApiController : Controller
	{
		private readonly ISpeciesService _speciesService;
		public SpeciesApiController(ISpeciesService speciesService)
		{
			_speciesService = speciesService;
		}
		// read

		/// <summary>
		/// Returns a list of all species
		/// </summary>
		/// <example>
		/// GET: Api/SpeciesApi/List -> [{Species},{Species},..]
		/// </example>
		/// <returns>A list of species objects</returns>
		public async Task<IEnumerable<Species>> ListSpecies()
		{
			return await _speciesService.ListSpecies();	
		}
		public async Task<Species> ListSingleSpecies(int id)
		{
			return await _speciesService.ListSingleSpecies(id);
		}
		//create

		/// <summary>
		/// Create a new species object
		/// </summary>
		/// <example>
		/// POST: Api/SpeciesApi/CreateSpecies -> {Species}
		/// </example>
		/// <param name="name"></param>
		/// <returns>Returns created species</returns>
		public async Task<Species> CreateSpecies(string name)
		{
			return await _speciesService.CreateSpecies(name);
		}
		//update

		/// <summary>
		/// Update a species object
		/// </summary>
		/// <example>
		/// POST: Api/SpeciesApi/UpdateSpecies -> {Species}
		/// </example>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <returns>Returns the updated species object</returns>
		public async Task<Species> UpdateSpecies(int id, string name)
		{
			return await _speciesService.UpdateSpecies(id, name);
		}
		//delete
		public async Task<string> DeleteSpecies(int id)
		{
			return await _speciesService.DeleteSpecies(id);
		}
	}
}
