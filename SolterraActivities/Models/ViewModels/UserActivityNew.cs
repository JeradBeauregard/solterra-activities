using Microsoft.AspNetCore.Mvc.Rendering;

namespace SolterraActivities.Models.ViewModels
{
    public class UserActivityNew
    {
        public UserActivityDto UserActivity { get; set; } = new UserActivityDto();

        public IEnumerable<SelectListItem> AllUsers { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AllActivities { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AllPets { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> AllItems { get; set; } = new List<SelectListItem>();
    }
}
