namespace SolterraActivities.Models.ViewModels
{
    public class ActivityEdit
    {
        public ActivityDto Activity { get; set; }
        public IEnumerable<HobbyDto> HobbyOptions { get; set; }
        public List<MoodDto> MoodOptions { get; set; } = new List<MoodDto>();
        public List<ActivityMoodDto> ActivityMoods { get; set; } = new List<ActivityMoodDto>();

    }
}
