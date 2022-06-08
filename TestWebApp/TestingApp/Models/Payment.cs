namespace TestingApp.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int PersonId { get; set; }
        public ApplicationUser Person { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double CashAmount { get; set; }
        public virtual List<Article> Articles { get; set; } = new List<Article>();
    }
}