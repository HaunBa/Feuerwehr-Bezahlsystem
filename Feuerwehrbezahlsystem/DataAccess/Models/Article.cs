using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Article
    {
        public Article()
        {
            PaymentPayments = new HashSet<Payment>();
        }

        public int ArticleId { get; set; }
        public string ArticleName { get; set; } = null!;
        public string ArticleAmount { get; set; } = null!;
        public int PriceId { get; set; }

        public virtual Price Price { get; set; } = null!;

        public virtual ICollection<Payment> PaymentPayments { get; set; }
    }
}
