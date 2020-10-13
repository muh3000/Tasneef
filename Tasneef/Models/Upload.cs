using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class Upload:AuditableEntity
    {
        public int Id { get; set; }

        public int MessageId { set; get; }
        public Message Message { set; get; }

        public string FilePath { set; get; }
        public string Tag { set; get; }

    }
}

