using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using SolterraActivities.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace SolterraActivities.Services
{
    public class ActivityMoodService : IActivityMoodService
    {
        private readonly ApplicationDbContext _context;

        // dependency injection of database context
        public ActivityMoodService(ApplicationDbContext context)
        {
            _context = context;
        }


        // List all ActivityMoods
        public async Task<IEnumerable<ActivityMoodDto>> ListActivityMoods()
        {
            // Retrieve all ActivityMoods including related Activitys and Moods
            List<ActivityMood> activityMoods = await _context.ActivityMoods
                .Include(em => em.Activity)
                .Include(em => em.Mood)
                .ToListAsync();

            // Convert each activityMood entity to DTO format
            List<ActivityMoodDto> activityMoodDtos = new();
            foreach (ActivityMood em in activityMoods)
            {
                activityMoodDtos.Add(new ActivityMoodDto()
                {
                    ActivityMoodId = em.ActivityMoodId,
                    ActivityId = em.ActivityId,
                    MoodId = em.MoodId,
                    ActivityName = em.Activity.ActivityName,
                    MoodName = em.Mood.MoodName,
                    ActivityDate = em.Activity.ActivityDate,
                    MoodIntensityBefore = em.MoodIntensityBefore,
                    MoodIntensityAfter = em.MoodIntensityAfter
                });
            }
            return activityMoodDtos;
        }

        // find a specific ActivityMood by ID
 
        public async Task<ActivityMoodDto?> FindActivityMood(int id)
        {
            //retrieve ActivityMood including related Activity and Mood
            var activityMood = await _context.ActivityMoods
                .Include(em => em.Activity)
                .Include(em => em.Mood)
                .FirstOrDefaultAsync(em => em.ActivityMoodId == id);

            // if not found, return null
            if (activityMood == null)
            {
                return null;
            }

            // create and return DTO instance
            return new ActivityMoodDto()
            {
                ActivityMoodId = activityMood.ActivityMoodId,
                ActivityId = activityMood.ActivityId,
                MoodId = activityMood.MoodId,
                ActivityName = activityMood.Activity.ActivityName,
                MoodName = activityMood.Mood.MoodName,
                ActivityDate = activityMood.Activity.ActivityDate,
                MoodIntensityBefore = activityMood.MoodIntensityBefore,
                MoodIntensityAfter = activityMood.MoodIntensityAfter
            };
        }

        // add a new ActivityMood
        public async Task<ServiceResponse> AddActivityMood(ActivityMoodDto activityMoodDto)
        {
            ServiceResponse response = new();

            // Validate Activity and Mood exist
            var activity = await _context.Activities.FindAsync(activityMoodDto.ActivityId);
            var mood = await _context.Moods.FindAsync(activityMoodDto.MoodId);

            if (activity == null || mood == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Invalid ActivityId or MoodId.");
                return response;
            }

            try
            {
                // Create ActivityMood entity
                ActivityMood activityMood = new()
                {
                    ActivityId = activity.ActivityId,
                    MoodId = mood.MoodId,
                    Activity = activity,
                    Mood = mood,
                    MoodIntensityBefore = activityMoodDto.MoodIntensityBefore,
                    MoodIntensityAfter = activityMoodDto.MoodIntensityAfter
                };

                _context.ActivityMoods.Add(activityMood);
                await _context.SaveChangesAsync();

                response.Status = ServiceResponse.ServiceStatus.Created;
                response.CreatedId = activityMood.ActivityMoodId;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("There was an error adding the ActivityMood.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        // Update an existing ActivityMood
        public async Task<ServiceResponse> UpdateActivityMood(int id, ActivityMoodDto activityMoodDto)
        {
            ServiceResponse response = new();

            // Validate ActivityMood ID
            if (id != activityMoodDto.ActivityMoodId)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("ActivityMood ID mismatch.");
                return response;
            }

            var existingActivityMood = await _context.ActivityMoods.FindAsync(id);
            if (existingActivityMood == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("ActivityMood not found.");
                return response;
            }

            // Validate Activity and Mood exist
            var activity = await _context.Activities.FindAsync(activityMoodDto.ActivityId);
            var mood = await _context.Moods.FindAsync(activityMoodDto.MoodId);

            if (activity == null || mood == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Invalid ActivityId or MoodId.");
                return response;
            }

            try
            {
                // Update ActivityMood properties
                existingActivityMood.ActivityId = activity.ActivityId;
                existingActivityMood.MoodId = mood.MoodId;
                existingActivityMood.Activity = activity;
                existingActivityMood.Mood = mood;
                existingActivityMood.MoodIntensityBefore = activityMoodDto.MoodIntensityBefore;
                existingActivityMood.MoodIntensityAfter = activityMoodDto.MoodIntensityAfter;

                _context.Entry(existingActivityMood).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                response.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ActivityMoodExists(id))
                {
                    response.Status = ServiceResponse.ServiceStatus.NotFound;
                    response.Messages.Add("ActivityMood not found.");
                }
                else
                {
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    response.Messages.Add("Concurrency error updating the ActivityMood.");
                }
            }

            return response;
        }

        // Delete an ActivityMood
        public async Task<ServiceResponse> DeleteActivityMood(int id)
        {
            ServiceResponse response = new();

            var activityMood = await _context.ActivityMoods.FindAsync(id);
            if (activityMood == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("ActivityMood not found.");
                return response;
            }

            try
            {
                _context.ActivityMoods.Remove(activityMood);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error deleting the ActivityMood.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        // check if ActivityMood exists
        private async Task<bool> ActivityMoodExists(int id)
        {
            return await _context.ActivityMoods.AnyAsync(em => em.ActivityMoodId == id);
        }

        // List ActivityMoods for a specific Activity
        public async Task<IEnumerable<ActivityMoodDto>> ListActivityMoodsForActivity(int activityId)
        {
            List<ActivityMood> activityMoods = await _context.ActivityMoods
                .Where(em => em.ActivityId == activityId)
                .Include(em => em.Mood)
                .ToListAsync();

            List<ActivityMoodDto> activityMoodDtos = new();
            foreach (ActivityMood em in activityMoods)
            {
                activityMoodDtos.Add(new ActivityMoodDto()
                {
                    ActivityMoodId = em.ActivityMoodId,
                    ActivityId = em.ActivityId,
                    MoodId = em.MoodId,
                    ActivityName = em.Activity.ActivityName,
                    MoodName = em.Mood.MoodName,
                    ActivityDate = em.Activity.ActivityDate,
                    MoodIntensityBefore = em.MoodIntensityBefore,
                    MoodIntensityAfter = em.MoodIntensityAfter
                });
            }
            return activityMoodDtos;
        }


       
        public async Task<ServiceResponse> LinkActivityToMood(int activityId, int moodId, int? moodIntensityBefore, int? moodIntensityAfter)
        {
            ServiceResponse response = new();

            // check if the Activity and mood exist
            var activity = await _context.Activities.FindAsync(activityId);
            var mood = await _context.Moods.FindAsync(moodId);

            if (activity == null || mood == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                if (activity == null) response.Messages.Add("Activity not found.");
                if (mood == null) response.Messages.Add("Mood not found.");
                return response;
            }

            try
            {
               
                ActivityMood activityMood = new()
                {
                    ActivityId = activityId,
                    MoodId = moodId,
                    Activity = activity,  
                    Mood = mood,  
                    MoodIntensityBefore = moodIntensityBefore ?? 0,  
                    MoodIntensityAfter = moodIntensityAfter ?? 0  
                };

                // add to database
                _context.ActivityMoods.Add(activityMood);
                await _context.SaveChangesAsync();

                response.Status = ServiceResponse.ServiceStatus.Created;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error linking Activity to mood.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        // unlink Activity from mood
        public async Task<ServiceResponse> UnlinkActivityFromMood(int activityMoodId)
        {
            ServiceResponse response = new();

            var activityMood = await _context.ActivityMoods
                .FirstOrDefaultAsync(em => em.ActivityMoodId == activityMoodId);

            if (activityMood == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Link between Activity and mood not found.");
                return response;
            }

            try
            {
                _context.ActivityMoods.Remove(activityMood);
                await _context.SaveChangesAsync();

                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error unlinking Activity from mood.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }


    }
}
