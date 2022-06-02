using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Price
    {
        public Price()
        {
            Articles = new HashSet<Article>();
        }

        public int PriceId { get; set; }
        public float PriceValue { get; set; }
        public DateTime PriceSinceDate { get; set; }
        public DateTime? PriceUntilDate { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
    }
}
