using DataAccess.Extensions;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static DataAccess.Extensions.Enums;

namespace Bezahlwebsite.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public Article GetArticle(int id)
        {
            return _articleService.GetArticle(id);
        }

        [HttpGet]
        public List<Article> GetAllArticles()
        {
            return _articleService.GetArticles();
        }

        [HttpPut]
        public State UpdateArticle(int id, Article article)
        {
            return _articleService.UpdateArticle(id, article);
        }

        [HttpDelete]
        public State DeleteArticle(int id)
        {
            return _articleService.DeleteArticle(id);
        }

        [HttpPost]
        public State AddArticle(Article article)
        {
            return _articleService.AddArticle(article);
        }
    }
}
