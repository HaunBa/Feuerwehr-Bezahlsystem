namespace DataAccess.Interfaces
{
    public interface IBoughtArticleService
    {
        public List<BoughtArticle>? GetAllBoughtArticlesFromUser(string userId);
    }
}
