namespace TestingApp.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceId { get; set; }
        public Price Price { get; set; }
        public int Amount { get; set; }
        public byte[] ImageData { get; set; }
        public string Type { get; set; }
    }
}
