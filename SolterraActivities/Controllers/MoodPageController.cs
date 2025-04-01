using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Interfaces;
using SolterraActivities.Models.ViewModels;
using SolterraActivities.Models;
using Microsoft.AspNetCore.Authorization;

namespace SolterraActivities.Controllers
{
    public class MoodPageController : Controller
    {
        private readonly IMoodService _moodService;
        private readonly IActivityService _ActivityService;

        public MoodPageController(IMoodService moodService, IActivityService ActivityService)
        {
            _moodService = moodService;
            _ActivityService = ActivityService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: MoodPage/List
        public async Task<IActionResult> List()
        {
            IEnumerable<MoodDto?> moodDtos = await _moodService.ListMoods();
            return View(moodDtos);
        }

        // GET: MoodPage/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            MoodDto? moodDto = await _moodService.FindMood(id);
            IEnumerable<ActivityDto> relatedActivities = await _ActivityService.ListActivitiesForMood(id);

            if (moodDto == null)
            {
                return View("Error", new ErrorViewModel() { Errors = ["Could not find mood"] });
            }

            MoodDetails moodInfo = new MoodDetails()
            {
                Mood = moodDto,
                RelatedActivities = relatedActivities
            };

            return View(moodInfo);
        }

        // GET: MoodPage/New
        [Authorize]
        public ActionResult New()
        {
            return View();
        }

        // POST: MoodPage/Add
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Add(MoodDto moodDto)
        {
            ServiceResponse response = await _moodService.AddMood(moodDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", new { id = response.CreatedId });
            }
            return View("Error", new ErrorViewModel() { Errors = response.Messages });
        }

        // GET: MoodPage/Edit/{id}
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            MoodDto? moodDto = await _moodService.FindMood(id);
            if (moodDto == null)
            {
                return View("Error");
            }
            return View(moodDto);
        }

        // POST: MoodPage/Update/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int id, MoodDto moodDto)
        {
            ServiceResponse response = await _moodService.UpdateMood(id, moodDto);

            if (response.Status == ServiceResponse.ServiceStatus.Updated)
            {
                return RedirectToAction("Details", new { id });
            }
            return View("Error", new ErrorViewModel() { Errors = response.Messages });
        }

        // GET: MoodPage/ConfirmDelete/{id}
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            MoodDto? moodDto = await _moodService.FindMood(id);
            if (moodDto == null)
            {
                return View("Error");
            }
            else
            {
                return View(moodDto);
            }
        }

        // POST: MoodPage/Delete/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            ServiceResponse response = await _moodService.DeleteMood(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List", "MoodPage");
            }
            else
            {
                return View("Error", new ErrorViewModel() { Errors = response.Messages });
            }
        }

    }
}
