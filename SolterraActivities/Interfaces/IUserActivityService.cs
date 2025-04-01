using SolterraActivities.Models;

namespace SolterraActivities.Interfaces
{
    public interface IUserActivityService
    {
        // List all UserActivities
        Task<IEnumerable<UserActivityDto>> ListUserActivities();

        // Find a specific UserActivity
        Task<UserActivityDto?> FindUserActivity(int id);

        // Add a new UserActivity
        Task<ServiceResponse> AddUserActivity(UserActivityDto userActivityDto);

        // Update an existing UserActivity
        Task<ServiceResponse> UpdateUserActivity(int id, UserActivityDto userActivityDto);

        // Delete a UserActivity
        Task<ServiceResponse> DeleteUserActivity(int id);

        // List all UserActivities for a specific user
        Task<IEnumerable<UserActivityDto>> ListUserActivitiesForUser(int userId);

        // Link a user to an activity (add)
        Task<ServiceResponse> LinkUserToActivity(int userId, int activityId, int petId, int itemId);

        // Unlink (delete) a specific UserActivity record
        Task<ServiceResponse> UnlinkUserFromActivity(int userActivityId);
    }
}
