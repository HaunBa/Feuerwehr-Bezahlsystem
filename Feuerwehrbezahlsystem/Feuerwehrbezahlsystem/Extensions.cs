using DataAccess.Models;
using Feuerwehrbezahlsystem.Data;
using Feuerwehrbezahlsystem.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Feuerwehrbezahlsystem
{
    public class Extensions
    {
        private readonly ApplicationDbContext _appContext;
        private readonly PaymentsystemContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public Extensions(ApplicationDbContext appContext, PaymentsystemContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _appContext = appContext;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<UserVM> GetUserVM(string id)
        {            
            var fUser = _appContext.Users.FirstOrDefault(x => x.Id == id);
            var user = _context.Users.FirstOrDefault(x => x.UserId == id);
            var role = await _userManager.GetRolesAsync(fUser);

            if (fUser == null) return null;
            if (user == null) return null;

            var userVM = new UserVM
            {
                Roles = role.ToList(),
                AccessFailedCount = fUser.AccessFailedCount,
                OpenCheckoutAmount = user.OpenCheckoutAmount,
                Balance = user.Balance,
                Comment = user.Comment,
                ConcurrencyStamp = fUser.ConcurrencyStamp,
                Email = fUser.Email,
                EmailConfirmed = fUser.EmailConfirmed,
                Id = fUser.Id,
                LockoutEnabled = fUser.LockoutEnabled,
                LockoutEnd = fUser.LockoutEnd,
                NormalizedEmail = fUser.NormalizedEmail,
                NormalizedUserName = fUser.NormalizedUserName,
                OpenCheckoutDate = user.OpenCheckoutDate,
                PasswordHash = fUser.PasswordHash,
                Payments = user.Payments,
                PhoneNumber = fUser.PhoneNumber,
                PhoneNumberConfirmed = fUser.PhoneNumberConfirmed,
                SecurityStamp = fUser.SecurityStamp,
                Topups = user.Topups,
                TopupsNavigation = user.TopupsNavigation,
                TwoFactorEnabled = fUser.TwoFactorEnabled,
                UserName = fUser.UserName
            };

            return userVM;
        }
    }
}
