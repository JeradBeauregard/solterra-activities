﻿using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SolterraActivities.Models;
using System.Diagnostics;

namespace SolterraActivities.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoodController : ControllerBase
    {
        private readonly IMoodService _moodService;

        public MoodController(IMoodService moodService)
        {
            _moodService = moodService;
        }

        /// <summary>
        /// Returns a list of all moods.
        /// </summary>
        /// <returns>
        /// 200 OK
        /// [{MoodDto},{MoodDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Mood/List -> [{MoodDto},{MoodDto},..]
        /// </example>
        [HttpGet("List")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<MoodDto>>> ListMoods()
        {
            var moods = await _moodService.ListMoods();
            return Ok(moods);
        }

        /// <summary>
        /// Returns a specific mood by ID.
        /// </summary>
        /// <param name="id">The Mood ID</param>
        /// <returns>
        /// 200 OK
        /// {MoodDto}
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// GET: api/Mood/Find/1 -> {MoodDto}
        /// </example>
        [HttpGet("Find/{id}")]
        [Authorize]
        public async Task<ActionResult<MoodDto>> FindMood(int id)
        {
            var mood = await _moodService.FindMood(id);

            if (mood == null)
            {
                return NotFound();
            }

            return Ok(mood);
        }

        /// <summary>
        /// Lists activities related to a specific mood.
        /// </summary>
        /// <param name="moodId">The Mood ID</param>
        /// <returns>
        /// 200 OK
        /// [{ActivityDto},{ActivityDto},..]
        /// </returns>
        /// <example>
        /// GET: api/Mood/Activities/3 -> [{ActivityDto},{ActivityDto},..]
        /// </example>
        [HttpGet("Activities/{moodId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> ListActivitiesForMood(int moodId)
        {
            var activities = await _moodService.ListActivitiesForMood(moodId);
            return Ok(activities);
        }

        /// <summary>
        /// Adds a new mood.
        /// </summary>
        /// <param name="moodDto">The required information to add the mood</param>
        /// <returns>
        /// 201 Created
        /// Location: api/Mood/Find/{MoodId}
        /// {MoodDto}
        /// or
        /// 500 Internal Server Error
        /// </returns>
        /// <example>
        /// POST: api/Mood/Add
        /// Request Headers: Content-Type: application/json, cookie: .AspNetCore.Identity.Application={token}
        /// Request Body: {MoodDto}
        /// -> Response Code: 201 Created
        /// Response Headers: Location: api/Mood/Find/{MoodId}
        /// </example>
        [HttpPost("Add")]
        [Authorize]
        public async Task<ActionResult<Mood>> AddMood(MoodDto moodDto)
        {
            ServiceResponse response = await _moodService.AddMood(moodDto);

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return StatusCode(500, response.Messages);
            }

            return Created($"api/Mood/Find/{response.CreatedId}", moodDto);
        }

        /// <summary>
        /// Updates an existing mood.
        /// </summary>
        /// <param name="id">The Mood ID</param>
        /// <param name="moodDto">The required information to update the mood</param>
        /// <returns>
        /// 400 Bad Request
        /// or
        /// 404 Not Found
        /// or
        /// 204 No Content
        /// </returns>
        /// <example>
        /// PUT: api/Mood/Update/5
        /// Request Headers: Content-Type: application/json, cookie: .AspNetCore.Identity.Application={token}
        /// Request Body: {MoodDto}
        /// -> Response Code: 204 No Content
        /// </example>
        [HttpPut("Update/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateMood(int id, MoodDto moodDto)
        {
            if (id != moodDto.MoodId)
            {
                return BadRequest();
            }

            ServiceResponse response = await _moodService.UpdateMood(id, moodDto);

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
        /// Deletes a mood by ID.
        /// </summary>
        /// <param name="id">The Mood ID to delete</param>
        /// <returns>
        /// 204 No Content
        /// or
        /// 404 Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/Mood/Delete/7
        /// Request Headers: cookie: .AspNetCore.Identity.Application={token} 
        /// -> Response Code: 204 No Content
        /// </example>
        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteMood(int id)
        {
            ServiceResponse response = await _moodService.DeleteMood(id);

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
    }
}
