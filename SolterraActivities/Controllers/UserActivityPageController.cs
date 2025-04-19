using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;
using SolterraActivities.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SolterraActivities.Controllers
{
    public class UserActivityPageController : Controller
    {
        private readonly IUserActivityService _userActivityService;
        private readonly IUserService _userService;
        private readonly IActivityService _activityService;
        private readonly IPetService _petService;
        private readonly IItemService _itemService;

        public UserActivityPageController(
            IUserActivityService userActivityService,
            IUserService userService,
            IActivityService activityService,
            IPetService petService,
            IItemService itemService)
        {
            _userActivityService = userActivityService;
            _userService = userService;
            _activityService = activityService;
            _petService = petService;
            _itemService = itemService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        // GET: UserActivityPage/List
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> List()
        {
            var userActivities = await _userActivityService.ListUserActivities();
            return View(userActivities);
        }

        // GET: UserActivityPage/Details/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var userActivity = await _userActivityService.FindUserActivity(id);
            if (userActivity == null)
            {
                return View("Error", new ErrorViewModel { Errors = ["Could not find UserActivity."] });
            }

            return View(userActivity);
        }

        // GET: UserActivityPage/New
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> New()
        {
            var users = await _userService.GetUsers();
            var activities = await _activityService.ListActivities();
            var pets = await _petService.ListPets();
            var items = await _itemService.GetItems();

            UserActivityNew options = new()
            {
                AllUsers = users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Username
                }),

                AllActivities = activities.Select(a => new SelectListItem
                {
                    Value = a.ActivityId.ToString(),
                    Text = a.ActivityName
                }),

                AllPets = pets.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }),

                AllItems = items.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name
                })
            };

            return View(options);
        }

        // POST: UserActivityPage/Add
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(UserActivityDto userActivityDto)
        {
            var response = await _userActivityService.AddUserActivity(userActivityDto);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", new { id = response.CreatedId });
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }

        // GET: UserActivityPage/Edit/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var userActivity = await _userActivityService.FindUserActivity(id);
            var users = await _userService.GetUsers();
            var activities = await _activityService.ListActivities();
            var pets = await _petService.ListPets();
            var items = await _itemService.GetItems();

            if (userActivity == null)
            {
                return View("Error", new ErrorViewModel { Errors = ["Could not find UserActivity."] });
            }

            UserActivityEdit options = new()
            {
                UserActivity = userActivity,
                AllUsers = users.Select(u => new SelectListItem
                {
                    Value = u.Id.ToString(),
                    Text = u.Username
                }),

                AllActivities = activities.Select(a => new SelectListItem
                {
                    Value = a.ActivityId.ToString(),
                    Text = a.ActivityName
                }),
                AllPets = pets.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }),

                AllItems = items.Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name
                })
            };

            return View(options);
        }

        // POST: UserActivityPage/Update/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromForm] UserActivityDto userActivityDto)
        {
            if (id != userActivityDto.UserActivityId)
            {
                return BadRequest(new { message = "UserActivity ID mismatch." });
            }

            var response = await _userActivityService.UpdateUserActivity(id, userActivityDto);

            if (response.Status == ServiceResponse.ServiceStatus.NotFound)
            {
                return NotFound(new { message = "UserActivity not found." });
            }

            if (response.Status == ServiceResponse.ServiceStatus.Error)
            {
                return BadRequest(new { message = "Error updating UserActivity." });
            }

            return RedirectToAction("Edit", new { id = userActivityDto.UserActivityId, success = true, message = "User activity updated successfully!" });
        }

        // GET: UserActivityPage/ConfirmDelete/{id}
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var userActivity = await _userActivityService.FindUserActivity(id);
            if (userActivity == null)
            {
                return View("Error", new ErrorViewModel { Errors = ["UserActivity not found."] });
            }

            return View(userActivity);
        }

        // POST: UserActivityPage/Delete/{id}
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _userActivityService.DeleteUserActivity(id);

            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List");
            }

            return View("Error", new ErrorViewModel { Errors = response.Messages });
        }
    }
}
