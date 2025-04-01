using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using SolterraActivities.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Microsoft.AspNetCore.Mvc;
//using System.Diagnostics;

namespace SolterraActivities.Services
{
    public class ActivityService : IActivityService
    {
        private readonly ApplicationDbContext _context;

        // Dependency injection of database context
        public ActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        // list all Activities
        public async Task<IEnumerable<ActivityDto>> ListActivities()
        {
            // all activities
            List<Activity> activities = await _context.Activities
                .Include(a => a.Hobby)
                .ToListAsync();

            // empty list of data transfer object ActivityDto
            List<ActivityDto> activityDtos = new List<ActivityDto>();

            // foreach Activity record in database
            foreach (Activity activity in activities)
            {
                // create new instance of ActivityDto, add to list
                activityDtos.Add(new ActivityDto()
                {
                    
                    ActivityId = activity.ActivityId,
                    ActivityName = activity.ActivityName,
                    HobbyId = activity.HobbyId,
                    HobbyName = activity.Hobby != null ? activity.Hobby.HobbyName : "No Hobby Associated",
                    ActivityDate = activity.ActivityDate,
                    ActivityLocation = activity.ActivityLocation,
                    DurationinHours = activity.DurationInHours,
                    ActivityCost = activity.ActivityCost,
                    //ActivityMoods = activity.ActivityMoods
                    //    .Where(em => em.ActivityId == activity.ActivityId)
                    //    .Select(em => em.Mood.MoodName)
                    //    .ToList() ?? new List<string>()



                });
            }
            // return ActivityDtos
            return activityDtos;
        }

        // find a specific activity 
        public async Task<ActivityDto?> FindActivity(int id)
        {
            var activity = await _context.Activities
                .Include(a => a.Hobby)
                .Include(a => a.ActivityMoods)
                    .ThenInclude(am => am.Mood)
                .FirstOrDefaultAsync(a => a.ActivityId == id);

            if (activity == null)
            {
                return null;
            }

            return new ActivityDto()
            {
                ActivityId = activity.ActivityId,
                ActivityName = activity.ActivityName,
                HobbyId = activity.HobbyId,
                HobbyName = activity.Hobby?.HobbyName,
                ActivityCost = activity.ActivityCost,
                DurationinHours = activity.DurationInHours,
                ActivityDate = activity.ActivityDate,
                ActivityLocation = activity.ActivityLocation
                //ActivityMoods = activity.ActivityMoods
                //    .Select(em => new ActivityMoodDto()
                //    {
                //        ActivityMoodId = em.ActivityMoodId,
                //        MoodId = em.MoodId,
                //        MoodName = em.Mood.MoodName,
                //        MoodIntensityBefore = em.MoodIntensityBefore,
                //        MoodIntensityAfter = em.MoodIntensityAfter
                //    }).ToList()
            };
        }


        // list activities related to a specific hobby
        public async Task<IEnumerable<ActivityDto>> ListActivitiesForHobby(int hobbyId)
        {
            // where activities have HobbyId = {hobbyId}
            List<Activity> activities = await _context.Activities
                .Where(a => a.HobbyId == hobbyId)
                .ToListAsync();

            // empty list of data transfer object ActivityDto
            List<ActivityDto> activityDtos = new List<ActivityDto>();

            //foreach Activity record in database
            foreach (Activity activity in activities)
            {
                // create new instance of ActivityDto, add to list
                activityDtos.Add(new ActivityDto()
                {
                    ActivityId = activity.ActivityId,
                    ActivityName = activity.ActivityName,
                    HobbyId = activity.HobbyId,
                    HobbyName = activity.Hobby?.HobbyName,
                    ActivityCost = activity.ActivityCost,
                    DurationinHours = activity.DurationInHours,
                    ActivityDate = activity.ActivityDate,
                    ActivityLocation = activity.ActivityLocation
                    //ActivityMoods = _context.ActivityMoods
                    //    .Where(em => em.ActivityId == activity.ActivityId)
                    //    .Select(em => em.Mood.MoodName)
                    //    .ToList()
                });
            }
            
            return activityDtos;
        }

