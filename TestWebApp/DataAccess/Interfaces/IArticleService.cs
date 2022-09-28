namespace DataAccess.Interfaces
{
    public interface IArticleService
    {
        public List<Article> GetArticles();
        public State UpdateArticle(int id, Article article);
        public Article? GetArticle(int id);
        public State DeleteArticle(int id);
        public State AddArticle(Article article);
    }
}
