using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestingApp.Controllers
{
    public class RoleManagerController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<IdentityRole> _logger;

        public RoleManagerController(RoleManager<IdentityRole> roleManager, ILogger<IdentityRole> logger)
        {
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            _logger.LogInformation("Add Role Called");
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
                _logger.LogInformation("Role Created");
            }
            else
            {
                _logger.LogError("Rolename null");
            }
            return RedirectToAction("Index");
        }
    }
}
