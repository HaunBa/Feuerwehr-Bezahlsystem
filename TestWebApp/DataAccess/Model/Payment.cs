namespace DataAccess.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public string PersonId { get; set; }
        public ApplicationUser Person { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double CashAmount { get; set; }
        public virtual List<BoughtArticle> Articles { get; set; } = new List<BoughtArticle>();        
        public string? ExecutorId { get; set; }
        public ApplicationUser? Executor { get; set; }
    }
}