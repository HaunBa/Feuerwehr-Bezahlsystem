using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApp.Data;
using TestingApp.Helpers;
using TestingApp.ViewModels;

namespace TestingApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var articles = await (from article in _context.Articles.Include(x => x.Price)
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
    }
}
