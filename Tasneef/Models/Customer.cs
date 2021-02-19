using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class Customer
    {
        public int Id { set; get; }
        public string  Name { get; set; }

        
        public List<Subscription> Subscriptions { set; get; }
        public List<CustomerUser> CustomerUsers { set; get; }
    }
}
