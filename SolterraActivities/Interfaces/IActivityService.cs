using SolterraActivities.Models;

namespace SolterraActivities.Interfaces
{
    public interface IActivityService
    {
        // list all Activities (only ID and Name)
        Task<IEnumerable<ActivityDto>> ListActivities();

        // find a specific Activity (only ID and Name)
        Task<ActivityDto?> FindActivity(int id);

        // list Activities related to a specific hobby
        Task<IEnumerable<ActivityDto>> ListActivitiesForHobby(int hobbyId);

        //list activities related to a specific mood
        Task<IEnumerable<ActivityDto>> ListActivitiesForMood(int moodId);


        // add an activity
        Task<ServiceResponse> AddActivity(ActivityDto activityDto);

        // update an activity
        Task<ServiceResponse> UpdateActivity(int id, ActivityDto activityDto);

        // delete an activity
        Task<ServiceResponse> DeleteActivity(int id);

        // link an Activity to a hobby
        Task<ServiceResponse> LinkActivityToHobby(int activityId, int hobbyId);

        // unlink an activity from a hobby
        Task<ServiceResponse> UnlinkActivityFromHobby(int activityId);

        // update activity image
        Task<ServiceResponse> UpdateActivityImage(int id, IFormFile activityImage);

    }
}
