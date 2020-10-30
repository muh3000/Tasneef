using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class Subscription : AuditableEntity
    {
        public int Id { set; get; }
        public int CustomerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
