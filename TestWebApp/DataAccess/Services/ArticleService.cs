namespace DataAccess.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ApplicationDbContext _context;
        public ArticleService(ApplicationDbContext db)
        {
            _context = db;
        }

        public State AddArticle(Article article)
        {
            if (!_context.Articles.Any(x => x.Name == article.Name))
            {
                _context.Articles.Add(article);
                return State.Successful;
            }

            return State.Error;
        }

        public State DeleteArticle(int id)
        {
            if (!_context.Articles.Any(x => x.Id == id))
            {
                var article = GetArticle(id);
                if(article != null) _context.Articles.Remove(article);
                _context.SaveChanges();
                return State.Successful;
            }

            return State.Error;
        }

        public Article? GetArticle(int id)
        {
            return _context.Articles.FirstOrDefault(x => x.Id == id);
        }

        public List<Article> GetArticles()
        {
            return _context.Articles.Include(x => x.Price).ToList();
        }

        public State UpdateArticle(int id, Article article)
        {
            var art = GetArticle(id);
            if (art != null)
            {
                art = article;
                art.Id = id;

                _context.Articles.Update(art);
                _context.SaveChanges();
                return State.Successful;
            }

            return State.Error;
        }
    }
}
