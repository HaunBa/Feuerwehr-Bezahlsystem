namespace DataAccess.Models
{
    public class Price
    {
        public int PriceId { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        public double Amount { get; set; }
    }
}