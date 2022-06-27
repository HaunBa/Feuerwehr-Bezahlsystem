using System.ComponentModel.DataAnnotations;

namespace TestingApp.ViewModels
{
    public class PaymentVM
    {
        public int PaymentId { get; set; }
        public string PersonId { get; set; }
        public ApplicationUser Person { get; set; }
        [Display(Name="Datum")]
        public DateTime Date { get; set; }
        [Display(Name ="Beschreibung")]
        public string Description { get; set; }
        [Display(Name ="Endpreis")]
        public double CashAmount { get; set; }
        public virtual List<BoughtArticle> Articles { get; set; } = new();
        public string ExecutorId { get; set; }
        public ApplicationUser Executor { get; set; }
    }
}
