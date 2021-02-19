using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class EmployeeCustomerGrant
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public int CustomerId { get; set; }


        public AppUser Employee { set; get; }

        public Customer Customer { set; get; }

    }
}
