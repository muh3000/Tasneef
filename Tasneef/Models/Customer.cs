using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class Customer:AuditableEntity
    {
        public int Id { set; get; }
        public string  Name { get; set; }

        public ICollection<AppUser> Users { set; get; }

        
    }
}
