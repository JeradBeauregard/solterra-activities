namespace SolterraActivities.Models.ViewModels
{
    public class MoodDetails
    {
        public MoodDto Mood { get; set; }
        public IEnumerable<ActivityDto> RelatedActivities { get; set; } = new List<ActivityDto>();
    }
}

