namespace DataAccess.Services
{
    public class BoughtArticleService : IBoughtArticleService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;

        public BoughtArticleService(IUserService userService, ApplicationDbContext db)
        {
            _userService = userService;
            _db = db;
        }

        public List<BoughtArticle>? GetAllBoughtArticlesFromUser(string userId)
        {
            var user = _userService.GetUserById(userId);
            if(user == null) return null;

            List<BoughtArticle> boughtArticles = new();

            var boughtArticleLists = (from ba in user.Payments
                                  select ba.Articles).ToList();

            foreach (var ba in boughtArticleLists)
            {
                boughtArticles.AddRange(ba);
            }

            return boughtArticles;
        }
    }
}
