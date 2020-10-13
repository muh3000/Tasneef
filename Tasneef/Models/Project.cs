using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class Project:AuditableEntity
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ClosedDate { get; set; }

        public int CustomerId { set; get; }
        public Customer Customer { set; get; }
        public ICollection<Message> Messages { get; set; }
        public string Name { set; get; }

    }
}
