using SolterraActivities.Models;

namespace SolterraActivities.Interfaces
{
    public interface IHobbyService
    {
        // list all hobbies
        Task<IEnumerable<HobbyDto>> ListHobbies();

        // find a specific hobby by ID
        Task<HobbyDto?> FindHobby(int id);

        // list activity related to a specific hobby
        Task<IEnumerable<ActivityDto>> ListActivitiesForHobby(int hobbyId);

        // add a new hobby
        Task<ServiceResponse> AddHobby(HobbyDto hobbyDto);

        // update an existing hobby
        Task<ServiceResponse> UpdateHobby(int id, HobbyDto hobbyDto);

        // delete a hobby by ID
        Task<ServiceResponse> DeleteHobby(int id);

        // link a hobby to an activity
        Task<ServiceResponse> LinkHobbyToActivity(int hobbyId, int activityId);

        // unlink a hobby from an activity
        Task<ServiceResponse> UnlinkHobbyFromActivity(int hobbyId, int activityId);
    }
}
