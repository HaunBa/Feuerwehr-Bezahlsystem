using System.ComponentModel.DataAnnotations;

namespace TestingApp.ViewModels
{
    public class ArticleWithPriceVM
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "Amount")]
        public int Amount { get; set; }
        [Display(Name = "Article Image")]
        public byte[] ImageData { get; set; }
        [Display(Name = "Article type")]
        public string Type { get; set; }

        public int PriceId { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }
        [Display(Name = "Price Amount")]
        public double PriceAmount { get; set; }
    }
}
