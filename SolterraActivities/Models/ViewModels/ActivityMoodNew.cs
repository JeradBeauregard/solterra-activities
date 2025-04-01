using SolterraActivities.Models;

namespace SolterraActivities.Models.ViewModels
{
    public class ActivityMoodNew
    {
        public IEnumerable<ActivityDto> AllActivities { get; set; } = new List<ActivityDto>();
        public IEnumerable<MoodDto> AllMoods { get; set; } = new List<MoodDto>();
    }
}
