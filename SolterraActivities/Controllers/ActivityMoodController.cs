using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SolterraActivities.Data;

namespace SolterraActivities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityMoodController : ControllerBase
    {
        private readonly IActivityMoodService _activityMoodService;

        // Dependency injection of service interfaces
        public ActivityMoodController(IActivityMoodService activityMoodService)
        {
            _activityMoodService = activityMoodService;
        }

        /// <summary>
        /// Returns a list of all Activity moods.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{ActivityMoodDto},{ActivityMoodDto},..]
        /// </returns>
        /// <example>
        /// GET: api/ActivityMood/List -> [{ActivityMoodDto},{ActivityMoodDto},..]
        /// </example>
        [HttpGet("List")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ActivityMoodDto>>> ListActivityMoods()
        {
            IEnumerable<ActivityMoodDto> activityMoodDtos = await _activityMoodService.ListActivityMoods();
            return Ok(activityMoodDtos);
        }

        /// <summary>
        /// Returns a specific Activity mood by ID.
        /// </summary>
        /// <param name="id">The Activity Mood ID</param>
        /// <returns>
        /// 200 OK
        /// {ActivityMoodDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/ActivityMood/Find/1 -> {ActivityMoodDto}
        /// </example>
        [HttpGet("Find/{id}")]
        [Authorize]
        public async Task<ActionResult<ActivityMoodDto>> FindActivityMood(int id)
        {
            var activityMoodDto = await _activityMoodService.FindActivityMood(id);

            if (activityMoodDto == null)
            {
                return NotFound();
            }

            return Ok(activityMoodDto);
        }

        /// <summary>
        /// Adds a new Activity mood.
        /// </summary>
        /// <param name="activityMoodDto">The required information to add the Activity mood</param>
        /// <returns>
        /// 201 Created
        /// Location: api/ActivityMood/Find/{ActivityMoodId}
        /// {ActivityMoodDto}
        /// or
        /// 500 Internal Server Error
        /// </returns>
        /// <example>
        /// POST: api/ActivityMood/Add
        /// Request Body: {ActivityMoodDto}
        /// -> Response Code: 201 Created
        /// Response Headers: Location: api/ActivityMood/Find/{ActivityMoodId}
        /// </example>
        [HttpPost("Add")]
        [Authorize]

        public async Task<ActionResult<ActivityMood>> AddActivityMood(ActivityMoodDto activityMoodDto)
        {
            ServiceResponse response = await _activityMoodService.AddActivityMood(activityMoodDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return Created($"api/ActivityMood/Find/{response.CreatedId}", activityMoodDto);
        }

        /// <summary>
        /// Updates an existing Activity mood.
        /// </summary>
        /// <param name="id">The Activity Mood ID</param>
        /// <param name="activityMoodDto">The required information to update the Activity mood</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// POST: api/ActivityMood/Update/5
        /// Request Body: {ActivityMoodDto}
        /// -> Response Code: 204 No Content
        /// </example>
        [HttpPost("Update/{id}")]
        [Authorize]

        public async Task<ActionResult> UpdateActivityMood(int id, ActivityMoodDto activityMoodDto)
        {
            if (id != activityMoodDto.ActivityMoodId)
            {
                return BadRequest();
            }

            ServiceResponse response = await _activityMoodService.UpdateActivityMood(id, activityMoodDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(response.Messages);
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes an Activity mood by ID.
        /// </summary>
        /// <param name="id">The Activity Mood ID to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/ActivityMood/Delete/7
        /// -> Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteActivityMood(int id)
        {
            ServiceResponse response = await _activityMoodService.DeleteActivityMood(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return NoContent();
        }

        /// <summary>
        /// Returns a list of Activity moods for a specific Activity.
        /// </summary>
        /// <param name="activityId">The Activity ID</param>
        /// <returns>
        /// 200 OK
        /// [{ActivityMoodDto},{ActivityMoodDto},..]
        /// </returns>
        /// <example>
        /// GET: api/ActivityMood/ListForActivity/3 -> [{ActivityMoodDto},{ActivityMoodDto},..]
        /// </example>
        [HttpGet("ListForActivity/{activityId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ActivityMoodDto>>> ListActivityMoodsForActivity(int activityId)
        {
            var activityMoods = await _activityMoodService.ListActivityMoodsForActivity(activityId);
            return Ok(activityMoods);
        }

    }
}
