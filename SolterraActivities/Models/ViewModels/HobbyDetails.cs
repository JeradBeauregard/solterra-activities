namespace SolterraActivities.Models.ViewModels
{
    public class HobbyDetails
    {
        public HobbyDto Hobby { get; set; } = null!;
        public IEnumerable<ActivityDto> HobbyActivities { get; set; } = new List<ActivityDto>();
        public IEnumerable<MoodDto> AllMoods { get; set; } = new List<MoodDto>();
        public IEnumerable<ActivityMoodDto> ActivityMoods { get; set; } = new List<ActivityMoodDto>();
    }
}
