using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using SolterraActivities.Data;
using Microsoft.EntityFrameworkCore;

namespace SolterraActivities.Services
{
    public class UserActivityService : IUserActivityService
    {
        private readonly ApplicationDbContext _context;

        public UserActivityService(ApplicationDbContext context)
        {
            _context = context;
        }

        // List all UserActivities
        public async Task<IEnumerable<UserActivityDto>> ListUserActivities()
        {
            var userActivities = await _context.UserActivities
                .Include(ua => ua.User)
                .Include(ua => ua.Activity)
                    .ThenInclude(a => a.ActivityMoods)
                        .ThenInclude(am => am.Mood)
                .Include(ua => ua.Pet)
                .Include(ua => ua.Item)
                .ToListAsync();

            return userActivities.Select(ua => new UserActivityDto
            {
                UserActivityId = ua.UserActivityId,
                UserId = ua.UserId,
                ActivityId = ua.ActivityId,
                PetId = ua.PetId,
                ItemId = ua.ItemId,
                Username = ua.User?.Username,
                ActivityName = ua.Activity?.ActivityName,
                ActivityDate = ua.Activity?.ActivityDate,
                ActivityCost = ua.Activity?.ActivityCost,
                ActivityDurationInHours = ua.Activity?.DurationInHours,
                PetName = ua.Pet?.Name,
                PetMoodAfterActivity = string.Join(", ", ua.Activity?.ActivityMoods.Select(am => am.Mood.MoodName) ?? []),
                ItemGained = ua.Item?.Name
            }).ToList();
        }

        // Find a single UserActivity
        public async Task<UserActivityDto?> FindUserActivity(int id)
        {
            var ua = await _context.UserActivities
                .Include(ua => ua.User)
                .Include(ua => ua.Activity)
                    .ThenInclude(a => a.ActivityMoods)
                        .ThenInclude(am => am.Mood)
                .Include(ua => ua.Pet)
                .Include(ua => ua.Item)
                .FirstOrDefaultAsync(ua => ua.UserActivityId == id);

            if (ua == null) return null;

            return new UserActivityDto
            {
                UserActivityId = ua.UserActivityId,
                UserId = ua.UserId,
                ActivityId = ua.ActivityId,
                PetId = ua.PetId,
                ItemId = ua.ItemId,
                Username = ua.User?.Username,
                ActivityName = ua.Activity?.ActivityName,
                ActivityDate = ua.Activity?.ActivityDate,
                ActivityCost = ua.Activity?.ActivityCost,
                ActivityDurationInHours = ua.Activity?.DurationInHours,
                PetName = ua.Pet?.Name,
                PetMoodAfterActivity = string.Join(", ", ua.Activity?.ActivityMoods.Select(am => am.Mood.MoodName) ?? []),
                ItemGained = ua.Item?.Name
            };
        }

