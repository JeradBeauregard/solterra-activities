using SolterraActivities.Models;

namespace SolterraActivities.Interfaces
{
    public interface IMoodService
    {
        // list all moods
        Task<IEnumerable<MoodDto>> ListMoods();

        // find a specific mood
        Task<MoodDto?> FindMood(int id);

        // list activities for a specific mood
        Task<IEnumerable<ActivityDto>> ListActivitiesForMood(int moodId);

        // list moods for a specific activities
        Task<IEnumerable<MoodDto>> ListMoodsForActivity(int experienceId);


        // add a mood
        Task<ServiceResponse> AddMood(MoodDto moodDto);

        // update a mood
        Task<ServiceResponse> UpdateMood(int id, MoodDto moodDto);

        // delete a mood
        Task<ServiceResponse> DeleteMood(int id);
    }
}
