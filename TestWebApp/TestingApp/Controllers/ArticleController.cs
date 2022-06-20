using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestingApp.Data;
using TestingApp.ViewModels;

namespace TestingApp.Controllers
{
    [Authorize(Roles = "User, Admin, SuperAdmin")]
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ArticleController(ApplicationDbContext context)
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

            return View(articles);
        }

        public async Task<IActionResult> Create()
        {
            return View(new ArticleWithPriceVM());
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Amount,ImageData,Type,Priceid,Since,Until,PriceAmount")]ArticleWithPriceVM articleVm)
        {
            if (Request.Form.Files.Count > 0)
            {
                using (var dataStream = new MemoryStream())
                {
                    IFormFile? file = Request.Form.Files.FirstOrDefault();
                    await file.CopyToAsync(dataStream);
                    articleVm.ImageData = dataStream.ToArray();
                }
            }
            else
            {
                articleVm.ImageData = Array.Empty<byte>();
            }

            

            if (ModelState.ErrorCount <= 1)
            {
                var fprice = await _context.Prices.FirstOrDefaultAsync(x => x.Amount == articleVm.PriceAmount);
                if(fprice == null)
                {
                    fprice = new Price
                    {
                        Amount = articleVm.PriceAmount,
                        Since = DateTime.Now
                    };
                    _context.Prices.Add(fprice);
                    await _context.SaveChangesAsync();
                }
                
                fprice = await _context.Prices.FirstAsync(x => x.Amount == fprice.Amount);
                
                var art = new Article
                {
                    Amount = articleVm.Amount,
                    ImageData = articleVm.ImageData,
                    Name = articleVm.Name,
                    Type = articleVm.Type,
                    PriceId = fprice.PriceId
                };

                var res = await _context.Articles.AddAsync(art);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int articleId)
        {
            var art = await (from article in _context.Articles.Include(x => x.Price)
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
                             }).FirstOrDefaultAsync(x => x.Id == articleId);

            if(art == null) return NotFound();
            return View(art);            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Amount,ImageData,Type,Priceid,Since,Until,PriceAmount")] ArticleWithPriceVM articleVm)
        {
            var art = await _context.Articles.FirstOrDefaultAsync(x => x.Id == id);
            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    articleVm.ImageData = dataStream.ToArray();
                }
            }else if (art != null && art.ImageData != Array.Empty<byte>())
            {
                articleVm.ImageData = art.ImageData;
            }
            else
            {
                articleVm.ImageData = Array.Empty<byte>();
            }

            if (ModelState.ErrorCount <= 1)
            {
                var fprice = await _context.Prices.FirstOrDefaultAsync(x => x.Amount == articleVm.PriceAmount);

                if (fprice == null)
                {
                    fprice = new Price
                    {
                        Amount = articleVm.PriceAmount,
                        Since = DateTime.Now
                    };
                    _context.Prices.Add(fprice);
                    await _context.SaveChangesAsync();
                }

                fprice = await _context.Prices.FirstAsync(x => x.Amount == fprice.Amount);

                art.Amount = articleVm.Amount;
                art.ImageData = articleVm.ImageData;
                art.Name = articleVm.Name;
                art.Type = articleVm.Type;
                art.PriceId = fprice.PriceId;

                var res = _context.Articles.Update(art);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id)
        {
            var art = _context.Articles.FirstOrDefault(x => x.Id == id);
            if (art != null)
            {
                _context.Articles.Remove(art);
                await _context.SaveChangesAsync();
            }

            return Redirect("~/Article");
        }
    }
}