        // list activities related to a specific mood
        public async Task<IEnumerable<ActivityDto>> ListActivitiesForMood(int moodId)
        {
            List<ActivityMood> activityMoods = await _context.ActivityMoods
                .Include(em => em.Activity)
                .Where(em => em.MoodId == moodId)
                .ToListAsync();

            return activityMoods.Select(em => new ActivityDto
            {
                ActivityId = em.Activity.ActivityId,
                ActivityName = em.Activity.ActivityName,
                HobbyId = em.Activity.HobbyId,
                HobbyName = em.Activity.Hobby?.HobbyName,
                ActivityCost = em.Activity.ActivityCost,
                DurationinHours = em.Activity.DurationInHours,
                ActivityDate = em.Activity.ActivityDate,
                ActivityLocation = em.Activity.ActivityLocation
            }).ToList();
        }


        // add an activity
        public async Task<ServiceResponse> AddActivity(ActivityDto activityDto)
        {
            ServiceResponse serviceResponse = new();

            // Ensure activity links to a valid Hobby
            var hobby = await _context.Hobbies.FindAsync(activityDto.HobbyId);
            if (hobby == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Hobby not found.");
                return serviceResponse;
            }

            // Create an instance of Activity
            Activity activity = new Activity()
            {
                ActivityName = activityDto.ActivityName,
                HobbyId = hobby.HobbyId,
                ActivityCost = activityDto.ActivityCost,
                DurationInHours = activityDto.DurationinHours,
                ActivityDate = activityDto.ActivityDate,
                ActivityLocation = activityDto.ActivityLocation
            };

            try
            {
                _context.Activities.Add(activity);
                await _context.SaveChangesAsync(); 
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an error adding the activity.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            serviceResponse.CreatedId = activity.ActivityId;
            return serviceResponse;
        }



        // update an Activity
        public async Task<ServiceResponse> UpdateActivity(int id, ActivityDto activityDto)
        {
            ServiceResponse serviceResponse = new();

            if (id != activityDto.ActivityId)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Activity ID mismatch.");
                return serviceResponse;
            }

            var existingActivity = await _context.Activities
                .Include(a => a.ActivityMoods)
                .FirstOrDefaultAsync(a => a.ActivityId == id);

            if (existingActivity == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Activity not found.");
                return serviceResponse;
            }

            _context.Entry(existingActivity).State = EntityState.Detached;

            var hobby = await _context.Hobbies.FindAsync(activityDto.HobbyId);
            if (hobby == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Hobby not found.");
                return serviceResponse;
            }

            // update Activity fields
            existingActivity.ActivityName = activityDto.ActivityName;
            existingActivity.HobbyId = hobby.HobbyId;
            existingActivity.ActivityCost = activityDto.ActivityCost;
            existingActivity.DurationInHours = activityDto.DurationinHours;
            existingActivity.ActivityDate = activityDto.ActivityDate;
            existingActivity.ActivityLocation = activityDto.ActivityLocation;

            // mood intensity update
            foreach (var moodDto in activityDto.ActivityMoods)
            {
                var existingMood = _context.ActivityMoods
                    .FirstOrDefault(em => em.ActivityMoodId == moodDto.ActivityMoodId);

                if (existingMood != null)
                {
                    existingMood.MoodIntensityBefore = moodDto.MoodIntensityBefore;
                    existingMood.MoodIntensityAfter = moodDto.MoodIntensityAfter;

                    _context.Entry(existingMood).State = EntityState.Modified;
                }
            }

            try
            {
                _context.Activities.Attach(existingActivity);
                _context.Entry(existingActivity).State = EntityState.Modified;

                await _context.SaveChangesAsync();

                serviceResponse.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (DbUpdateConcurrencyException)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("An error occurred updating the record.");
            }

            return serviceResponse;
        }




