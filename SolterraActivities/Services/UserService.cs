using SolterraActivities.Data;  
using SolterraActivities.Models;  
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using SolterraActivities.Interfaces;

namespace SolterraActivities.Services  
{
	public class UserService : IUserService
	{
		private readonly ApplicationDbContext _context;

		// Inject ApplicationDbContext
		public UserService(ApplicationDbContext context)
		{
			_context = context;
		}

		// get individual user
		public async Task<User> GetUser(int id)
		{
			var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				User userError = new User();
				user.Username = "User not found";
				return userError;
			}

			return user;
		}

		// list users
		public async Task<IEnumerable<User>> GetUsers()
		{
			return await _context.Users.ToListAsync();
		}


		/// <summary>
		/// updates inventory space for a user
		/// </summary>
		/// <returns>
		/// An object 
		/*
		 {
           "message": string,
           "userId": int
           "updatedInventorySpace": int
         }
		 */
		/// </returns>
		/// <example>
		/// updateUserSpace(2,2); ->
		/*
	        {
              "message": "Inventory space updated successfully.",
              "userId": 2,
              "updatedInventorySpace": 41
            }
		*/
		/// </example>
		// method for updating user inventory space
		public async Task<string> updateUserSpace(int userId, int spaceChange)
		{
			// find user 
			var user = await _context.Users.FindAsync(userId);
			user.InventorySpace -= spaceChange;
			await _context.SaveChangesAsync();

			return "space updated succesfully";
			// update to return service response

		}

		// create user

		public async Task<CreateUserDto> PostUser(string username, string password)
		{
			User user = new User();

			user.Username = username;
			user.Password = password;
			user.InventorySpace = 50;
			user.SolShards = 5000;

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			CreateUserDto createUserDto = new CreateUserDto();

			createUserDto.Username = user.Username;
			createUserDto.Password = user.Password;
			createUserDto.InventorySpace = user.InventorySpace;
			createUserDto.SolShards = user.SolShards;
			

			return createUserDto;
		}

		// edit an existing user

		public async Task<CreateUserDto> EditUser(int id, string username, string password, int solshards)
		{
			User user = await _context.Users.FindAsync(id);
			user.Username = username;
			user.Password = password;
			user.SolShards = solshards;
			_context.Users.Update(user);
			await _context.SaveChangesAsync();
			CreateUserDto createUserDto = new CreateUserDto();
			createUserDto.Username = user.Username;
			createUserDto.Password = user.Password;
			createUserDto.InventorySpace = user.InventorySpace;
			createUserDto.SolShards = user.SolShards;
			return createUserDto;
		}

		// delete user

		public async Task<string> DeleteUser(int id)
		{
			User user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				return "user not found";
			}
			_context.Users.Remove(user);
			await _context.SaveChangesAsync();
			return "user deleted";
		}

		// update active pet

		public async Task<string> UpdateActivePet(int id, int petId)
		{
			User user = await _context.Users.FindAsync(id);
			if (user == null)
			{
				return "user not found";
			}
			user.ActivePetId = petId;
			_context.Users.Update(user);
			await _context.SaveChangesAsync();
			return "active pet updated";
		}


	}
}


