namespace TestingApp.Models
{
    public class TopUp
    {
        public int TopUpId { get; set; }
        public int PersonId { get; set; }
        public ApplicationUser Person { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public double CashAmount { get; set; }
    }
}