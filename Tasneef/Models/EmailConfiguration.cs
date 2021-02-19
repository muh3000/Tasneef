using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.Models
{
    public class EmailConfiguration
    {
        public string Id { set; get; }
        public string FromEmail { set; get; }
        public string SenderName { set; get; }
        public string SMTPUername { set; get; }
        public string SMTPPassword { set; get; }
        public string SMTPServer { set; get; }
        public int SMTPPort { set; get; }
        public bool EnableSSL { set; get; }
    }
}
