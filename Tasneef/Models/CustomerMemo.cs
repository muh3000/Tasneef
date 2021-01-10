using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class CustomerMemo:AuditableEntity
    {
        public int Id { get; set; }

        public int CustomerId { set; get; }
        public Customer Customer { get; set; }

        public Boolean IsRead { set; get; }

        public DateTime ReadDate { set; get; }

        public int? SubscriptionId { set; get; }
        public Subscription Subscription { set; get; }

        public int MemoId { set; get; }
        public Memo Memo { set; get; }

    }
}
