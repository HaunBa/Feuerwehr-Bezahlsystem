using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            Topups = new HashSet<Topup>();
            Payments = new HashSet<Payment>();
            TopupsNavigation = new HashSet<Topup>();
        }

        public string UserId { get; set; } = null!;
        public int Balance { get; set; }
        public string? Comment { get; set; }
        public DateTime? OpenCheckoutDate { get; set; }
        public string? OpenCheckoutAmount { get; set; }

        public virtual ICollection<Topup> Topups { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Topup> TopupsNavigation { get; set; }
    }
}
