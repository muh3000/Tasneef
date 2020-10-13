using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class AuditableEntity
    {
        public string  CreatedById { get; set; }
        public AppUser CreatedBy { get; set; }

        public DateTime CreatedDate { set; get; }

        public string? UpdatedById { get; set; }
        public AppUser? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { set; get; }

    }

}
