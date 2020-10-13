using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class CustomerUser:AuditableEntity
    {
        public int Id { set; get; }
        public int CustomerId { set; get; }
        public string UserId { set; get; }
        public AppUser User { set; get; } 

    }
}
