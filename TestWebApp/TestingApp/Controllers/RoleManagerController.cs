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
                _logger.LogInformation($"Role {roleName} Created");
            }
            else
            {
                _logger.LogError("Rolename null");
            }
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await _roleManager.DeleteAsync(role);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(string? id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                return View(role);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,NormalizedName,ConcurrencyStamp")] IdentityRole role)
        {
            if (id != role.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var frole = await _roleManager.FindByIdAsync(role.Id);
                if (frole.Name != role.Name)
                {
                    frole.Name = role.Name;
                    frole.NormalizedName = frole.Name.ToUpper();
                    var res = await _roleManager.UpdateAsync(frole);
                }
                
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }
    }
}
