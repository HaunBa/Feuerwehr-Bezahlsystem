using DataAccess.Interfaces;
using DataAccess.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bezahlwebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoughtArticleController : ControllerBase
    {
        private readonly IBoughtArticleService _boughtArticleService;
        public BoughtArticleController(IBoughtArticleService boughtArticleService)
        {
            _boughtArticleService = boughtArticleService;
        }

        [HttpGet]
        public List<BoughtArticle>? GetAllBoughtArticlesFromUser(string userId)
        {
            return _boughtArticleService.GetAllBoughtArticlesFromUser(userId);
        }
    }
}
