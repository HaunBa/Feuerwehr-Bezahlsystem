using System.ComponentModel.DataAnnotations;
using static TestingApp.Extensions;

namespace TestingApp.ViewModels
{
    public class ArticleWithPriceVM
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Anzahl")]
        public int Amount { get; set; }
        [Display(Name = "Artikel Foto")]
        public byte[] ImageData { get; set; }
        [Display(Name = "Artikel Typ")]
        public ArtType Type { get; set; }

        public int PriceId { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        [Display(Name = "Preis")]
        public double PriceAmount { get; set; }
    }
}
