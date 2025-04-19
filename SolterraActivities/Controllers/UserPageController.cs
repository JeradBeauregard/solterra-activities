using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolterraActivities.Interfaces;
using SolterraActivities.Models;

namespace SolterraActivities.Controllers
{
    
    public class UserPageController : Controller
    {
        // Dependency Injection	
        private readonly IUserService _userService;
        private readonly IInventoryService _inventoryService;
        private readonly IItemService _itemService;
        private readonly IPetService _petService;

        public UserPageController(IUserService userService, IInventoryService inventoryService, IItemService itemService, IPetService petService)
        {
            _userService = userService;
            _inventoryService = inventoryService;
            _itemService = itemService;
            _petService = petService;
        }

        // List
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> List()
        {
            IEnumerable<User> Users = await _userService.GetUsers();
            return View(Users);
        }

        // New (Create)
        [HttpGet]
        [Authorize]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(string username, string password)
        {
            await _userService.PostUser(username, password);
            return RedirectToAction("List");
        }

        // Add to user inventory
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToInventory(int userId, int itemId, int quantity)
        {
            string result = await _inventoryService.AddToInventory(userId, itemId, quantity);
            return RedirectToAction("Details", new { id = userId });
        }

        // Update active Pet
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateActivePet(int UserId, int petId)
        {
            string result = await _userService.UpdateActivePet(UserId, petId);
            return RedirectToAction("Details", new { id = UserId });
        }

        // Details
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            User user = await _userService.GetUser(id);
            IEnumerable<InventoryDto> inventory = await _inventoryService.ListUserInventory(id);
            IEnumerable<Item> items = await _itemService.GetItems();
            IEnumerable<PetDto> pets = await _petService.ListUserPets(id);

            items = items.OrderBy(i => i.Name);

            UserDetailsViewModel userDetails = new UserDetailsViewModel
            {
                User = user,
                Inventory = inventory,
                AllItems = items,
                Pets = pets
            };

            return View(userDetails);
        }

        // Use item on pet
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UseItemOnPet(int userId, int petId, int itemId)
        {
            string result = await _inventoryService.UseItemOnPet(userId, petId, itemId);
            return RedirectToAction("Details", new { id = userId });
        }

        // ConfirmDelete
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ConfirmDelete(int Id)
        {
            User user = await _userService.GetUser(Id);
            return View(user);
        }

        // Delete
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(int Id)
        {
            await _userService.DeleteUser(Id);
            return RedirectToAction("List");
        }

        // Edit
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            User user = await _userService.GetUser(id);
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditUser(int id, string username, string password, int solshards)
        {
            await _userService.EditUser(id, username, password, solshards);
            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateQuantity(int id, int quantity)
        {
            await _inventoryService.UpdateQuantity(id, quantity);
            InventoryDto UserId = await _inventoryService.ListInventory(id);

            return RedirectToAction("Details", new { id = UserId.UserId });
        }
    }
}
