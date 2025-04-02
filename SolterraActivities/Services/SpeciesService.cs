using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolterraActivities.Data;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Services
{
	public class SpeciesService : ISpeciesService
	{
		//create database connection
		private readonly ApplicationDbContext _context;
		public SpeciesService(ApplicationDbContext context)
		{
			_context = context;
		}

		// read
		public async Task<IEnumerable<Species>> ListSpecies()
		{
			return await _context.Species.ToListAsync();
		}

		//create

		public async Task<Species> CreateSpecies(string name)
		{
			Species species = new Species
			{
				Name = name
			};
			_context.Species.Add(species);
			await _context.SaveChangesAsync();
			return species;
		}

		//update

		public async Task<Species> UpdateSpecies(int id, string name)
		{
			Species species = await _context.Species.FindAsync(id);
			if (species == null)
			{
				return null;
			}
			species.Name = name;
			await _context.SaveChangesAsync();
			return species;
		}

		//delete

		public async Task<string> DeleteSpecies(int id)
		{
			Species species = await _context.Species.FindAsync(id);
			if (species == null)
			{
				return "Species not found";
			}
			_context.Species.Remove(species);
			await _context.SaveChangesAsync();
			return "Species deleted";
		}
	}
}
