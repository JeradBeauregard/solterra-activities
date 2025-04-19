using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Models;
using SolterraActivities.Interfaces;

namespace SolterraActivities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [HttpGet("List")]
        [Authorize]
        public async Task<IEnumerable<Species>> ListSpecies()
        {
            return await _speciesService.ListSpecies();
        }

        [HttpGet("Find/{id}")]
        [Authorize]
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
        [HttpPost("CreateSpecies")]
        [Authorize]
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
        [HttpPost("UpdateSpecies")]
        [Authorize]
        public async Task<Species> UpdateSpecies(int id, string name)
        {
            return await _speciesService.UpdateSpecies(id, name);
        }

        //delete

        [HttpPost("DeleteSpecies")]
        [Authorize]
        public async Task<string> DeleteSpecies(int id)
        {
            return await _speciesService.DeleteSpecies(id);
        }
    }
}
