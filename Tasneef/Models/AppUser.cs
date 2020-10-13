using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Identity;

namespace Tasneef.Models
{
    public class AppUser: IdentityUser
    {
        public string Name { set; get; }

        public bool Active { set; get; }
        //[ForeignKey("Customer")]
        public int? CustomerId { set; get; }

        [NotMapped]
        public Customer Customer { set; get; }
    }
}
