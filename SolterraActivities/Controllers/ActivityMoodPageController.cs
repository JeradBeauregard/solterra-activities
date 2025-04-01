using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using SolterraActivities.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace SolterraActivities.Controllers
{
    public class ActivityMoodPageController : Controller
    {
        private readonly IHobbyService _hobbyService;
        private readonly IActivityService _activityService;
        private readonly IMoodService _moodService;
        private readonly IActivityMoodService _activityMoodService;

        public ActivityMoodPageController(
            IHobbyService hobbyService,
            IActivityService activityService,
            IMoodService moodService,
            IActivityMoodService activityMoodService)
        {
            _hobbyService = hobbyService;
            _activityService = activityService;
            _moodService = moodService;
            _activityMoodService = activityMoodService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: ActivityMoodPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<ActivityMoodDto> activityMoodDtos = await _activityMoodService.ListActivityMoods();
            return View(activityMoodDtos);
        }

        // GET: ActivityMoodPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ActivityMoodDto? activityMoodDto = await _activityMoodService.FindActivityMood(id);

            if (activityMoodDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find ActivityMood"] });
            }

            return View(activityMoodDto);
        }

        // GET: ActivityMoodPage/New
        [Authorize]
        public async Task<IActionResult> New()
        {
            var activities = await _activityService.ListActivities();
            var moods = await _moodService.ListMoods();

            ActivityMoodNew options = new()
            {
                AllActivities = activities,
                AllMoods = moods
            };

            return View(options);
        }

        // POST: ActivityMoodPage/Add
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(ActivityMoodDto activityMoodDto)
        {
            ServiceResponse response = await _activityMoodService.AddActivityMood(activityMoodDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // GET: ActivityMoodPage/Edit/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            ActivityMoodDto? activityMoodDto = await _activityMoodService.FindActivityMood(id);
            var activities = await _activityService.ListActivities();
            var moods = await _moodService.ListMoods();

            if (activityMoodDto == null)
            {
                return View("Error");
            }

            ActivityMoodEdit options = new()
            {
                ActivityMood = activityMoodDto,
                AllActivities = activities,
                AllMoods = moods
            };

            return View(options);
        }

        // POST: ActivityMoodPage/UpdateActivityMood/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateActivityMood(int id, [FromForm] ActivityMoodDto activityMoodDto)
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








        // GET: ActivityMoodPage/ConfirmDelete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            ActivityMoodDto? activityMoodDto = await _activityMoodService.FindActivityMood(id);

            if (activityMoodDto == null)
            {
                return View("Error");
            }

            return View(activityMoodDto);
        }

        // POST: ActivityMoodPage/Delete/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _activityMoodService.DeleteActivityMood(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }
    }
}
