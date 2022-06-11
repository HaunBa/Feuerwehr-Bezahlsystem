using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApp.Data;
using TestingApp.Helpers;
using TestingApp.ViewModels;

namespace TestingApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("index")]
        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<ArticleWithPriceVM>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.total = cart.Sum(item => item.PriceAmount * item.Amount);
            return View();
        }

        [Route("buy/{id}")]
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
                                                     }).FirstOrDefaultAsync();

            if (SessionHelper.GetObjectFromJson<List<ArticleWithPriceVM>>(HttpContext.Session, "cart") == null)
            {
                List<ArticleWithPriceVM> cart = new List<ArticleWithPriceVM>();
                cart.Add(new ArticleWithPriceVM 
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
                });

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
                    cart.Add(new ArticleWithPriceVM
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
                    });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            List<ArticleWithPriceVM> cart = SessionHelper.GetObjectFromJson<List<ArticleWithPriceVM>>(HttpContext.Session, "cart");
            int index = doesExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
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
