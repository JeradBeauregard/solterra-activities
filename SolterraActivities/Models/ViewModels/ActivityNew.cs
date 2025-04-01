using System.Collections.Generic;

namespace SolterraActivities.Models.ViewModels
{
    public class ActivityNew
    {
        public ActivityDto Activity { get; set; }
        public IEnumerable<HobbyDto> HobbyOptions { get; set; } = new List<HobbyDto>();
        public IEnumerable<MoodDto> MoodOptions { get; set; } = new List<MoodDto>();   
    }
}
