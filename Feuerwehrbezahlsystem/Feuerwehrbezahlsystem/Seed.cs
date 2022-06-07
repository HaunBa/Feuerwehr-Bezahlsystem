using DataAccess.Models;

namespace Feuerwehrbezahlsystem
{
    public class Seed
    {
        public static async Task EnsureSeedData()
        {
            PaymentsystemContext _context = new();
            if (!_context.Prices.Any())
            {
                var price1 = new Price
                {
                    PriceSinceDate = DateTime.Now.AddDays(-1),
                    PriceUntilDate = DateTime.Now.AddMonths(2),
                    PriceValue = 1
                };

                var price2 = new Price
                {
                    PriceSinceDate = DateTime.Now.AddDays(-1),
                    PriceUntilDate = DateTime.Now.AddMonths(2),
                    PriceValue = 2
                };

                _context.Prices.Add(price1);
                _context.Prices.Add(price2);

                await _context.SaveChangesAsync();
            }

            if (!_context.Articles.Any())
            {
                var art1 = new Article
                {
                    ArticleAmount = 10.ToString(),
                    ArticleName = "Freistädter",
                    Price = _context.Prices.First(x => x.PriceValue == 1)                    
                };

                var art2 = new Article
                {
                    ArticleAmount = 10.ToString(),
                    ArticleName = "ZM",
                    Price = _context.Prices.First(x => x.PriceValue == 1)
                };

                _context.Articles.Add(art1);
                _context.Articles.Add(art2);

                await _context.SaveChangesAsync();
            }
        }
    }
}
