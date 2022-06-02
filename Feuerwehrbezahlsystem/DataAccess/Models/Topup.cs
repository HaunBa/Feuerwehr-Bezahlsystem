using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Topup
    {
        public Topup()
        {
            Users = new HashSet<User>();
        }

        public int TopupId { get; set; }
        public DateTime TopupDate { get; set; }
        public string Text { get; set; } = null!;
        public int TopupAmount { get; set; }
        public string TopupExecutorId { get; set; } = null!;

        public virtual User TopupExecutor { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
