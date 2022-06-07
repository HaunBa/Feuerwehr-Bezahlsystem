using DataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace Feuerwehrbezahlsystem.ViewModels
{
    public class UserVM : IdentityUser
    {
        public string UserId { get; set; } = null!;
        public int Balance { get; set; }
        public string? Comment { get; set; }
        public DateTime? OpenCheckoutDate { get; set; }
        public string? OpenCheckoutAmount { get; set; }

        public List<string> Roles { get; set; }

        public ICollection<Topup> Topups { get; set; }

        public ICollection<Payment> Payments { get; set; }
        public ICollection<Topup> TopupsNavigation { get; set; }
    }
}
