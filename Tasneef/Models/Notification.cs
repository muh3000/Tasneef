using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string  UserId { get; set; }
        public string Title { set; get; }

        public bool Read { set; get; }
        public DateTime CreatedDate { set; get; }
        public DateTime ReadDate { set; get; }
        public string Entity { set; get; }
        public string EntityId { set; get; }


    }
}
