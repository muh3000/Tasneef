using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class Message:AuditableEntity
    {
        public int Id { get; set; }
        public int ProjectId { set; get;}

        public Project Project { set; get; }
        public string Body { set; get; }
    }
}
