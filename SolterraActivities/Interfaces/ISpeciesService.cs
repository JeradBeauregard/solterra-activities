using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Models;
using SolterraActivities.Services;

namespace SolterraActivities.Interfaces
{
	public interface ISpeciesService
	{
		// read
		Task<IEnumerable<Species>> ListSpecies();
		//create
		Task<Species> CreateSpecies(string name);
		//update
		Task<Species> UpdateSpecies(int id, string name);
		//delete
		Task<string> DeleteSpecies(int id);
	}
}
