using System.ComponentModel.DataAnnotations;

namespace TestingApp.ViewModels
{
    public class UserRolesViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Benutzername")]
        public string Username { get; set; }
        [Display(Name = "Vorname")]
        public string FirstName { get; set; }
        [Display(Name = "Nachname")]
        public string LastName { get; set; }
        [Display(Name = "Kontostand")]
        public double Balance { get; set; }
        [Display(Name = "Kommentar")]
        public string Comment { get; set; }

        [Display(Name = "Datum der offenen Kasse")]
        public DateTime OpenCheckoutDate { get; set; }
        [Display(Name = "Maximalwert bei offener Kasse")]
        public double OpenCheckoutValue { get; set; }

        [Display(Name = "Rolle")]
        public string Role { get; set; }
    }
}
