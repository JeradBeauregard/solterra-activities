using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using SolterraActivities.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace SolterraActivities.Controllers
{
    public class HobbyPageController : Controller
    {
        private readonly IHobbyService _hobbyService;
        private readonly IActivityService _ActivityService;
        private readonly IMoodService _moodService;
        private readonly IActivityMoodService _ActivityMoodService;

        // Dependency injection of service interfaces
        public HobbyPageController(
            IHobbyService hobbyService, 
            IActivityService ActivityService,
            IMoodService moodService,
            IActivityMoodService ActivityMoodService)
        {
            _hobbyService = hobbyService;
            _ActivityService = ActivityService;
            _moodService = moodService;
            _ActivityMoodService = ActivityMoodService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: HobbyPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<HobbyDto?> hobbyDtos = await _hobbyService.ListHobbies();
            return View(hobbyDtos);
        }

        // GET: HobbyPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            HobbyDto? hobby = await _hobbyService.FindHobby(id);
            IEnumerable<ActivityDto> associatedActivities = await _ActivityService.ListActivitiesForHobby(id);
            IEnumerable<MoodDto> allMoods = await _moodService.ListMoods();
            IEnumerable<ActivityMoodDto> ActivityMoods = await _ActivityMoodService.ListActivityMoods();

            if (hobby == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Sorry, Hobby not found."] });
            }
            else
            {
                HobbyDetails hobbyDetails = new HobbyDetails()
                {
                    Hobby = hobby,
                    HobbyActivities = associatedActivities,
                    AllMoods = allMoods,
                    ActivityMoods = ActivityMoods
                };
                return View(hobbyDetails);
            }
        }

        // GET: HobbyPage/New

        public ActionResult New()
        {
            return View();
        }

        // POST: HobbyPage/Add
        [HttpPost]
        public async Task<IActionResult> Add(HobbyDto hobbyDto)
        {
            ServiceResponse response = await _hobbyService.AddHobby(hobbyDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "HobbyPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // GET: HobbyPage/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            HobbyDto? hobby = await _hobbyService.FindHobby(id);
            if (hobby == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Hobby not found."] });
            }
            return View(hobby);  
        }


        // POST: HobbyPage/Update/{id}
        [HttpPost]
        public async Task<IActionResult> Update(int id, HobbyDto hobbyDto)
        {
            if (id != hobbyDto.HobbyId)  
            {
                return View("Error", new ErrorViewModel() { Errors = ["Hobby ID mismatch."] });
            }

            ServiceResponse response = await _hobbyService.UpdateHobby(id, hobbyDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", "HobbyPage", new { id = id });
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }



        // GET: HobbyPage/ConfirmDelete/{id}
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            HobbyDto? hobbyDto = await _hobbyService.FindHobby(id);
            if (hobbyDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(hobbyDto);
            }
        }

        // POST: HobbyPage/Delete/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _hobbyService.DeleteHobby(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "HobbyPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

        // POST: HobbyPage/LinkToActivity
        [HttpPost]
        public async Task<IActionResult> LinkToActivity([FromForm] int hobbyId, [FromForm] int ActivityId)
        {
            await _hobbyService.LinkHobbyToActivity(hobbyId, ActivityId);

            return RedirectToAction("Details", new { id = hobbyId });
        }

        // POST: HobbyPage/UnlinkFromActivity
        [HttpPost]
        public async Task<IActionResult> UnlinkFromActivity([FromForm] int hobbyId, [FromForm] int ActivityId)
        {
            await _hobbyService.UnlinkHobbyFromActivity(hobbyId, ActivityId);

            return RedirectToAction("Details", new { id = hobbyId });
        }
    }
}
