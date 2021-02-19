using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Tasneef.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class WebServicesController : Controller
    {
        private IApplicationLifetime ApplicationLifetime { get; set; }

        public WebServicesController(IApplicationLifetime appLifetime)
        {
            ApplicationLifetime = appLifetime;
        }

        public async Task ShutdownSite()
        {
            ApplicationLifetime.StopApplication();
            //return "Done";
        }

    }
}

