using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Interfaces;
using SolterraActivities.Models.ViewModels;
using SolterraActivities.Models;
using Microsoft.AspNetCore.Authorization;
using SolterraActivities.Services;
using System.Diagnostics;

namespace SolterraActivities.Controllers
{
    public class ActivityPageController : Controller
    { 
        private readonly IActivityService _activityService;
        private readonly IHobbyService _hobbyService;
        private readonly IMoodService _moodService;
        private readonly IActivityMoodService _activityMoodService;

        // Dependency injection of services
        public ActivityPageController(
            IActivityService activityService, 
            IHobbyService hobbyService, 
            IMoodService moodService, 
            IActivityMoodService activityMoodService)
        {
            _activityService = activityService;
            _hobbyService = hobbyService;
            _moodService = moodService;
            _activityMoodService = activityMoodService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: ActivityPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<ActivityDto> activities = await _activityService.ListActivities();
            return View(activities);
        }

        // GET: ActivityPage/Details/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            ActivityDto? activity = await _activityService.FindActivity(id);

            if (activity == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Activity not found."] });
            }

            // All hobbies
            IEnumerable<HobbyDto> hobbyOptions = await _hobbyService.ListHobbies();

            //  all moods
            IEnumerable<MoodDto> moodOptions = await _moodService.ListMoods();

        //  all moods already linked to this activity
            IEnumerable<ActivityMoodDto> activityMoods = await _activityMoodService.ListActivityMoodsForActivity(id);

            // Pass all data to the ViewModel
            ActivityDetails activityInfo = new ActivityDetails()
            {
                Activity = activity,
                HobbyOptions = hobbyOptions,
                MoodOptions = moodOptions,
                ActivityMoods = activityMoods
            };

            return View(activityInfo);
        }

        // GET: ActivityPage/Edit/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            // Retrieve the activity by ID
            ActivityDto? activity = await _activityService.FindActivity(id);

            // Get all available hobbies and moods
            IEnumerable<HobbyDto> hobbies = await _hobbyService.ListHobbies();
            IEnumerable<MoodDto> moods = await _moodService.ListMoods();
            
            
            IEnumerable<ActivityMoodDto> activityMoods = await _activityMoodService.ListActivityMoodsForActivity(id);

            // If activity is not found, return an error page
            if (activity == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Activity not found."] });
            }

            
            ActivityDetails activityInfo = new ActivityDetails()
            {
                Activity = activity,
                HobbyOptions = hobbies,
                MoodOptions = moods,
                ActivityMoods = activityMoods
            };

            return View(activityInfo);
        }


        // POST: ActivityPage/Update/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int id, ActivityDto activityDto)
        {
            if (id != activityDto.ActivityId)
            {
                activityDto.ActivityId = id; 
            }

            ServiceResponse response = await _activityService.UpdateActivity(id, activityDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }


        // POST: ActivityPage/UpdateHobby
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateHobby(int activityId, int hobbyId)
        {
            ServiceResponse response = await _activityService.LinkActivityToHobby(activityId, hobbyId);

            return RedirectToAction("Details", new { id =activityId });
        }

        // POST: ActivityPage/LinkMood
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LinkMood(int activityId, int moodId, int? beforeIntensity, int? afterIntensity)
        {
            ServiceResponse response = await _activityMoodService.LinkActivityToMood(activityId, moodId, beforeIntensity, afterIntensity);

            return RedirectToAction("Details", new { id = activityId });
        }

        // POST: ActivityPage/UnlinkMood
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UnlinkMood(int activityMoodId, int activityId)
        {
            ServiceResponse response = await _activityMoodService.UnlinkActivityFromMood(activityMoodId);

            return RedirectToAction("Details", new { id = activityId });
        }


        // POST: ActivityMoodPage/UpdateActivityMood/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateActivityMood(int id, ActivityMoodDto activityMoodDto)
        {
            if (id != activityMoodDto.ActivityMoodId)
            {
                return BadRequest(new { message = "ActivityMood ID mismatch." });
            }

            var response = await _activityMoodService.UpdateActivityMood(id, activityMoodDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { message = "ActivityMood not found." });
            }
            else if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(new { message = "Error updating ActivityMood." });
            }

            return RedirectToAction("Edit", new { success = true, message = "Mood updated successfully!" });
        }



        // GET: ActivityPage/New
        [Authorize]
        public async Task<IActionResult> New()
        {
            IEnumerable<HobbyDto> hobbies = await _hobbyService.ListHobbies();
            IEnumerable<MoodDto> moods = await _moodService.ListMoods();

            ActivityNew activityNew = new ActivityNew()
            {
                HobbyOptions = hobbies,
                MoodOptions = moods
            };

            return View(activityNew);
        }

        // POST: ActivityPage/Add
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(ActivityDto activityDto)
        {
            ServiceResponse response = await _activityService.AddActivity(activityDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "ActivityPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // GET: ActivityPage/ConfirmDelete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            ActivityDto? activity = await _activityService.FindActivity(id);
            if (activity == null)
            {
                return View("Error");
            }
            else
            {
                return View(activity);
            }
        }

        // POST: ActivityPage/Delete/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _activityService.DeleteActivity(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "ActivityPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateActivityImage(int id, IFormFile activityImage)
        {
            if (id <= 0)
            {
                return View("Error", new ErrorViewModel() { Errors = [$"Invalid Activity ID: {id}"] });
            }

            ServiceResponse response = await _activityService.UpdateActivityImage(id, activityImage);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }


    }
}
