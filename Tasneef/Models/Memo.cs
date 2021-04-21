using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class Memo:AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body  { get; set; }

        public List<CustomerMemo> CustomerMemos { set; get; }


    }
}
