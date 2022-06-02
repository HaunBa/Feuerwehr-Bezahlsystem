using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Payment
    {
        public Payment()
        {
            ArticleArticles = new HashSet<Article>();
            Users = new HashSet<User>();
        }

        public int PaymentId { get; set; }
        public string PaymentDate { get; set; } = null!;
        public string PaymentText { get; set; } = null!;
        public string PaymentTotal { get; set; } = null!;

        public virtual ICollection<Article> ArticleArticles { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
