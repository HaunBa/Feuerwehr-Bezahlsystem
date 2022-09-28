

namespace DataAccess.Models
{
    public class ApplicationUser : IdentityUser
    {        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Balance { get; set; }
        public string Comment { get; set; }
        
        public List<Payment> Payments { get; set; } = new List<Payment>();
        public List<TopUp> TopUps { get; set; } = new List<TopUp>();
        public DateTime OpenCheckoutDate { get; set; }
        public double OpenCheckoutValue { get; set; }
    }
}
