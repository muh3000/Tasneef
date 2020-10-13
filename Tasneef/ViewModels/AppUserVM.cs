using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasneef.ViewModels
{
    public class AppUserVM
    {
        public string Id { set; get; }
        public string Name { get; set; }
        public int? CustomerId { set; get; }
        public string CustomerName { set; get; }
        public string Username { set; get; }
        public string Password { set; get; }
        public string Mobile { set; get; }
        public string RoleId { set; get; }
        public string RoleName { set; get; }

    }
}