        // delete an Activity
        public async Task<ServiceResponse> DeleteActivity(int id)
        {
            ServiceResponse serviceResponse = new();

            // Activity must exist in the first place
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Activity cannot be deleted because it does not exist.");
                return serviceResponse;
            }

            try
            {
                // delete related ActivityMoods
                var activityMoods = _context.ActivityMoods.Where(em => em.ActivityId == id);
                _context.ActivityMoods.RemoveRange(activityMoods);

                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("Error encountered while deleting the activity.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            return serviceResponse;
        }

        // link an activity to a hobby
        public async Task<ServiceResponse> LinkActivityToHobby(int ActivityId, int hobbyId)
        {
            ServiceResponse serviceResponse = new();

            Activity? activity = await _context.Activities.FindAsync(ActivityId);
            Hobby? hobby = await _context.Hobbies.FindAsync(hobbyId);

            // data must link to a valid entity
            if (activity == null || hobby == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                if (activity == null) serviceResponse.Messages.Add("Activity was not found.");
                if (hobby == null) serviceResponse.Messages.Add("Hobby was not found.");
                return serviceResponse;
            }

            try
            {
                activity.HobbyId = hobby.HobbyId;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an issue linking the activity to the hobby.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Created;
            return serviceResponse;
        }

        // unlink an Activity from a hobby
        public async Task<ServiceResponse> UnlinkActivityFromHobby(int activityId)
        {
            ServiceResponse serviceResponse = new();

            Activity? activity = await _context.Activities.FindAsync(activityId);

            // data must link to a valid entity
            if (activity == null)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.NotFound;
                serviceResponse.Messages.Add("Activity was not found.");
                return serviceResponse;
            }

            try
            {

                activity.HobbyId = 0;  
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Status = ServiceResponse.ServiceStatus.Error;
                serviceResponse.Messages.Add("There was an issue unlinking the activity from the hobby.");
                serviceResponse.Messages.Add(ex.Message);
                return serviceResponse;
            }

            serviceResponse.Status = ServiceResponse.ServiceStatus.Deleted;
            return serviceResponse;
        }

        // check if activity exists
        private async Task<bool> ActivityExists(int id)
        {
            return await _context.Activities.AnyAsync(a => a.ActivityId == id);
        }



        // update an activity image
        // to do: fix the issue of image uploading but not visible on views
        public async Task<ServiceResponse> UpdateActivityImage(int activityId, IFormFile activityImage)
        {
            ServiceResponse response = new();

            // Find the Activity in the database
            var activity = await _context.Activities.FindAsync(activityId);
            if (activity == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add($"Activity {activityId} not found");
                return response;
            }

            // Validate image presence and content
            if (activityImage == null || activityImage.Length == 0)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("No image provided or file is empty");
                return response;
            }

            // Define allowed file extensions
            List<string> allowedExtensions = new() { ".jpeg", ".jpg", ".png", ".gif" };
            string imageExtension = Path.GetExtension(activityImage.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(imageExtension))
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add($"{imageExtension} is not a valid file extension");
                return response;
            }

            // Remove old image if it exists
            if (!string.IsNullOrEmpty(activity.ActivityImagePath))
            {
                string oldFilePath = Path.Combine("wwwroot/images/Activitys/", activity.ActivityImagePath);
                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }

            // Generate and save new file
            string fileName = $"{activityId}{imageExtension}";
            string filePath = Path.Combine("wwwroot/images/activities/", fileName);

            using (var stream = File.Create(filePath))
            {
                await activityImage.CopyToAsync(stream);
            }

            // Confirm successful upload
            if (File.Exists(filePath))
            {
                activity.ActivityImagePath = fileName;
                _context.Entry(activity).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    response.Messages.Add("An error occurred updating the record");
                    return response;
                }
            }

            response.Status = ServiceResponse.ServiceStatus.Updated;
            response.Messages.Add("Image uploaded successfully");
            return response;
        }




    }
}
