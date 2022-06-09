namespace TestingApp.ViewModels
{
    public class PaymentVM
    {
        public int PaymentId { get; set; }
        public string PersonId { get; set; }
        public ApplicationUser Person { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double CashAmount { get; set; }
        public virtual List<Article> Articles { get; set; } = new List<Article>();
        public string ExecutorId { get; set; }
        public ApplicationUser Executor { get; set; }
    }
}
