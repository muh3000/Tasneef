using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class CustomerUser
    {
        public int Id { set; get; }
        public int CustomerId { set; get; }
        public Customer Customer { set; get; }
        public string UserId { set; get; }
        public AppUser User { set; get; } 

    }
}
