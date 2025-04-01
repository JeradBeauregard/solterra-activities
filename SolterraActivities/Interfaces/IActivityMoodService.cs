using SolterraActivities.Models;

namespace SolterraActivities.Interfaces
{
    public interface IActivityMoodService
    {
        // list all Activity moods
        Task<IEnumerable<ActivityMoodDto>> ListActivityMoods();

        // find a specific Activity mood
        Task<ActivityMoodDto?> FindActivityMood(int id);

        // add a new Activity mood
        Task<ServiceResponse> AddActivityMood(ActivityMoodDto activityMoodDto);

        // update an existing Activity mood
        Task<ServiceResponse> UpdateActivityMood(int id, ActivityMoodDto activityMoodDto);

        // delete an Activity mood
        Task<ServiceResponse> DeleteActivityMood(int id);

        // list moods for a specific Activity
        Task<IEnumerable<ActivityMoodDto>> ListActivityMoodsForActivity(int activityId);

        // link an Activity to a mood
        Task<ServiceResponse> LinkActivityToMood(int activityId, int moodId, int? moodIntensityBefore, int? moodIntensityAfter);

        // unlink an Activity from a mood
        Task<ServiceResponse> UnlinkActivityFromMood(int activityMoodId);


    }
}
