using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApp.ViewModels;

namespace TestingApp.Controllers
{
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserRolesViewModel>();
            foreach (ApplicationUser user in users)
            {
                var role = await GetUserRoles(user);
                if (role.Count == 0)
                {
                    role.Add(" ");
                }

                var thisViewModel = new UserRolesViewModel();
                thisViewModel.Id = user.Id;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Role = role[0];
                userRoles.Add(thisViewModel);
            }

            return View(userRoles);
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        // GET:
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null) return NotFound();

            var role = await _userManager.GetRolesAsync(user);

            var thisViewModel = new UserRolesViewModel();
            thisViewModel.Id = user.Id;
            thisViewModel.FirstName = user.FirstName;
            thisViewModel.LastName = user.LastName;
            thisViewModel.Role = role[0];

            return View(thisViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Balance,Comment,OpenCheckoutDate,OpenCheckoutValue,Role")] UserRolesViewModel userRolesVm)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();



            return RedirectToAction(nameof(Index));
        }
    }
}