        // Add a new UserActivity
        public async Task<ServiceResponse> AddUserActivity(UserActivityDto dto)
        {
            ServiceResponse response = new();

            var user = await _context.Users.FindAsync(dto.UserId);
            var activity = await _context.Activities
                .Include(a => a.ActivityMoods)
                .FirstOrDefaultAsync(a => a.ActivityId == dto.ActivityId);
            var pet = await _context.Pets.FindAsync(dto.PetId);
            var item = await _context.Items.FindAsync(dto.ItemId);

            if (user == null || activity == null || pet == null || item == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                if (user == null) response.Messages.Add("User not found.");
                if (activity == null) response.Messages.Add("Activity not found.");
                if (pet == null) response.Messages.Add("Pet not found.");
                if (item == null) response.Messages.Add("Item not found.");
                return response;
            }

            try
            {
                var ua = new UserActivity
                {
                    UserId = user.Id,
                    ActivityId = activity.ActivityId,
                    PetId = pet.Id,
                    ItemId = item.Id,
                    User = user,
                    Activity = activity,
                    Pet = pet,
                    Item = item
                };

                _context.UserActivities.Add(ua);
                await _context.SaveChangesAsync();

                response.Status = ServiceResponse.ServiceStatus.Created;
                response.CreatedId = ua.UserActivityId;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error adding UserActivity.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        // Update an existing UserActivity
        public async Task<ServiceResponse> UpdateUserActivity(int id, UserActivityDto dto)
        {
            ServiceResponse response = new();

            if (id != dto.UserActivityId)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("UserActivity ID mismatch.");
                return response;
            }

            var ua = await _context.UserActivities.FindAsync(id);
            if (ua == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("UserActivity not found.");
                return response;
            }

            var user = await _context.Users.FindAsync(dto.UserId);
            var activity = await _context.Activities.FindAsync(dto.ActivityId);
            var pet = await _context.Pets.FindAsync(dto.PetId);
            var item = await _context.Items.FindAsync(dto.ItemId);

            if (user == null || activity == null || pet == null || item == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                if (user == null) response.Messages.Add("User not found.");
                if (activity == null) response.Messages.Add("Activity not found.");
                if (pet == null) response.Messages.Add("Pet not found.");
                if (item == null) response.Messages.Add("Item not found.");
                return response;
            }

            try
            {
                ua.UserId = user.Id;
                ua.ActivityId = activity.ActivityId;
                ua.PetId = pet.Id;
                ua.ItemId = item.Id;
                ua.User = user;
                ua.Activity = activity;
                ua.Pet = pet;
                ua.Item = item;

                _context.Entry(ua).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                response.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserActivityExists(id))
                {
                    response.Status = ServiceResponse.ServiceStatus.NotFound;
                    response.Messages.Add("UserActivity not found.");
                }
                else
                {
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    response.Messages.Add("Concurrency error updating the UserActivity.");
                }
            }

            return response;
        }

        // Delete a UserActivity
        public async Task<ServiceResponse> DeleteUserActivity(int id)
        {
            ServiceResponse response = new();

            var ua = await _context.UserActivities.FindAsync(id);
            if (ua == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("UserActivity not found.");
                return response;
            }

            try
            {
                _context.UserActivities.Remove(ua);
                await _context.SaveChangesAsync();

                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (Exception ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error deleting UserActivity.");
                response.Messages.Add(ex.Message);
            }

            return response;
        }

        // List activities for a specific user
        public async Task<IEnumerable<UserActivityDto>> ListUserActivitiesForUser(int userId)
        {
            var userActivities = await _context.UserActivities
                .Where(ua => ua.UserId == userId)
                .Include(ua => ua.Activity)
                    .ThenInclude(a => a.ActivityMoods)
                        .ThenInclude(am => am.Mood)
                .Include(ua => ua.Pet)
                .Include(ua => ua.Item)
                .Include(ua => ua.User)
                .ToListAsync();

            return userActivities.Select(ua => new UserActivityDto
            {
                UserActivityId = ua.UserActivityId,
                UserId = ua.UserId,
                ActivityId = ua.ActivityId,
                PetId = ua.PetId,
                ItemId = ua.ItemId,
                Username = ua.User?.Username,
                ActivityName = ua.Activity?.ActivityName,
                ActivityDate = ua.Activity?.ActivityDate,
                ActivityCost = ua.Activity?.ActivityCost,
                ActivityDurationInHours = ua.Activity?.DurationInHours,
                PetName = ua.Pet?.Name,
                PetMoodAfterActivity = string.Join(", ", ua.Activity?.ActivityMoods.Select(am => am.Mood.MoodName) ?? []),
                ItemGained = ua.Item?.Name
            }).ToList();
        }

        // Helper
        private async Task<bool> UserActivityExists(int id)
        {
            return await _context.UserActivities.AnyAsync(ua => ua.UserActivityId == id);
        }

        // Link a user to an activity 
        public async Task<ServiceResponse> LinkUserToActivity(int userId, int activityId, int petId, int itemId)
        {
            var dto = new UserActivityDto
            {
                UserId = userId,
                ActivityId = activityId,
                PetId = petId,
                ItemId = itemId
            };
            return await AddUserActivity(dto);
        }

        // Unlink (delete) a UserActivity
        public async Task<ServiceResponse> UnlinkUserFromActivity(int userActivityId)
        {
            return await DeleteUserActivity(userActivityId);
        }
    }
}
