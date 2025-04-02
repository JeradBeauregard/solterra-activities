
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolterraActivities.Data;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Services
{
	public class PetService : IPetService
	{
		//create database context
		private readonly ApplicationDbContext _context;
		public PetService(ApplicationDbContext context)
		{
			_context = context;
		}

		// read
		public async Task<IEnumerable<Pet>> ListPets()
		{
			return await _context.Pets.ToListAsync();
		}

		//create

		public async Task<Pet> CreatePetAdmin(string name, int userId, int species_id, int level, int health, int strength, int agility, int intelligence, int defence, int hunger, string mood)
		{
			Pet pet = new Pet
			{
				Name = name,
				UserId = userId,
				SpeciesId = species_id,
				Level = level,
				Health = health,
				Strength = strength,
				Agility = agility,
				Intelligence = intelligence,
				Defence = defence,
				Hunger = hunger,
				Mood = mood
			};
			_context.Pets.Add(pet);
			await _context.SaveChangesAsync();
			return pet;
		}

		public async Task<Pet> CreatePetUser(string name, int userId, int species_id)
		{
			Pet pet = new Pet
			{
				Name = name,
				UserId = userId,
				SpeciesId = species_id,
				Level = 1,
				Health = 100,
				Strength = 10,
				Agility = 10,
				Intelligence = 10,
				Defence = 10,
				Hunger = 100,
				Mood = "Content" // this may need to be adjusted later depending on mood system
			};
			_context.Pets.Add(pet);
			await _context.SaveChangesAsync();
			return pet;
		}

		//update

		public async Task<Pet> UpdatePetAdmin(int id, string name, int userId, int species_id, int level, int health, int strength, int agility, int intelligence, int defence, int hunger, string mood)
		{
			Pet pet = await _context.Pets.FindAsync(id);
			if (pet == null)
			{
				return null;
			}
			pet.Name = name;
			pet.UserId = userId;
			pet.SpeciesId = species_id;
			pet.Level = level;
			pet.Health = health;
			pet.Strength = strength;
			pet.Agility = agility;
			pet.Intelligence = intelligence;
			pet.Defence = defence;
			pet.Hunger = hunger;
			pet.Mood = mood;
			_context.Pets.Update(pet);
			await _context.SaveChangesAsync();
			return pet;
		}

		//delete

		public async Task<string> DeletePet(int id)
		{
			Pet pet = await _context.Pets.FindAsync(id);
			if (pet == null)
			{
				return "Pet not found";
			}
			_context.Pets.Remove(pet);
			await _context.SaveChangesAsync();
			return "Pet deleted";
		}


	}
}
