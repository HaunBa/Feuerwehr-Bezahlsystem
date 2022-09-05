using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApp.Data;
using TestingApp.Helpers;
using TestingApp.ViewModels;

namespace TestingApp.Controllers
{
    [Authorize(Roles = "User,Admin,SuperAdmin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<VendingHub, IVendingHub> _hubContext;

        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHubContext<VendingHub, IVendingHub> hubContext)
        {
            _context = context;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await    (from article in _context.Articles.Include(x => x.Price)
                                    select new ArticleWithPriceVM
                                    {
                                        Amount = article.Amount,
                                        PriceAmount = article.Price.Amount,
                                        Id = article.Id,
                                        ImageData = article.ImageData,
                                        Name = article.Name,
                                        PriceId = article.PriceId,
                                        Since = article.Price.Since,
                                        Until = article.Price.Until,
                                        Type = article.Type
                                    }).ToListAsync();

            var cart = SessionHelper.GetObjectFromJson<List<ArticleWithPriceVM>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            if (cart == null) ViewBag.total = 0;
            else ViewBag.total = cart.Sum(item => item.PriceAmount * item.Amount);

            return View(articles);
        }

        public IActionResult Remove(int id)
        {
            List<ArticleWithPriceVM> cart = SessionHelper.GetObjectFromJson<List<ArticleWithPriceVM>>(HttpContext.Session, "cart");
            int index = doesExist(id);
            var art = new ArticleWithPriceVM();

            if (index >= 0)
            {
                art = cart[index];
            }

            if (cart[index].Amount > 1)
            {
                cart[index].Amount--;
            }
            else
            {
                cart.RemoveAt(index);
            }

            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            if (index >= 0)
            {
                return Redirect($"~/Products#{art.Name}");
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }
                
        public async Task<IActionResult> Purchase()
        {
            var cart = SessionHelper.GetObjectFromJson<List<ArticleWithPriceVM>>(HttpContext.Session, "cart");
            if (cart != null)
            {
                Dictionary<string, List<VendingItems>> vendingArticles = new();
                // payment functionality
                var articles = (from art in cart
                                select new BoughtArticle
                                {
                                    Amount = art.Amount,
                                    ImageData = art.ImageData,
                                    Name = art.Name,
                                    PriceId = art.PriceId,
                                    Type = art.Type
                                }).ToList();

                var tmp = _userManager.Users.Where(x => x.OpenCheckoutDate != DateTime.MinValue).ToList();
                /// open Checkout user
                var ocu = tmp.FirstOrDefault(x => DateOnly.FromDateTime(x.OpenCheckoutDate) == DateOnly.FromDateTime(DateTime.Now));
                
                if (ocu != null)
                {
                    // OPEN CHECKOUT
                    var currentDate = DateTime.Now;
                    var sum = articles.Sum(x => x.Amount * _context.Prices.First(y => y.PriceId == x.PriceId).Amount);
                    var currentUser = ocu;
                    currentUser.Balance -= sum;

                    var payment = new Payment()
                    {
                        Articles = articles,
                        CashAmount = sum,
                        Date = currentDate,
                        Description = $"Payment on {currentDate} over {sum} €",
                        Person = currentUser,
                        PersonId = currentUser.Id
                    };

                    currentUser.Payments.Add(payment);

                    await _context.SaveChangesAsync();

                    foreach (var item in articles)
                    {
                        var foundArt = await _context.Articles.FirstOrDefaultAsync(x => x.Id == item.Id);
                        if (foundArt != null && foundArt.Amount >= item.Amount)
                        {
                            foundArt.Amount -= item.Amount;

                            _context.Articles.Update(foundArt);
                        }
                    }
                }
                else
                {
                    // Normal Checkout

                    var currentDate = DateTime.Now;
                    var sum = articles.Sum(x => x.Amount * _context.Prices.First(y => y.PriceId == x.PriceId).Amount);
                    var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
                    currentUser.Balance -= sum;

                    var payment = new Payment()
                    {
                        Articles = articles,
                        CashAmount = sum,
                        Date = currentDate,
                        Description = $"Payment on {currentDate} over {sum} €",
                        Person = currentUser,
                        PersonId = currentUser.Id
                    };

                    currentUser.Payments.Add(payment);

                    await _context.SaveChangesAsync();

                    foreach (var item in articles)
                    {
                        var foundArt = await _context.Articles.FirstOrDefaultAsync(x => x.Id == item.Id);
                        if (foundArt != null && foundArt.Amount >= item.Amount)
                        {
                            foundArt.Amount -= item.Amount;

                            _context.Articles.Update(foundArt);
                        }
                    }
                }
                vendingArticles = CheckForVendingArticles(articles);

                foreach (var item in vendingArticles)
                {
                    await _hubContext.Clients.Groups(item.Key).EjectItem(item.Value);
                }

                await _context.SaveChangesAsync();

                // clear cart
                cart.Clear();
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }

            return RedirectToAction(nameof(Index));
        }

        private Dictionary<string, List<VendingItems>> CheckForVendingArticles(List<BoughtArticle> articles)
        {
            Dictionary<string, List<VendingItems>> Items = new();

            foreach (var article in articles)
            {
                if (article.IsInVending)
                {
                    if (Items.TryGetValue(article.VendingMachineNumber.ToString(), out var value))
                    {
                        value.Add(new VendingItems() { Amount = article.Amount, Slot = article.VendingSlot});
                    }
                    else
                    {
                        Items.Add(article.VendingMachineNumber.ToString(), new List<VendingItems>() { new VendingItems() { Amount = article.Amount, Slot = article.VendingSlot} });                        
                    }
                }
            }

            return Items;
        }

        public async Task<IActionResult> Buy(int id)
        {
            ArticleWithPriceVM? productModel = await (from article in _context.Articles.Include(x => x.Price)
                                                      select new ArticleWithPriceVM
                                                      {
                                                          Amount = article.Amount,
                                                          PriceAmount = article.Price.Amount,
                                                          Id = article.Id,
                                                          ImageData = article.ImageData,
                                                          Name = article.Name,
                                                          PriceId = article.PriceId,
                                                          Since = article.Price.Since,
                                                          Until = article.Price.Until,
                                                          Type = article.Type
                                                      }).FirstOrDefaultAsync(x => x.Id == id);

            var newArt = new ArticleWithPriceVM
            {
                Amount = 1,
                PriceAmount = productModel.PriceAmount,
                Id = productModel.Id,
                ImageData = productModel.ImageData,
                Name = productModel.Name,
                PriceId = productModel.PriceId,
                Since = productModel.Since,
                Type = productModel.Type,
                Until = productModel.Until
            };

            if (SessionHelper.GetObjectFromJson<List<ArticleWithPriceVM>>(HttpContext.Session, "cart") == null)
            {
                List<ArticleWithPriceVM> cart = new List<ArticleWithPriceVM>();

                cart.Add(newArt);

                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            }
            else
            {
                List<ArticleWithPriceVM> cart = SessionHelper.GetObjectFromJson<List<ArticleWithPriceVM>>(HttpContext.Session, "cart");
                int index = doesExist(id);
                if (index != -1)
                {
                    cart[index].Amount++;
                }
                else
                {
                    cart.Add(newArt);
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return Redirect($"~/Products#{newArt.Name}");
        }

        private int doesExist(int id)
        {
            List<ArticleWithPriceVM> cart = SessionHelper.GetObjectFromJson<List<ArticleWithPriceVM>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Id.Equals(id))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
