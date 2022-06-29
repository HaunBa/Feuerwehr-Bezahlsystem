using Microsoft.AspNetCore.Identity;
using static TestingApp.Extensions;

namespace TestingApp.Data
{
    public class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Moderator.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            }
        }

        public static async Task SeedSuperAdminAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "superAdmin",
                LastName = "superAdmin",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (await userManager.FindByNameAsync("superadmin") == null)
            {
                var user = await userManager.FindByNameAsync(defaultUser.UserName);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin123!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }
            }
        }

        public static async Task SeedArticlesAsync(ApplicationDbContext context)
        {            
            if (!context.Articles.Any())
            {
                // add drink article

                var drinkPrice = new Price()
                {
                    Amount = 1,
                    Since = DateTime.Now
                };

                context.Prices.Add(drinkPrice);

                var imageData = Array.Empty<byte>();
                
                var drink = new Article()
                {
                    Amount = 100,
                    Name = "Freistädter Ratsherrn",
                    Price = drinkPrice,
                    Type = ArtType.Drink,
                    ImageData = imageData
                };

                //add food article

                var foodPrice = new Price()
                {
                    Amount = 2.5,
                    Since = DateTime.Now
                };

                context.Prices.Add(foodPrice);

                var food = new Article()
                {
                    Amount = 100,
                    Active = true,
                    ImageData = imageData,
                    Name = "Pizza Diavolo",
                    Type = ArtType.Food,
                    Price = foodPrice
                };

                // add else article

                var elsePrice = new Price()
                {
                    Amount = 0.5,
                    Since = DateTime.Now
                };

                context.Prices.Add(elsePrice);

                var elseArt = new Article()
                {
                    Amount = 100,
                    Name = "Gabel",
                    Active = true,
                    Price = elsePrice,
                    Type = ArtType.Else,
                    ImageData = imageData
                };

                context.Articles.Add(food);
                context.Articles.Add(drink);
                context.Articles.Add(elseArt);

                var res = await context.SaveChangesAsync();
                Console.WriteLine($"--- Total added Articles: {res}");
            }
        }

        public static async Task SeedBasicUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await userManager.FindByNameAsync("basicUser") ! == null)
            {
                //Seed Default User
                var defaultUser = new ApplicationUser
                {
                    UserName = "basicUser",
                    Email = "basicUser@gmail.com",
                    FirstName = "basicUser",
                    LastName = "basicUser",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };

                await userManager.CreateAsync(defaultUser, "Admin123!");

                await userManager.AddToRoleAsync(defaultUser, "User");
            }
        }
    }
}
