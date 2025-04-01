using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SolterraActivities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _activityService;

        public ActivityController(IActivityService activityService)
        {
            _activityService = activityService;
        }


        /// <summary>
        /// Returns a list of all Activitys
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{ActivityDto},{ActivityDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Activity/List -> [{ActivityDto},{ActivityDto},..]
        /// </example>
        [HttpGet("List")]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> ListActivities()
        {
            var activities = await _activityService.ListActivities();
            return Ok(activities);
        }



        /// <summary>
        /// Returns a single Activity specified by its {id}
        /// </summary>
        /// <param name="id">The Activity ID</param>
        /// <returns>
        /// 200 OK
        /// {ActivityDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Activity/Find/1 -> {ActivityDto}
        /// </example>
        [HttpGet("Find/{id}")]
        public async Task<ActionResult<ActivityDto>> FindActivity(int id)
        {
            var activity = await _activityService.FindActivity(id);
            if (activity == null) return NotFound();

            return Ok(activity);
        }


        /// <summary>
        /// Adds an Activity
        /// </summary>
        /// <param name="activityDto">The required information to add the activity</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Activity/Find/{ActivityId}
        /// {ActivityDto}
        /// or
        /// 500 Internal Server Error
        /// </returns>
        /// <example>
        /// POST: api/Activity/Add
        /// Request Headers: Content-Type: application/json, cookie: .AspNetCore.Identity.Application={token}
        /// Request Body: {ActivityDto}
        /// -> Response Code: 201 Created
        /// Response Headers: Location: api/Activity/Find/{ActivityId}
        /// </example>
        [HttpPost("Add")]
        [Authorize]
        public async Task<ActionResult> AddActivity(ActivityDto activityDto)
        {
            var response = await _activityService.AddActivity(activityDto);

            if (response.Status == ServiceResponse.ServiceStatus.Error)
                return StatusCode(500, response.Messages);

            return Created($"api/Activity/Find/{response.CreatedId}", activityDto);
        }


        /// <summary>
        /// Updates an existing Activity
        /// </summary>
        /// <param name="id">The ID of the Activity to update</param>
        /// <param name="activityDto">The required information to update the activity</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/Activity/Update/5
        /// Request Headers: Content-Type: application/json, cookie: .AspNetCore.Identity.Application={token} 
        /// Request Body: {ActivityDto}
        /// -> Response Code: 204 No Content
        /// </example>

        [HttpPut("Update/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateActivity(int id, ActivityDto activityDto)
        {
            if (id != activityDto.ActivityId) return BadRequest();

            var response = await _activityService.UpdateActivity(id, activityDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound) return NotFound();
            if (response.Status == ServiceResponse.ServiceStatus.Error) return StatusCode(500, response.Messages);

            return NoContent();
        }


        /// <summary>
        /// Deletes an Activity
        /// </summary>
        /// <param name="id">The ID of the Activity to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Activity/Delete/7
        /// Request Headers: C cookie: .AspNetCore.Identity.Application={token}
        /// -> Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteActivity(int id)
        {
            var response = await _activityService.DeleteActivity(id);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound) return NotFound();
            if (response.Status == ServiceResponse.ServiceStatus.Error) return StatusCode(500, response.Messages);

            return NoContent();
        }



        /// <summary>
        /// Updates an Activity's image and saves it to a designated location
        /// </summary>
        /// <param name="id">The Activity ID for which the image is being updated</param>
        /// <param name="activityImage">The new image file</param>
        /// <returns>
        /// 200 OK
        /// or
        /// 404 Not Found
        /// or
        /// 500 Internal Server Error
        /// </returns>
        /// <example>
        /// PUT: api/Activity/UpdateActivityImage/5
        /// HEADERS: Content-Type: Multi-part/form-data, Cookie: .AspNetCore.Identity.Application={token}
        /// FORM DATA:
        /// ------boundary
        /// Content-Disposition: form-data; name="activityImage"; filename="activity1.jpg"
        /// Content-Type: image/jpeg
        /// </example>
        /// <example>
        /// curl "https://localhost:xx/api/Activity/UpdateActivityImage/5" -H "Cookie: .AspNetCore.Identity.Application={token}" -X "PUT" -F ActivityImage=@experienve1.jpg
        /// </example>
        [HttpPut("UpdateActivityImage/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateActivityImage(int id, IFormFile activityImage)
        {
            ServiceResponse response = await _activityService.UpdateActivityImage(id, activityImage);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound();
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return Ok(new { message = "Image updated successfully." });
        }

    }
}
