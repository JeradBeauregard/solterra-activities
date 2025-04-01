using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using SolterraActivities.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolterraActivities.Services
{
    public class MoodService : IMoodService
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection of database context
        public MoodService(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all moods
        public async Task<IEnumerable<MoodDto>> ListMoods()
        {
            // Retrieve all moods from the database
            List<Mood> moods = await _context.Moods.ToListAsync();

            
            List<MoodDto> moodDtos = new();

            // foreach mood, create a DTO and add to list
            foreach (Mood mood in moods)
            {
                moodDtos.Add(new MoodDto()
                {
                    MoodId = mood.MoodId,
                    MoodName = mood.MoodName,
                    MoodActivityCount = _context.ActivityMoods.Count(em => em.MoodId == mood.MoodId)
                });
            }
            return moodDtos;
        }

        // Find a specific mood by ID
        public async Task<MoodDto?> FindMood(int id)
        {
            // Find the mood in the database
            var mood = await _context.Moods
                .FirstOrDefaultAsync(m => m.MoodId == id);

            // If no mood is found, return null
            if (!await MoodExists(id))
            {
                return null;
            }

            // create and return an instance of MoodDto
            return new MoodDto()
            {
                MoodId = mood.MoodId,
                MoodName = mood.MoodName,
                MoodActivityCount = _context.ActivityMoods.Count(em => em.MoodId == mood.MoodId)
            };
        }


        // List Activities related to a specific mood
        public async Task<IEnumerable<ActivityDto>> ListActivitiesForMood(int moodId)
        {
            // join ActivityMood and Activity tables to retrieve Activities linked to this mood
            List<Activity> activities = await _context.ActivityMoods
                .Where(em => em.MoodId == moodId)
                .Include(em => em.Activity)
                .Select(em => em.Activity)
                .Distinct()
                .ToListAsync();

            
            List<ActivityDto> activityDtos = new();

            // for each Activity, create a DTO and add to list
            foreach (Activity a in activities)
            {
                activityDtos.Add(new ActivityDto()
                {
                    ActivityId = a.ActivityId,
                    ActivityName = a.ActivityName,
                    HobbyId = a.HobbyId,
                    HobbyName = a.Hobby?.HobbyName,
                    ActivityCost = a.ActivityCost,
                    DurationinHours = a.DurationInHours,
                    ActivityDate = a.ActivityDate,
                    ActivityLocation = a.ActivityLocation
                });
            }
            return activityDtos;
        }

        // List moods associated with a specific Activity
        public async Task<IEnumerable<MoodDto>> ListMoodsForActivity(int activityId)
        {
            // Retrieve moods linked to this Activity
            List<Mood> moods = await _context.ActivityMoods
                .Where(am => am.ActivityId == activityId)
                .Select(em => em.Mood)
                .ToListAsync();
            
            List<MoodDto> moodDtos = new();

            // for each mood, create a DTO and add to list
            foreach (Mood mood in moods)
            {
                moodDtos.Add(new MoodDto()
                {
                    MoodId = mood.MoodId,
                    MoodName = mood.MoodName
                });
            }
            return moodDtos;
        }

        // add a new mood
        public async Task<ServiceResponse> AddMood(MoodDto moodDto)
        {
            ServiceResponse response = new();

            // validate that mood name is not empty
            if (string.IsNullOrWhiteSpace(moodDto.MoodName))
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Mood name cannot be empty.");
                return response;
            }

            try
            {
                // Create a new Mood object
                Mood mood = new()
                {
                    MoodName = moodDto.MoodName
                };

                _context.Moods.Add(mood);
                await _context.SaveChangesAsync();

                response.Status = ServiceResponse.ServiceStatus.Created;
                response.CreatedId = mood.MoodId;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error adding the mood.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        // Update an existing mood
        public async Task<ServiceResponse> UpdateMood(int id, MoodDto moodDto)
        {
            ServiceResponse response = new();

            // Validate mood ID
            if (id != moodDto.MoodId)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Mood ID mismatch.");
                return response;
            }

            var existingMood = await _context.Moods.FindAsync(id);

            if (existingMood == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Mood not found.");
                return response;
            }

            try
            {
                // Update mood properties
                existingMood.MoodName = moodDto.MoodName;
                _context.Entry(existingMood).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await MoodExists(id))
                {
                    response.Status = ServiceResponse.ServiceStatus.NotFound;
                    response.Messages.Add("Mood not found.");
                }
                else
                {
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    response.Messages.Add("Concurrency error updating the mood.");
                }
            }

            return response;
        }

        // Delete a mood
        public async Task<ServiceResponse> DeleteMood(int id)
        {
            ServiceResponse response = new();

            var mood = await _context.Moods.FindAsync(id);

            if (!await MoodExists(id))
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Mood not found.");
                return response;
            }

            try
            {
                _context.Moods.Remove(mood);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error deleting the mood.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        // Checks if a mood exists
        private async Task<bool> MoodExists(int id)
        {
            return await _context.Moods.AnyAsync(m => m.MoodId == id);
        }
    }
}
