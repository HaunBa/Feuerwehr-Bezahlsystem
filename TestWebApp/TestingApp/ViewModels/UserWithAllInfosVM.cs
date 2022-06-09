using System.ComponentModel.DataAnnotations;

namespace TestingApp.ViewModels
{
    public class UserWithAllInfosVM
    {
        public string Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Balance")]
        public double Balance { get; set; }
        [Display(Name = "Comment")]
        public string Comment { get; set; }

        [Display(Name = "Open Checkout Date")]
        public DateTime OpenCheckoutDate { get; set; }
        [Display(Name = "Open Checkout Value")]
        public double OpenCheckoutValue { get; set; }

        [Display(Name = "Role")]
        public string Role { get; set; }

        public List<Payment> Payments { get; set; }
        public List<TopUp> TopUps { get; set; }
    }
}
