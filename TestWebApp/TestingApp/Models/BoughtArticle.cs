using static TestingApp.Extensions;

namespace TestingApp.Models
{
    public class BoughtArticle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceId { get; set; }
        public Price Price { get; set; }
        public int Amount { get; set; }
        public byte[] ImageData { get; set; }
        public ArtType Type { get; set; }
    }
}
