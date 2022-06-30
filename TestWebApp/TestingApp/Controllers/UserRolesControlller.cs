using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApp.Data;
using TestingApp.ViewModels;

namespace TestingApp.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class UserRolesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserRolesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
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
                thisViewModel.Username = user.UserName;
                thisViewModel.FirstName = user.FirstName;
                thisViewModel.LastName = user.LastName;
                thisViewModel.Role = role[0];
                thisViewModel.Balance = user.Balance;
                thisViewModel.Comment = user.Comment;
                userRoles.Add(thisViewModel);
            }

            return View(userRoles);
        }

        private async Task<List<string>> GetUserRoles(ApplicationUser user)
        {
            return new List<string>(await _userManager.GetRolesAsync(user));
        }

        [Authorize(Roles = "User, Admin, SuperAdmin")]

        public async Task<IActionResult> Details(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var role = await _userManager.GetRolesAsync(user);

            var vm = new UserWithAllInfosVM();
            vm.Id = user.Id;
            vm.Username = user.UserName;
            vm.FirstName = user.FirstName;
            vm.LastName = user.LastName;
            vm.Role = role[0];
            vm.Payments = await _context.Payments.Include(x => x.Executor).Where(x => x.PersonId == user.Id).ToListAsync();
            vm.TopUps = await _context.TopUps.Include(x => x.Executor).Where(x => x.PersonId == user.Id).ToListAsync();
            vm.Comment = user.Comment;
            vm.Balance = user.Balance;

            return View(vm);
        }

        public async Task<IActionResult> PaymentDetails (int paymentId)
        {
            var payment = await _context.Payments.Include(x => x.Articles).ThenInclude(x => x.Price).FirstOrDefaultAsync(x => x.PaymentId == paymentId);
            if (payment == null) return NotFound();

            var vm = new PaymentVM
            {
                PaymentId = paymentId,
                Articles = payment.Articles,
                CashAmount = payment.CashAmount,
                Date = payment.Date,
                Description = payment.Description,
                Person = payment.Person,
                PersonId = payment.PersonId
            };

            return View(vm);
        }

        public async Task<IActionResult> TopUpDetails(int topupid)
        {
            var topup = await _context.TopUps.Include(x => x.Executor).FirstOrDefaultAsync(x => x.TopUpId == topupid);
            if (topup == null) return NotFound();

            var vm = new PaymentVM
            {
                PaymentId = topupid,
                CashAmount = topup.CashAmount,
                Date = topup.Date,
                Description = topup.Description,
                Executor = topup.Executor,
                ExecutorId = topup.ExecutorId,
                Person = topup.Person,
                PersonId = topup.PersonId
            };

            return View(vm);
        }

        // GET:
        public async Task<IActionResult> Edit(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null) return NotFound();

            var role = await _userManager.GetRolesAsync(user);

            var thisViewModel = new UserRolesViewModel();
            thisViewModel.Id = user.Id;
            thisViewModel.Username = user.UserName;
            thisViewModel.FirstName = user.FirstName;
            thisViewModel.LastName = user.LastName;
            thisViewModel.Role = role[0];
            thisViewModel.Comment = user.Comment;
            thisViewModel.Balance = user.Balance;
            thisViewModel.OpenCheckoutDate = user.OpenCheckoutDate;
            thisViewModel.OpenCheckoutValue = user.OpenCheckoutValue;

            return View(thisViewModel);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Balance,Comment,OpenCheckoutDate,OpenCheckoutValue,Role")] UserRolesViewModel userRolesVm)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var executor = await _userManager.FindByNameAsync(User.Identity.Name);

            user.FirstName = userRolesVm.FirstName;
            user.LastName = userRolesVm.LastName;

            double dif = userRolesVm.Balance - user.Balance;

            // topup
            if (dif < 0)
            {
                var payment = new Payment
                {
                    CashAmount = dif * (-1),
                    Date = DateTime.Now,
                    Description = $"Payment on {DateTime.Now.ToString("g")} over {dif * (-1)}",
                    PersonId = user.Id,
                    ExecutorId = executor.Id
                };

                user.Balance += dif;

                user.Payments.Add(payment);
            }
            // top up
            else if (dif > 0)
            {
                var topUp = new TopUp
                {
                    CashAmount = dif,
                    Date = DateTime.Now,
                    Description = $"Payment on {DateTime.Now.ToString("g")} over {dif} €",
                    PersonId = user.Id,
                    Person = user,
                    ExecutorId = executor.Id,
                    Executor = executor
                };

                user.Balance += dif;
                user.TopUps.Add(topUp);
            }
            // do nothing
            else
            {

            }


            user.Comment = userRolesVm.Comment;
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));

            await _userManager.AddToRoleAsync(user, userRolesVm.Role);

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
