using DataAccess.Models;
using Feuerwehrbezahlsystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Feuerwehrbezahlsystem
{
    public class Seed
    {
        private readonly ApplicationDbContext _appContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public Seed(ApplicationDbContext appContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _appContext = appContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public ApplicationDbContext Get_appContext()
        {
            return _appContext;
        }

        public RoleManager<IdentityRole> Get_roleManager()
        {
            return _roleManager;
        }

        public UserManager<IdentityUser> Get_userManager()
        {
            return _userManager;
        }

        public async Task EnsureSeedData()
        {               
            PaymentsystemContext _context = new();

            if (!_context.Prices.Any())
            {
                var price1 = new Price
                {
                    PriceSinceDate = DateTime.Now.AddDays(-1),
                    PriceUntilDate = DateTime.Now.AddMonths(2),
                    PriceValue = 1
                };

                var price2 = new Price
                {
                    PriceSinceDate = DateTime.Now.AddDays(-1),
                    PriceUntilDate = DateTime.Now.AddMonths(2),
                    PriceValue = 2
                };

                _context.Prices.Add(price1);
                _context.Prices.Add(price2);

                await _context.SaveChangesAsync();
            }

            if (!_context.Articles.Any())
            {
                var art1 = new Article
                {
                    ArticleAmount = 10.ToString(),
                    ArticleName = "Freistädter",
                    Price = _context.Prices.First(x => x.PriceValue == 1)                    
                };

                var art2 = new Article
                {
                    ArticleAmount = 10.ToString(),
                    ArticleName = "ZM",
                    Price = _context.Prices.First(x => x.PriceValue == 1)
                };

                _context.Articles.Add(art1);
                _context.Articles.Add(art2);

                await _context.SaveChangesAsync();
            }

            if (!_appContext.UserRoles.Any())
            {
                var adminRole = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                var userRole = await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            if (!_appContext.Users.Any())
            {
                var cUser = new IdentityUser("Admin")
                {
                    Email = "admin@admin.com",
                    LockoutEnabled = false
                };

                var tmp = _userManager.PasswordHasher.HashPassword(cUser, "Admin123!");

                var adminUser = await _userManager.CreateAsync(cUser);
            }
        }
    }
}
