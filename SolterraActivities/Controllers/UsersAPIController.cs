using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolterraActivities.Data;
using SolterraActivities.Models;
using SolterraActivities.Services;

namespace SolterraActivities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersAPIController : ControllerBase
    {
		private readonly ApplicationDbContext _context;
		private readonly UserService _userService;  

		// Inject both ApplicationDbContext and UserService
		public UsersAPIController(ApplicationDbContext context, UserService userService)
		{
			_context = context;
			_userService = userService; 
		}
		/// <summary>
		/// updates inventory space using a user service
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
		/// GET: api/UserAPI/updateInventorySpace/2/2 -> 
		/*
	        {
              "message": "Inventory space updated successfully.",
              "userId": 2,
              "updatedInventorySpace": 41
            }
		*/
		/// </example>

		// PUT api/UsersAPI/updateInventorySpace
		[HttpPut("updateInventorySpace/{userId}/{spaceRemoved}")]

        public async Task<ActionResult<Object>> updateUserSpace(int userId, int spaceRemoved)
        {

			return await _userService.updateUserSpace(userId, spaceRemoved);

		}

		/// <summary>
		/// Gets a list of all users
		/// </summary>
		/// <returns>
		/// [{User},{User}, {User},...]
		/// </returns>
		/// <example>
		/// GET: api/UserAPI
		/* [
		  {
			"id": 2,
			"username": "mango_salad",
			"password": "mango123",
			"inventorySpace": 17,
			"solShards": 5000
		  },
		  {
			"id": 3,
			"username": "iamtheeditedusername",
			"password": "password",
			"inventorySpace": 46,
			"solShards": 5000
		  }, */
		/// </example>

		// GET: api/UsersAPI
		[HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

		/// <summary>
		/// Gets a single User by id
		/// </summary>
		/// <returns>
		/// {User}
		/// </returns>
		/// <example>
		/// GET: api/UserAPI/3
		/* {
		  "id": 3,
		  "username": "iamtheeditedusername",
		  "password": "password",
		  "inventorySpace": 46,
		  "solShards": 5000
		} */
		/// </example>

		// GET: api/UsersAPI/5
		[HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

		/// <summary>
		///	Create a new user, note that the id is auto-generated, and inventorySpace and solShards are set to default values
		/// </summary>
		/// <returns>
		/// {CreateUserDto}
		/// </returns>
		/// <example>
		/// GET: api/UserAPI/PostUser/Jerad/Beauregard ->	
		/*
		 * {
		  "id": 10,
		  "username": "Jerad",
		  "password": "Beauregard",
		  "inventorySpace": 50,
		  "solShards": 5000
		}*/
		/// </example>

		// POST: api/UsersAPI
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost("PostUser/{username}/{password}")]
        public async Task<ActionResult<CreateUserDto>> PostUser(string username, string password)
        {

			CreateUserDto result = await _userService.PostUser(username, password);

            return result;
        }

		/// <summary>
		///	Edit an existing user, note that inventory space cannot be edited.
		/// </summary>
		/// <param name="id">The id of the user to that will be edited</param>
		/// <param name="username">the new username</param>
		/// <param name="password">the new password</param>
		/// <param name="solshards">the new amount of solshards</param>
		/// <returns>
		/// {CreateUserDto}
		/// </returns>
		/// <example>
		/// GET: api/UserAPI/EditUser/10/Jerry/Beauregard/46/2000 ->	
		/*
		 * {
		  "id": 10,
		  "username": "Jerry",
		  "password": "Beauregard",
		  "inventorySpace": 50,
		  "solShards": 2000
		}*/
		/// </example>

		//POST: api/UsersAPI/Edit

		[HttpPost("EditUser/{id}/{username}/{password}/{solshards}")]
		public async Task<ActionResult<CreateUserDto>> EditUser(int id, string username, string password, int solshards)
		{
			CreateUserDto result = await _userService.EditUser(id, username, password, solshards);
			return result;
		}

		/// <summary> Deletes a user by id </summary>
		/// <param name="id">The id of the user to be deleted</param>
		/// <returns> A string message indicating the result of the deletion </returns>
		/// <example> DELETE: api/UsersAPI/5 -> "User deleted successfully" </example>

		// DELETE: api/UsersAPI/5
		[HttpDelete("{id}")]
        public async Task<ActionResult<string>> DeleteUser(int id)
        {
            string Result = await _userService.DeleteUser(id);
			return Result;
		}

		//PUT: api/UsersAPI/updateActivePet

		[HttpPut("updateActivePet/{userId}/{petId}")]

		public async Task<ActionResult<string>> UpdateActivePet(int userId, int petId)
		{
			string result = await _userService.UpdateActivePet(userId, petId);
			return result;
		}


	}
}
