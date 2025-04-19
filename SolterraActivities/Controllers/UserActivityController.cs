using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivityController : ControllerBase
    {
        private readonly IUserActivityService _userActivityService;

        // Dependency injection of service interfaces
        public UserActivityController(IUserActivityService userActivityService)
        {
            _userActivityService = userActivityService;
        }

        /// <summary>
        /// Returns a list of all user activity records.
        /// </summary>
        /// <returns>
        /// 200 OK  
        /// [{UserActivityDto},{UserActivityDto},...]
        /// </returns>
        /// <example>
        /// GET: api/UserActivity/List -> [{UserActivityDto},{UserActivityDto},...]
        /// </example>
        [HttpGet("List")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserActivityDto>>> ListUserActivities()
        {
            var results = await _userActivityService.ListUserActivities();
            return Ok(results);
        }

        /// <summary>
        /// Returns a specific user activity by ID.
        /// </summary>
        /// <param name="id">The UserActivity ID</param>
        /// <returns>
        /// 200 OK  
        /// {UserActivityDto}  
        /// or  
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/UserActivity/Find/5 -> {UserActivityDto}
        /// </example>
        [HttpGet("Find/{id}")]
        [Authorize]
        public async Task<ActionResult<UserActivityDto>> FindUserActivity(int id)
        {
            var dto = await _userActivityService.FindUserActivity(id);
            if (dto == null) return NotFound();
            return Ok(dto);
        }

        /// <summary>
        /// Adds a new user activity.
        /// </summary>
        /// <param name="userActivityDto">The information required to create a new user activity</param>
        /// <returns>
        /// 201 Created  
        /// Location: api/UserActivity/Find/{UserActivityId}  
        /// {UserActivityDto}  
        /// or  
        /// 404 Not Found (if related entities do not exist)  
        /// or  
        /// 500 Internal Server Error
        /// </returns>
        /// <example>
        /// POST: api/UserActivity/Add  
        /// Request Body: {UserActivityDto}  
        /// -> Response Code: 201 Created  
        /// Response Header: Location: api/UserActivity/Find/10
        /// </example>
        [HttpPost("Add")]
        [Authorize]
        public async Task<ActionResult> AddUserActivity(UserActivityDto userActivityDto)
        {
            var response = await _userActivityService.AddUserActivity(userActivityDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                return NotFound(response.Messages);

            if (response.Status == ServiceResponse.ServiceStatus.Error)
                return StatusCode(500, response.Messages);

            return Created($"api/UserActivity/Find/{response.CreatedId}", userActivityDto);
        }

        /// <summary>
        /// Updates an existing user activity.
        /// </summary>
        /// <param name="id">The UserActivity ID</param>
        /// <param name="userActivityDto">The updated information for the user activity</param>
        /// <returns>
        /// 204 No Content  
        /// or  
        /// 400 Bad Request  
        /// or  
        /// 404 Not Found  
        /// or  
        /// 500 Internal Server Error
        /// </returns>
        /// <example>
        /// POST: api/UserActivity/Update/5  
        /// Request Body: {UserActivityDto}  
        /// -> Response Code: 204 No Content
        /// </example>
        [HttpPost("Update/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateUserActivity(int id, UserActivityDto userActivityDto)
        {
            if (id != userActivityDto.UserActivityId)
                return BadRequest("UserActivity ID mismatch.");

            var response = await _userActivityService.UpdateUserActivity(id, userActivityDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                return NotFound(response.Messages);

            if (response.Status == ServiceResponse.ServiceStatus.Error)
                return StatusCode(500, response.Messages);

            return NoContent();
        }

        /// <summary>
        /// Deletes a user activity by ID.
        /// </summary>
        /// <param name="id">The UserActivity ID to delete</param>
        /// <returns>
        /// 204 No Content  
        /// or  
        /// 404 Not Found  
        /// or  
        /// 500 Internal Server Error
        /// </returns>
        /// <example>
        /// DELETE: api/UserActivity/Delete/7  
        /// -> Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteUserActivity(int id)
        {
            var response = await _userActivityService.DeleteUserActivity(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                return NotFound();

            if (response.Status == ServiceResponse.ServiceStatus.Error)
                return StatusCode(500, response.Messages);

            return NoContent();
        }

        /// <summary>
        /// Returns a list of user activities for a specific user.
        /// </summary>
        /// <param name="userId">The User ID</param>
        /// <returns>
        /// 200 OK  
        /// [{UserActivityDto},{UserActivityDto},...]
        /// </returns>
        /// <example>
        /// GET: api/UserActivity/ListForUser/3  
        /// -> [{UserActivityDto},{UserActivityDto},...]
        /// </example>
        [HttpGet("ListForUser/{userId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserActivityDto>>> ListUserActivitiesForUser(int userId)
        {
            var results = await _userActivityService.ListUserActivitiesForUser(userId);
            return Ok(results);
        }

        /// <summary>
        /// Links a user to an activity by creating a new UserActivity record.
        /// </summary>
        /// <param name="userActivityDto">The UserId, ActivityId, PetId, and ItemId to link</param>
        /// <returns>
        /// 201 Created  
        /// or  
        /// 404 Not Found  
        /// or  
        /// 500 Internal Server Error
        /// </returns>
        /// <example>
        /// POST: api/UserActivity/LinkUser  
        /// Request Body: { UserId: 1, ActivityId: 2, PetId: 3, ItemId: 4 }  
        /// -> Response Code: 201 Created
        /// </example>
        [HttpPost("LinkUser")]
        [Authorize]
        public async Task<ActionResult> LinkUserToActivity([FromBody] UserActivityDto userActivityDto)
        {
            var response = await _userActivityService.LinkUserToActivity(
                userActivityDto.UserId,
                userActivityDto.ActivityId,
                userActivityDto.PetId,
                userActivityDto.ItemId
            );

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                return NotFound(response.Messages);

            if (response.Status == ServiceResponse.ServiceStatus.Error)
                return StatusCode(500, response.Messages);

            return Created($"api/UserActivity/Find/{response.CreatedId}", userActivityDto);
        }

        /// <summary>
        /// Unlinks a user from an activity by deleting the UserActivity record.
        /// </summary>
        /// <param name="userActivityId">The UserActivity ID to remove</param>
        /// <returns>
        /// 204 No Content  
        /// or  
        /// 404 Not Found  
        /// or  
        /// 500 Internal Server Error
        /// </returns>
        /// <example>
        /// POST: api/UserActivity/UnlinkUser/12  
        /// -> Response Code: 204 No Content
        /// </example>
        [HttpPost("UnlinkUser/{userActivityId}")]
        [Authorize]
        public async Task<ActionResult> UnlinkUserFromActivity(int userActivityId)
        {
            var response = await _userActivityService.UnlinkUserFromActivity(userActivityId);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
                return NotFound();

            if (response.Status == ServiceResponse.ServiceStatus.Error)
                return StatusCode(500, response.Messages);

            return NoContent();
        }
    }
}
