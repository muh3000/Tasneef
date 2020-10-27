using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class Upload:AuditableEntity
    {
        public int Id { get; set; }

        public int? MessageId { set; get; }
        public Message Message { set; get; }

        public int? ProjectId { set; get; }
        public Project Project { set; get; }

        public int? CustomerId { set; get; }
        public Customer Customer { set; get; }

        public bool IsForm { set; get; }

        public string FilePath { set; get; }
        public string Tag { set; get; }

    }
}

